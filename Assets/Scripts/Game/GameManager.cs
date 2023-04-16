using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    /// <summary>
    /// Controls game logic, such updating the figures and the pinning and checking for check/checkmate.
    /// </summary>
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField]
        private FigureSpawner spawnManager = default;

        public Board Board { get { return gameState.Board; } }
        public Color ActivePlayer => gameState.ActivePlayer;

        private Match match;
        private GameState gameState;


        private void Awake()
        {
            gameState = new GameState();
            match = new Match();
            SpawnFigures();
            UpdateGameState();
            UpdateRepresentation();
        }

        // TODO: Implement proper debug menu
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                UpdateGameState();
            }

            // TODO: Only switch active player if undo was successful
            if (Input.GetKeyDown(KeyCode.Q))
            {
                match.Undo(gameState);
                SwitchActivePlayer();
                UpdateGameState();
                UpdateRepresentation();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                match.Redo(gameState);
                SwitchActivePlayer();
                UpdateGameState();
                UpdateRepresentation();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                match.Serialize();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                spawnManager.ClearRepresentations();
            }
        }

        public void ExecuteMove(Move move)
        {
            match.Add(move);
            match.Redo(gameState);

            SwitchActivePlayer();
            UpdateGameState();
            UpdateRepresentation();
        }

        private void SwitchActivePlayer()
        {
            gameState.SwitchActivePlayer();
        }

        private void SpawnFigures()
        {
            new Rook(new Vector2Int(0, 0), Color.White, gameState);
            new Rook(new Vector2Int(7, 0), Color.White, gameState);
            new Rook(new Vector2Int(0, 7), Color.Black, gameState);
            new Rook(new Vector2Int(7, 7), Color.Black, gameState);

            new Knight(new Vector2Int(1, 0), Color.White, gameState);
            new Knight(new Vector2Int(6, 0), Color.White, gameState);
            new Knight(new Vector2Int(1, 7), Color.Black, gameState);
            new Knight(new Vector2Int(6, 7), Color.Black, gameState);

            new Bishop(new Vector2Int(2, 0), Color.White, gameState);
            new Bishop(new Vector2Int(5, 0), Color.White, gameState);
            new Bishop(new Vector2Int(2, 7), Color.Black, gameState);
            new Bishop(new Vector2Int(5, 7), Color.Black, gameState);

            new Queen(new Vector2Int(3, 0), Color.White, gameState);
            new Queen(new Vector2Int(3, 7), Color.Black, gameState);

            new King(new Vector2Int(4, 0), Color.White, gameState);
            new King(new Vector2Int(4, 7), Color.Black, gameState);

            for (int i = 0; i < Board.Width; i++)
            {
                new Pawn(new Vector2Int(i, 1), Color.White, gameState);
                new Pawn(new Vector2Int(i, 6), Color.Black, gameState);
            }
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

        private void UpdateRepresentation()
        {
            spawnManager.ClearRepresentations();
            foreach (Figure figure in gameState.Pieces)
            {
                spawnManager.CreateRepresentation(figure);
            }
        }
    }
}