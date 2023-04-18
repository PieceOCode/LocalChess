using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess
{
    /// <summary>
    /// Abstract base class for all figures. Each chess piece defines it's behavior by calculating a list of all possiblePositions it can move to.
    /// </summary>
    public abstract class Figure
    {
        public delegate void OnFigureMovedHandler(Figure figure);
        public event OnFigureMovedHandler OnFigureMovedEvent;

        public delegate void OnFigureDestroyedHandler(Figure figure);
        public event OnFigureDestroyedHandler OnFigureDestroyedEvent;

        public Chess.Color Color => color;
        public Vector2Int Position => position;
        public bool HasMoved { get => hasMoved; set { hasMoved = value; } }
        public List<Vector2Int> MoveablePositions { get { return moveablePositions; } }

        protected GameState GameState => gameState;
        protected Board Board => GameState.Board;


        private readonly GameState gameState;
        private readonly Chess.Color color = default;
        protected Vector2Int position = new();
        protected bool hasMoved = false;
        protected Figure pinnedBy = null;

        protected readonly List<Vector2Int> moveablePositions = new List<Vector2Int>();
        protected readonly List<Vector2Int> attackedPositions = new List<Vector2Int>();

        public Figure(Vector2Int position, Color color, GameState gameState)
        {
            this.position = position;
            this.color = color;
            this.gameState = gameState;
        }

        public bool CanMoveTo(Vector2Int position)
        {
            return moveablePositions.Contains(position);
        }

        public bool AttacksPosition(Vector2Int position)
        {
            return attackedPositions.Contains(position);
        }

        public void Move(Vector2Int newPosition)
        {
            OnMove(this.position, newPosition);
            this.position = newPosition;
            hasMoved = true;
            OnFigureMovedEvent?.Invoke(this);
        }

        public void ClearState()
        {
            moveablePositions.Clear();
            attackedPositions.Clear();
            pinnedBy = null;
        }

        public void ClearMoveablePositions()
        {
            moveablePositions.Clear();
        }

        public void ClearMoveablePositionsExcept(List<Vector2Int> validPositions)
        {
            moveablePositions.RemoveAll(x => !validPositions.Contains(x));
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

            // BUG: attacked positions probably stay the same if a piece is pinned. (king cannot move onto a tile that is attacked by a pinned figure (https://chess.stackexchange.com/questions/4565/can-the-king-be-attacked-by-a-pinned-piece)
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

        protected virtual void OnMove(Vector2Int oldPosition, Vector2Int newPosition) { }

        // Adds a square as movable and attacked position if it is not blocked by a piece of the same color
        protected void UpdateField(Vector2Int position)
        {
            if (Board.IsPositionValid(position))
            {
                attackedPositions.Add(position);
                if (Board.SquareIsEmpty(position) || Board.SquareHasEnemyPiece(Color, position))
                {
                    moveablePositions.Add(position);
                }
            }
        }

        // Checks for each square in a line if the figure can move there, stopping the first time a piece blocks the line.
        protected void UpdateLine(int horizontal, int vertical)
        {
            Vector2Int newPosition = position + new Vector2Int(horizontal, vertical);

            // Walks down the line until the end of the board or a blocked square is reached. 
            while (Board.IsPositionValid(newPosition) && Board.SquareIsEmpty(newPosition))
            {
                UpdateField(newPosition);
                newPosition += new Vector2Int(horizontal, vertical);
            }

            // If the current position was blocked by an enemy piece check if the piece was pinned. If the blocking piece was the enemy king the square behind the king is also attacked. 
            if (Board.IsPositionValid(newPosition))
            {
                UpdateField(newPosition);
                Figure figure = Board.GetFigure(newPosition);
                King enemyKing = color == Color.White ? gameState.BlackKing : gameState.WhiteKing;
                if (figure == enemyKing)
                {
                    Vector2Int positionBehindKing = new Vector2Int(newPosition.x + horizontal, newPosition.y + vertical);
                    if (Board.IsPositionValid(positionBehindKing))
                    {
                        attackedPositions.Add(positionBehindKing);
                    }
                }
                else if (figure.Color != this.color && figure.position.IsBetween(this.position, enemyKing.position))
                {
                    List<Vector2Int> positionsBetween = figure.position.GetPositionsBetween(enemyKing.position);
                    if (positionsBetween.Where(pos => !Board.SquareIsEmpty(pos)).Count() == 0)
                    {
                        figure.pinnedBy = this;
                    }
                }
            }
        }
    }
}