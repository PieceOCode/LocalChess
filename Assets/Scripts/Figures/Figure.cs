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
        private Chess.Color color = default;
        private GameManager gameManager;
        private SpriteRenderer sprite = default;
        protected bool hasMoved = false;

        protected Figure pinnedBy = null;

        public Chess.Color Color => color;
        public Position Position => position;
        protected GameManager GameManager => gameManager;
        protected Board Board => GameManager.Board;
        public bool HasMoved => hasMoved;

        protected Position position = new Position();
        protected readonly List<Position> moveablePositions = new List<Position>();
        protected readonly List<Position> attackedPositions = new List<Position>();

        public delegate void OnFigureSelectedHandler(Figure figure);
        public event OnFigureSelectedHandler OnFigureSelectedEvent;

        public delegate void OnFigureDestroyedHandler(Figure figure);
        public event OnFigureDestroyedHandler OnFigureDestroyedEvent;

        private SpriteRenderer Sprite
        {
            get
            {
                if (sprite == null)
                {
                    sprite = GetComponent<SpriteRenderer>();
                }
                return sprite;
            }
        }

        public List<Position> MoveablePositions { get { return moveablePositions; } }
        public List<Position> AttackedPositions { get { return attackedPositions; } }

        private void Start()
        {
            if (Color == Color.Black)
            {
                Sprite.color = UnityEngine.Color.green;
            }
            if (Color == Color.White)
            {
                Sprite.color = UnityEngine.Color.red;
            }
        }

        private void OnDestroy()
        {
            OnFigureDestroyedEvent.Invoke(this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnFigureSelectedEvent?.Invoke(this);
        }

        public void SetFigure(Position position, Color color, GameManager gameManager)
        {
            this.position = position;
            this.color = color;
            this.gameManager = gameManager;
            transform.position = Board.GetWorldPosition(position);
        }

        public bool CanMove(Position position)
        {
            if (moveablePositions.Contains(position))
            {
                return true;
            }
            return false;
        }

        public void Move(Position newPosition)
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
        }

        protected virtual void OnMove(Position oldPosition, Position newPosition)
        {

        }

        public virtual void UpdatePositions()
        {
            moveablePositions.Clear();
            attackedPositions.Clear();
        }

        public virtual void UpdatePinned()
        {
            if (pinnedBy == null)
            {
                return;
            }

            Vector2 piecePosition = new Vector2(position.File, position.Rank);
            Vector2 pinnedPosition = new Vector2(pinnedBy.position.File, pinnedBy.position.Rank);

            moveablePositions.RemoveAll(pos =>
            {
                Vector2 moveablePosition = new Vector2(pos.File, pos.Rank);
                if (Vector2.Angle(piecePosition - pinnedPosition, piecePosition - moveablePosition) > Vector2.kEpsilon)
                {
                    return true;
                }
                else return false;
            });

            attackedPositions.RemoveAll(pos =>
            {
                Vector2 moveablePosition = new Vector2(pos.File, pos.Rank);
                if (Vector2.Angle(piecePosition - pinnedPosition, piecePosition - moveablePosition) > Vector2.kEpsilon)
                {
                    return true;
                }
                else return false;
            });

            pinnedBy = null;
        }

        // Checks the square at position is empty or has an enemy piece.
        protected bool UpdateField(Position newPosition)
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
                Position newPosition = new Position(position.File + horizontal * i, position.Rank + vertical * i);
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
                            Position nextPosition = new Position(newPosition.File + horizontal, newPosition.Rank + vertical);
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
                                Position nextPosition = new Position(newPosition.File + horizontal * j, newPosition.Rank + vertical * j);
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