using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Chess
{
    /// <summary>
    /// Holds all relevant information about the current state of a game.
    /// </summary>
    // OPTIMIZE: Cache LINQ queries
    public class GameState
    {
        public List<Figure> WhitePieces => whitePieces;
        public List<Figure> BlackPieces => blackPieces;
        public List<Figure> Pieces => whitePieces.Concat(blackPieces).ToList();
        public King WhiteKing => whitePieces.Where(piece => piece is King).FirstOrDefault() as King;
        public King BlackKing => blackPieces.Where(piece => piece is King).FirstOrDefault() as King;
        public Color ActivePlayer => activePlayer;
        public Board Board => board;


        private Color activePlayer = Color.White;
        private readonly Board board;
        private readonly List<Figure> whitePieces = new List<Figure>();
        private readonly List<Figure> blackPieces = new List<Figure>();

        public GameState()
        {
            activePlayer = Color.White;
            board = new Board(8, 8);
        }

        public void SwitchActivePlayer()
        {
            activePlayer = activePlayer == Color.White ? Color.Black : Color.White;
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

        public void AddFigure(Figure figure)
        {
            Assert.IsNotNull(figure, "The figure argument should not be null.");
            Assert.IsTrue(!Pieces.Contains(figure), "This given figure already exists in this GameState");

            board.SetFigureToSquare(figure, figure.Position);
            if (figure.Color == Color.White)
            {
                whitePieces.Add(figure);
            }
            else
            {
                blackPieces.Add(figure);
            }
        }

        public void RemoveFigure(Figure figure)
        {
            Assert.IsNotNull(figure, "You cannot remove a figure that is null.");
            Assert.IsTrue(Pieces.Contains(figure), "The given figure does not exist in this GameState.");
            Assert.IsTrue(board.GetFigure(figure.Position) == figure, "A different figure exists on the given figures position on this board.");

            board.RemoveFigureFromSquare(figure.Position);
            if (figure.Color == Color.White)
            {
                whitePieces.Remove(figure);
            }
            else
            {
                blackPieces.Remove(figure);
            }
        }

        // Should this also kick a figure and return the kicked figure?
        // TODO: Implement tests for this
        public void MoveFigure(Figure figure, Vector2Int to)
        {
            board.RemoveFigureFromSquare(figure.Position);
            board.SetFigureToSquare(figure, to);
            figure.Move(to);
        }

        public void SpawnFigures()
        {
            AddFigure(new Rook(new Vector2Int(0, 0), Color.White, this));
            AddFigure(new Rook(new Vector2Int(7, 0), Color.White, this));
            AddFigure(new Rook(new Vector2Int(0, 7), Color.Black, this));
            AddFigure(new Rook(new Vector2Int(7, 7), Color.Black, this));

            AddFigure(new Knight(new Vector2Int(1, 0), Color.White, this));
            AddFigure(new Knight(new Vector2Int(6, 0), Color.White, this));
            AddFigure(new Knight(new Vector2Int(1, 7), Color.Black, this));
            AddFigure(new Knight(new Vector2Int(6, 7), Color.Black, this));

            AddFigure(new Bishop(new Vector2Int(2, 0), Color.White, this));
            AddFigure(new Bishop(new Vector2Int(5, 0), Color.White, this));
            AddFigure(new Bishop(new Vector2Int(2, 7), Color.Black, this));
            AddFigure(new Bishop(new Vector2Int(5, 7), Color.Black, this));

            AddFigure(new Queen(new Vector2Int(3, 0), Color.White, this));
            AddFigure(new Queen(new Vector2Int(3, 7), Color.Black, this));

            AddFigure(new King(new Vector2Int(4, 0), Color.White, this));
            AddFigure(new King(new Vector2Int(4, 7), Color.Black, this));

            for (int i = 0; i < Board.Width; i++)
            {
                AddFigure(new Pawn(new Vector2Int(i, 1), Color.White, this));
                AddFigure(new Pawn(new Vector2Int(i, 6), Color.Black, this));
            }
        }
    }
}
