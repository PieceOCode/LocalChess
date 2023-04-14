using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Chess
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField]
        private SpawnManager spawnManager = default;

        public Board Board { get { return board; } }
        public Match Match { get { return match; } }

        private Match match;
        private Board board;
        private List<Figure> whitePieces = new List<Figure>();
        private List<Figure> blackPieces = new List<Figure>();
        private List<Figure> pieces => whitePieces.Concat(blackPieces).ToList();

        private void Awake()
        {
            board = new Board(8, 8);
            match = new Match();
            spawnManager.ResetBoardStandard();
            UpdateGameState();
        }

        // TODO: Implement proper debug menu
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                UpdateGameState();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                match.Undo(board);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                match.Redo(board);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                match.Serialize();
            }
        }

        public void UpdateGameState()
        {
            pieces.ForEach(piece => piece.ClearState());
            pieces.ForEach(piece => piece.UpdatePositions());

            // Kings have to be updated last, because they cannot move to attacked tiles. 
            GetKingOfColor(Color.White).UpdatePositions();
            GetKingOfColor(Color.Black).UpdatePositions();

            pieces.ForEach(piece => piece.UpdatePinned());

            // If the king is attacked no piece can move if it does not resolve the check.
            UpdateCheck(Color.White);
            UpdateCheck(Color.Black);

            UpdateCheckMate(Color.White);
            UpdateCheckMate(Color.Black);
        }

        // The king is checkmated if he is in check and there are no valid moves to stop that. 
        // The game is a draw if there are no valid moves but the king is not in check. 
        private void UpdateCheckMate(Color color)
        {
            List<Figure> ownPieces = color == Color.White ? whitePieces : blackPieces;
            foreach (Figure piece in ownPieces)
            {
                if (piece.MoveablePositions.Count > 0)
                {
                    return;
                }
            }

            King king = GetKingOfColor(color);
            List<Figure> enemyPieces = color == Color.White ? blackPieces : whitePieces;
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

        private void UpdateCheck(Color kingColor)
        {
            King king = GetKingOfColor(kingColor);
            List<Figure> enemyPieces = kingColor == Color.White ? blackPieces : whitePieces;
            List<Figure> ownPieces = kingColor == Color.White ? whitePieces : blackPieces;

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

        public void RegisterFigure(Figure figure)
        {
            if (figure.Color == Color.White)
            {
                whitePieces.Add(figure);
            }
            else
            {
                blackPieces.Add(figure);
            }

            figure.OnFigureMovedEvent += OnFigureMoved;
            figure.OnFigureDestroyedEvent += OnFigureDestroyed;
        }

        private void OnFigureMoved(Figure figure)
        {
            UpdateGameState();
        }

        public void OnFigureDestroyed(Figure figure)
        {
            if (figure.Color == Color.White)
            {
                whitePieces.Remove(figure);
            }
            else
            {
                blackPieces.Remove(figure);
            }

            figure.OnFigureMovedEvent -= OnFigureMoved;
            figure.OnFigureDestroyedEvent -= OnFigureDestroyed;
        }

        public bool IsSquareAttacked(Color ownColor, Vector2Int position)
        {
            List<Figure> opponentPieces = ownColor == Color.White ? blackPieces : whitePieces;
            foreach (var piece in opponentPieces)
            {
                if (piece.AttacksPosition(position))
                {
                    return true;
                }
            }
            return false;
        }

        public King GetKingOfColor(Color color)
        {
            List<Figure> pieces = color == Color.White ? whitePieces : blackPieces;
            King king = pieces.Where(piece => piece is King).First() as King;
            Assert.IsNotNull(king, "A king of each color should exist at all time.");
            return king;
        }

        public King GetEnemyKing(Color color)
        {
            return GetKingOfColor(color == Color.White ? Color.Black : Color.White);
        }
    }
}