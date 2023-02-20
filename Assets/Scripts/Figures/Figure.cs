using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Chess
{
    /// <summary>
    /// Abstract base class for all figures. Each chess piece defines it's behaviour by calculating a list of all possiblePositions it can move to.
    /// </summary>
    public abstract class Figure : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private GameObject whiteSprite = default;
        [SerializeField]
        private GameObject blackSprite = default;

        private Chess.Color color = default;
        private GameManager gameManager;
        protected bool hasMoved = false;

        protected Figure pinnedBy = null;

        public Chess.Color Color => color;
        public Vector2Int Position => position;
        protected GameManager GameManager => gameManager;
        protected Board Board => GameManager.Board;
        public bool HasMoved => hasMoved;

        protected Vector2Int position = new Vector2Int();
        protected readonly List<Vector2Int> moveablePositions = new List<Vector2Int>();
        protected readonly List<Vector2Int> attackedPositions = new List<Vector2Int>();

        public delegate void OnFigureMovedHandler(Figure figure);
        public event OnFigureMovedHandler OnFigureMovedEvent;

        public delegate void OnFigureSelectedHandler(Figure figure);
        public event OnFigureSelectedHandler OnFigureSelectedEvent;

        public delegate void OnFigureDestroyedHandler(Figure figure);
        public event OnFigureDestroyedHandler OnFigureDestroyedEvent;

        public List<Vector2Int> MoveablePositions { get { return moveablePositions; } }
        public List<Vector2Int> AttackedPositions { get { return attackedPositions; } }


        private void OnDestroy()
        {
            OnFigureDestroyedEvent.Invoke(this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnFigureSelectedEvent?.Invoke(this);
        }

        public void SetFigure(Vector2Int position, Color color, GameManager gameManager)
        {
            this.position = position;
            this.color = color;
            this.gameManager = gameManager;
            transform.position = Board.GetWorldPosition(position);

            whiteSprite.SetActive(color == Color.White);
            blackSprite.SetActive(color == Color.Black);
        }

        public bool CanMove(Vector2Int position)
        {
            if (moveablePositions.Contains(position))
            {
                return true;
            }
            return false;
        }

        public void Move(Vector2Int newPosition)
        {
            Assert.IsTrue(CanMove(newPosition));
            Board.RemoveFigureFromSquare(position);

            if (!Board.SquareIsEmpty(newPosition))
            {
                Assert.IsTrue(Board.GetFigure(newPosition).Color != Color);
                Destroy(Board.GetFigure(newPosition).gameObject);
            }

            Board.SetFigureToSquare(this, newPosition);
            OnMove(this.position, newPosition);

            this.position = newPosition;
            transform.position = Board.GetWorldPosition(position);
            hasMoved = true;
            OnFigureMovedEvent?.Invoke(this);
        }

        protected virtual void OnMove(Vector2Int oldPosition, Vector2Int newPosition)
        {

        }

        public void ClearState()
        {
            moveablePositions.Clear();
            attackedPositions.Clear();
            pinnedBy = null;
        }

        public abstract void UpdatePositions();

        public virtual void UpdatePinned()
        {
            if (pinnedBy == null)
            {
                return;
            }

            Vector2Int piecePosition = new Vector2Int(position.x, position.y);
            Vector2Int pinnedPosition = new Vector2Int(pinnedBy.position.x, pinnedBy.position.y);

            moveablePositions.RemoveAll(pos =>
            {
                Vector2Int moveablePosition = new Vector2Int(pos.x, pos.y);
                if (Vector2.Angle(piecePosition - pinnedPosition, piecePosition - moveablePosition) > Vector2.kEpsilon)
                {
                    return true;
                }
                else return false;
            });

            attackedPositions.RemoveAll(pos =>
            {
                Vector2Int moveablePosition = new Vector2Int(pos.x, pos.y);
                if (Vector2.Angle(piecePosition - pinnedPosition, piecePosition - moveablePosition) > Vector2.kEpsilon)
                {
                    return true;
                }
                else return false;
            });

            pinnedBy = null;
        }

        // Checks the square at position is empty or has an enemy piece.
        protected bool UpdateField(Vector2Int newPosition)
        {
            if (newPosition.IsValid())
            {
                attackedPositions.Add(newPosition);
                if (Board.SquareIsEmpty(newPosition))
                {
                    moveablePositions.Add(newPosition);
                    return true;
                }

                if (Board.SquareHasEnemyPiece(Color, newPosition))
                {
                    moveablePositions.Add(newPosition);
                }
            }
            return false;
        }

        // Checks for each square in a line if the figure can move there, stopping the first time a piece blocks the line. 
        // TODO: Implement pinning behaviour
        protected void UpdateLine(int horizontal, int vertical)
        {
            for (int i = 1; i < Mathf.Max(Board.Width, Board.Height); i++)
            {
                Vector2Int newPosition = new Vector2Int(position.x + horizontal * i, position.y + vertical * i);
                if (!newPosition.IsValid())
                {
                    break;
                }

                if (!UpdateField(newPosition))
                {
                    // If the figure on the square is the enemy king, also mark the position behind the king as attacked
                    Figure figure = Board.GetFigure(newPosition);
                    if (figure != null && figure.Color != Color)
                    {
                        if (figure is King)
                        {
                            Vector2Int nextPosition = new Vector2Int(newPosition.x + horizontal, newPosition.y + vertical);
                            if (nextPosition.IsValid())
                            {
                                attackedPositions.Add(nextPosition);
                            }
                        }
                        else
                        {
                            // Continue walking down the line to check if piece is pinned because its king is behind.
                            // TODO: implement efficient check if king is on this line.
                            King enemyKing = gameManager.GetKingOfColor(color == Color.White ? Color.Black : Color.White);
                            for (int j = 1; j < Mathf.Max(Board.Width, Board.Height); j++)
                            {
                                Vector2Int nextPosition = new Vector2Int(newPosition.x + horizontal * j, newPosition.y + vertical * j);
                                if (!nextPosition.IsValid())
                                {
                                    break;
                                }

                                if (nextPosition == enemyKing.Position)
                                {
                                    figure.pinnedBy = this;
                                }

                                if (!Board.SquareIsEmpty(nextPosition))
                                {
                                    break;
                                }
                            }

                        }
                    }
                    break;
                }
            }
        }
    }
}