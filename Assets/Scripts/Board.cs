using UnityEngine;
using UnityEngine.Assertions;

namespace Chess
{
    /// <summary>
    /// Represents the chess board and the current game, by giving access to a list of squares and their figures. 
    /// </summary>
    public class Board : MonoBehaviour
    {
        [SerializeField]
        private Square squarePrefab = default;
        [SerializeField]
        private InputManager inputManager = default;

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        private readonly int height = 8;
        private readonly int width = 8;
        private readonly Square[,] squares = new Square[8, 8];

        public void InitializeBoard()
        {
            for (int file = 0; file < width; file++)
            {
                for (int rank = 0; rank < height; rank++)
                {
                    Square square = Instantiate(squarePrefab, transform);

                    Vector2Int pos = new Vector2Int(file, rank);
                    Color color = (Color)((file + rank + 1) % 2);
                    square.SetSquare(pos, color);
                    inputManager.RegisterSquare(square);

                    square.transform.position = GetWorldPosition(pos);
                    squares[file, rank] = square;
                }
            }
        }

        private Square GetSquare(Vector2Int position)
        {
            Assert.IsTrue(position.IsValid());
            return squares[position.x, position.y];
        }

        public Figure GetFigure(Vector2Int position)
        {
            return GetSquare(position).Figure;
        }


        public void SetFigureToSquare(Figure figure, Vector2Int squarePosition)
        {
            GetSquare(squarePosition).Figure = figure;
        }

        public void RemoveFigureFromSquare(Vector2Int squarePosition)
        {
            GetSquare(squarePosition).Figure = null;
        }

        public bool SquareIsEmpty(Vector2Int position)
        {
            Assert.IsTrue(position.IsValid());
            return GetSquare(position).IsEmpty;
        }

        public bool SquareHasEnemyPiece(Color ownColor, Vector2Int position)
        {
            Assert.IsTrue(position.IsValid());
            if (SquareIsEmpty(position))
            {
                return false;
            }
            return GetSquare(position).Figure.Color != ownColor;
        }

        public Vector3 GetWorldPosition(Vector2Int position)
        {
            Assert.IsTrue(position.IsValid());
            return new Vector3(position.x, position.y, transform.position.z);
        }
    }
}