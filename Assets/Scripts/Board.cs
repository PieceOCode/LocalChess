using UnityEngine;
using UnityEngine.Assertions;

namespace Chess
{
    /// <summary>
    /// Represents the state of the chess board and its figures. 
    /// </summary>
    public sealed class Board
    {
        public int Width { get { return width; } }
        public int Height { get { return height; } }

        private readonly int height;
        private readonly int width;
        private readonly Figure[,] squares;

        public Board(int height, int width)
        {
            this.height = height;
            this.width = width;
            squares = new Figure[width, height];
        }

        public Figure GetFigure(Vector2Int position)
        {
            Assert.IsTrue(position.IsValid());
            return squares[position.x, position.y];
        }

        public void SetFigureToSquare(Figure figure, Vector2Int position)
        {
            Assert.IsTrue(position.IsValid());
            squares[position.x, position.y] = figure;
        }

        public void RemoveFigureFromSquare(Vector2Int position)
        {
            Assert.IsTrue(position.IsValid());
            squares[position.x, position.y] = null;
        }

        public bool SquareIsEmpty(Vector2Int position)
        {
            Assert.IsTrue(position.IsValid());
            return squares[position.x, position.y] == null;
        }

        public bool SquareHasEnemyPiece(Color ownColor, Vector2Int position)
        {
            Assert.IsTrue(position.IsValid());
            if (SquareIsEmpty(position))
            {
                return false;
            }
            return GetFigure(position).Color != ownColor;
        }
    }
}