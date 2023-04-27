using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    /// <summary>
    /// Controls game logic, such updating the figures and the pinning and checking for check/checkmate.
    /// </summary>
    public sealed class Game
    {
        public delegate void OnMoveCompleted(List<Figure> pieces);
        public event OnMoveCompleted OnMoveCompletedEvent;

        public Board Board { get { return gameState.Board; } }
        public Color ActivePlayer => gameState.ActivePlayer;

        private readonly Match match;
        private readonly GameState gameState;

        public Game()
        {
            match = new Match();
            gameState = new GameState();
            gameState.SpawnFigures();
        }

        public void StartGame()
        {
            UpdateGameState();
            OnMoveCompletedEvent?.Invoke(gameState.Pieces);
        }

        public void Redo()
        {
            match.Redo(gameState);
            SwitchActivePlayer();
            UpdateGameState();
            OnMoveCompletedEvent?.Invoke(gameState.Pieces);
        }

        public void Undo()
        {
            match.Undo(gameState);
            SwitchActivePlayer();
            UpdateGameState();
            OnMoveCompletedEvent?.Invoke(gameState.Pieces);
        }

        public void ExecuteMove(Move move)
        {
            match.Add(move);
            Redo();
        }

        private void SwitchActivePlayer()
        {
            gameState.SwitchActivePlayer();
        }

        public void SerializeMatch()
        {
            match.Serialize();
        }

        public void DeserializeMatch()
        {
            match.Deserialize();
        }

        private void UpdateGameState()
        {
            gameState.Pieces.ForEach(piece => piece.ClearState());
            gameState.Pieces.ForEach(piece => piece.UpdatePositions());

            // Kings have to be updated last, because they cannot move to attacked tiles. 
            gameState.WhiteKing.UpdatePositions();
            gameState.BlackKing.UpdatePositions();

            gameState.Pieces.ForEach(piece => piece.UpdatePinned());

            // If the king is attacked no piece can move if it does not resolve the check.
            UpdateCheck(Color.White);
            UpdateCheck(Color.Black);

            UpdateCheckMate(Color.White);
            UpdateCheckMate(Color.Black);
        }

        private void UpdateCheck(Color kingColor)
        {
            King king = kingColor == Color.White ? gameState.WhiteKing : gameState.BlackKing;
            List<Figure> enemyPieces = kingColor == Color.White ? gameState.BlackPieces : gameState.WhitePieces;
            List<Figure> ownPieces = kingColor == Color.White ? gameState.WhitePieces : gameState.BlackPieces;

            List<Figure> attackingFigures = new List<Figure>();
            foreach (var piece in enemyPieces)
            {
                if (piece.AttacksPosition(king.Position))
                {
                    attackingFigures.Add(piece);
                    Debug.Log("Check!");
                }
            }

            // If the king is checked by two pieces simultaneously, no other move but moving the king is valid (Double Check)
            if (attackingFigures.Count >= 2)
            {
                foreach (var piece in ownPieces)
                {
                    if (piece is not King)
                    {
                        piece.ClearMoveablePositions();
                    }
                }
            }
            // If the king is checked by only one figure, only moves that kick the figure or set a figure in between (not for Knight or Pawn) are valid. 
            else if (attackingFigures.Count == 1)
            {
                Figure attackingFigure = attackingFigures[0];
                List<Vector2Int> validPositions = new List<Vector2Int>() { attackingFigure.Position };

                // If the figure is a queen, bishop or rook it is also possible to move a figure in between the king and the attacking figure. 
                if (attackingFigure is Queen || attackingFigure is Bishop || attackingFigure is Rook)
                {
                    List<Vector2Int> positionsBetween = attackingFigure.Position.GetPositionsBetween(king.Position);
                    validPositions.AddRange(positionsBetween);
                }

                foreach (var piece in ownPieces)
                {
                    if (piece is not King)
                    {
                        piece.ClearMoveablePositionsExcept(validPositions);
                    }
                }
            }
        }

        // The king is checkmated if he is in check and there are no valid moves to stop that. 
        // The game is a draw if there are no valid moves but the king is not in check. 
        private void UpdateCheckMate(Color color)
        {
            List<Figure> ownPieces = color == Color.White ? gameState.WhitePieces : gameState.BlackPieces;
            foreach (Figure piece in ownPieces)
            {
                if (piece.MoveablePositions.Count > 0)
                {
                    return;
                }
            }

            King king = color == Color.White ? gameState.WhiteKing : gameState.BlackKing;
            List<Figure> enemyPieces = color == Color.White ? gameState.BlackPieces : gameState.WhitePieces;
            foreach (var piece in enemyPieces)
            {
                if (piece.AttacksPosition(king.Position))
                {
                    Debug.Log("Checkmate!");
                    return;
                }
            }

            Debug.Log("Draw");
        }
    }
}