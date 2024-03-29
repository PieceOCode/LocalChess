using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Chess
{
    /// <summary>
    /// Controls game logic, such updating the figures and the pinning and checking for check/checkmate.
    /// </summary>
    public sealed class Game
    {
        public delegate void MoveCompletedEventHandler(List<Figure> pieces);
        public event MoveCompletedEventHandler MoveCompletedEvent;

        public delegate void GameEndedEventHandler(GameResult gameEnd);
        public event GameEndedEventHandler GameEndedEvent;

        public Board Board { get { return gameState.Board; } }
        public Color ActivePlayer => gameState.ActivePlayer;
        public Match Match => match;
        public GameState GameState => gameState;

        private Match match;
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
            MoveCompletedEvent?.Invoke(gameState.Pieces);
        }

        public void Redo()
        {
            if (match.CurrentIndex < match.Moves.Count)
            {
                match.Redo(gameState);
                SwitchActivePlayer();
                UpdateGameState();
                MoveCompletedEvent?.Invoke(gameState.Pieces);
            }
        }

        public void Undo()
        {
            if (match.CurrentIndex > 0)
            {
                match.Undo(gameState);
                SwitchActivePlayer();
                UpdateGameState();
                MoveCompletedEvent?.Invoke(gameState.Pieces);
            }
        }

        public void JumpToMove(Move move)
        {
            Assert.IsTrue(match.Moves.Contains(move), $"The move {move} is not contained in the current game state.");
            int jumpToIndex = match.Moves.IndexOf(move) + 1;
            Debug.Log($"Jump to move {move} with index {jumpToIndex}.");

            if (jumpToIndex < match.CurrentIndex)
            {
                for (int i = match.CurrentIndex; i > jumpToIndex; i--)
                {
                    Undo();
                }
            }
            else if (jumpToIndex > match.CurrentIndex)
            {
                for (int i = match.CurrentIndex; i < jumpToIndex; i++)
                {
                    Redo();
                }
            }
        }

        public void ExecuteMove(Move move)
        {
            if (match.IsPastGamestate())
            {
                return;
            }

            match.Add(move);
            Redo();
        }

        private void SwitchActivePlayer()
        {
            gameState.SwitchActivePlayer();
        }

        public void DeserializeMatch()
        {
            match = MatchSerializer.DeserializeMatch(Application.persistentDataPath + "/match.pgn");
        }

        private void UpdateGameState()
        {
            CheckIfPawnMovedTwoSquares();

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
                    match.Moves[match.CurrentIndex - 1].isCheck = true;
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

        private void UpdateCheckMate(Color color)
        {
            // Check if the player still has valid moves.
            List<Figure> ownPieces = color == Color.White ? gameState.WhitePieces : gameState.BlackPieces;
            foreach (Figure piece in ownPieces)
            {
                if (piece.MoveablePositions.Count > 0)
                {
                    return;
                }
            }

            // If the player has no valid moves and his king is checked, he is checkmated.
            King king = color == Color.White ? gameState.WhiteKing : gameState.BlackKing;
            List<Figure> enemyPieces = color == Color.White ? gameState.BlackPieces : gameState.WhitePieces;
            foreach (var piece in enemyPieces)
            {
                if (piece.AttacksPosition(king.Position))
                {
                    GameEndedEvent?.Invoke(color == Color.White ? GameResult.BlackWins : GameResult.WhiteWins);
                    match.Moves[match.CurrentIndex - 1].isCheckmate = true;
                    return;
                }
            }

            // If the player has no valid moves but his king is not checked the game is a draw.
            GameEndedEvent?.Invoke(GameResult.Draw);
        }

        private void CheckIfPawnMovedTwoSquares()
        {
            if (match.CurrentIndex == 0)
            {
                return;
            }

            Move lastMove = match.Moves[match.CurrentIndex - 1];
            if (lastMove.figureData.type == typeof(Pawn) && lastMove.from.ChebyshevDistance(lastMove.to) == 2)
            {
                foreach (Figure pawn in gameState.Pieces.Where((p) => p is Pawn))
                {
                    (pawn as Pawn).UpdatedHasMovedByTwo(lastMove.to);
                }
            }
        }
    }
}