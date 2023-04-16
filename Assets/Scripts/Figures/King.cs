using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Chess
{
    public class King : Figure
    {
        public King(Vector2Int position, Color color, GameState gameState) : base(position, color, gameState) { }

        public override void UpdatePositions()
        {
            AddPossibleSquares();
            CheckCastle();
            RemoveAttackedSquares();
        }

        public override void UpdatePinned()
        {
            Assert.IsNull(pinnedBy, $"Pinning the king should not be possible. PinnedBy: {pinnedBy}");
            return;
        }

        private void AddPossibleSquares()
        {
            List<Vector2Int> moveablePositions = new List<Vector2Int>
            {
                new Vector2Int(position.x + 1, position.y),
                new Vector2Int(position.x - 1, position.y),
                new Vector2Int(position.x, position.y + 1),
                new Vector2Int(position.x, position.y - 1),
                new Vector2Int(position.x + 1, position.y + 1),
                new Vector2Int(position.x + 1, position.y - 1),
                new Vector2Int(position.x - 1, position.y + 1),
                new Vector2Int(position.x - 1, position.y - 1)
            };

            foreach (var position in moveablePositions)
            {
                UpdateField(position);
            }
        }

        // BUG: King may not castle if he is checked. Some kind of bug here?
        private void CheckCastle()
        {
            if (!hasMoved)
            {
                Vector2Int rookPosition = Color == Color.White ? new Vector2Int(0, 0) : new Vector2Int(0, 7);
                if (!Board.SquareIsEmpty(rookPosition))
                {
                    Figure figure = Board.GetFigure(rookPosition);
                    if (figure != null && figure.Color == Color && !figure.HasMoved && figure is Rook)
                    {
                        if (Board.SquareIsEmpty(new Vector2Int(position.x - 1, position.y)) &&
                            Board.SquareIsEmpty(new Vector2Int(position.x - 2, position.y)) &&
                            !GameState.IsSquareAttacked(Color, position) &&
                            !GameState.IsSquareAttacked(Color, new Vector2Int(position.x - 1, position.y)) &&
                            !GameState.IsSquareAttacked(Color, new Vector2Int(position.x - 2, position.y))
                        )
                        {
                            moveablePositions.Add(new Vector2Int(position.x - 2, position.y));
                        }
                    }
                }

                rookPosition = Color == Color.White ? new Vector2Int(7, 0) : new Vector2Int(7, 7);
                if (!Board.SquareIsEmpty(rookPosition))
                {
                    Figure figure = Board.GetFigure(rookPosition);
                    if (figure != null && figure.Color == Color && !figure.HasMoved && figure is Rook)
                    {
                        if (Board.SquareIsEmpty(new Vector2Int(position.x + 1, position.y)) &&
                            Board.SquareIsEmpty(new Vector2Int(position.x + 2, position.y)) &&
                            !GameState.IsSquareAttacked(Color, position) &&
                            !GameState.IsSquareAttacked(Color, new Vector2Int(position.x + 1, position.y)) &&
                            !GameState.IsSquareAttacked(Color, new Vector2Int(position.x + 2, position.y))
                        )
                        {

                            moveablePositions.Add(new Vector2Int(position.x + 2, position.y));
                        }
                    }
                }
            }
        }

        private void RemoveAttackedSquares()
        {
            List<Vector2Int> attackedPositions = new List<Vector2Int>();
            foreach (Vector2Int position in moveablePositions)
            {
                if (GameState.IsSquareAttacked(Color, position))
                {
                    attackedPositions.Add(position);
                }
            }
            moveablePositions.RemoveAll((i) => attackedPositions.Contains(i));
        }

        // Check if move is castle to move the corresponding rook
        protected override void OnMove(Vector2Int oldPosition, Vector2Int newPosition)
        {
            base.OnMove(oldPosition, newPosition);
            float direction = oldPosition.x - newPosition.x;
            if (direction > 1)
            {
                Figure rook = Board.GetFigure(new Vector2Int(0, position.y));
                rook.Move(new Vector2Int(3, position.y));
            }
            else if (direction < -1)
            {
                Figure rook = Board.GetFigure(new Vector2Int(7, position.y));
                rook.Move(new Vector2Int(5, position.y));
            }
        }
    }
}