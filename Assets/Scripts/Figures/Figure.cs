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

        private void Update()
        {
            UpdatePositions();
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
                Destroy(Board.GetFigure(newPosition));
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

        protected abstract void UpdatePositions();


        protected bool UpdateField(Position newPosition)
        {
            if (newPosition.IsValid())
            {
                if (Board.SquareIsEmpty(newPosition))
                {
                    moveablePositions.Add(newPosition);
                    return true;
                }

                if (Board.SquareHasEnemyPiece(Color, newPosition))
                {
                    moveablePositions.Add(newPosition);
                }

                attackedPositions.Add(newPosition);
            }
            return false;
        }

        protected void UpdateLine(int horizontal, int vertical)
        {
            for (int i = 1; i < Mathf.Max(Board.Width, Board.Height); i++)
            {
                Position newPosition = new Position(position.File + horizontal * i, position.Rank + vertical * i);
                if (!UpdateField(newPosition))
                {
                    break;
                }
            }
        }
    }
}