using System;
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
            if (!IsPositionValid(position)) throw new ArgumentOutOfRangeException("position");
            Assert.IsNotNull(squares[position.x, position.y], "No figure exists on the board at the given position. Check if the position is not empty first.");

            return squares[position.x, position.y];
        }

        public void SetFigureToSquare(Figure figure, Vector2Int position)
        {
            if (!IsPositionValid(position)) throw new ArgumentOutOfRangeException("position");
            if (figure == null) throw new ArgumentNullException("figure");
            Assert.IsTrue(SquareIsEmpty(position), "A figure already exists on the board at the given position. Check if the position is empty first.");

            squares[position.x, position.y] = figure;
        }

        public void RemoveFigureFromSquare(Vector2Int position)
        {
            if (!IsPositionValid(position)) throw new ArgumentOutOfRangeException("position");
            squares[position.x, position.y] = null;
        }

        public bool SquareIsEmpty(Vector2Int position)
        {
            if (!IsPositionValid(position)) throw new ArgumentOutOfRangeException("position");
            return squares[position.x, position.y] == null;
        }

        public bool SquareHasEnemyPiece(Color ownColor, Vector2Int position)
        {
            if (!IsPositionValid(position)) throw new ArgumentOutOfRangeException("position");
            if (SquareIsEmpty(position))
            {
                return false;
            }
            return GetFigure(position).Color != ownColor;
        }

        public bool IsPositionValid(Vector2Int pos)
        {
            return (pos.x >= 0 && pos.x < Width) && (pos.y >= 0 && pos.y < Height);
        }
    }
}