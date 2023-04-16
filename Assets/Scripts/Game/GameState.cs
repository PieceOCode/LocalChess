using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess
{
    /// <summary>
    /// Holds all relevant information about the current state of a game.
    /// </summary>
    // TODO: (Optimization) Cache LINQ queries
    public class GameState
    {
        public List<Figure> WhitePieces => whitePieces;
        public List<Figure> BlackPieces => blackPieces;
        public List<Figure> Pieces => whitePieces.Concat(blackPieces).ToList();
        public King WhiteKing => whitePieces.Where(piece => piece is King).First() as King;
        public King BlackKing => blackPieces.Where(piece => piece is King).First() as King;
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
    }
}
