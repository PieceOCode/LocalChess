using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Board board = default;
        public Board Board { get { return board; } }

        private List<Figure> whitePieces = new List<Figure>();
        private List<Figure> blackPieces = new List<Figure>();
        private List<Figure> pieces => whitePieces.Concat(blackPieces).ToList();

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

            figure.OnFigureDestroyedEvent += OnFigureDestroyed;
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
        }

        public bool IsSquareAttacked(Color ownColor, Position position)
        {
            List<Figure> opponentPieces = ownColor == Color.White ? blackPieces : whitePieces;
            foreach (var piece in opponentPieces)
            {
                if (piece.AttackedPositions.Contains(position))
                {
                    return true;
                }
            }
            return false;
        }

    }
}