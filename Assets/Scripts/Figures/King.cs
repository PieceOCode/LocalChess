using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Chess
{
    public class King : Figure
    {
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

        // TODO: Refactor, this seems overly long and complicated. Is there a better way to quickly check a number of tiles?
        private void CheckCastle()
        {
            if (!hasMoved)
            {
                Vector2Int rookPosition = Color == Color.White ? new Vector2Int(0, 0) : new Vector2Int(0, 7);
                Figure figure = Board.GetFigure(rookPosition);
                if (figure != null && figure.Color == Color && !figure.HasMoved && figure is Rook)
                {
                    if (Board.GetFigure(new Vector2Int(position.x - 1, position.y)) == null &&
                        Board.GetFigure(new Vector2Int(position.x - 2, position.y)) == null &&
                        !GameManager.IsSquareAttacked(Color, position) &&
                        !GameManager.IsSquareAttacked(Color, new Vector2Int(position.x - 1, position.y)) &&
                        !GameManager.IsSquareAttacked(Color, new Vector2Int(position.x - 2, position.y))
                    )
                    {
                        moveablePositions.Add(new Vector2Int(position.x - 2, position.y));
                    }
                }

                rookPosition = Color == Color.White ? new Vector2Int(7, 0) : new Vector2Int(7, 7);
                figure = Board.GetFigure(rookPosition);
                if (figure != null && figure.Color == Color && !figure.HasMoved && figure is Rook)
                {
                    if (Board.GetFigure(new Vector2Int(position.x + 1, position.y)) == null &&
                        Board.GetFigure(new Vector2Int(position.x + 2, position.y)) == null &&
                        !GameManager.IsSquareAttacked(Color, position) &&
                        !GameManager.IsSquareAttacked(Color, new Vector2Int(position.x + 1, position.y)) &&
                        !GameManager.IsSquareAttacked(Color, new Vector2Int(position.x + 2, position.y))
                    )
                    {

                        moveablePositions.Add(new Vector2Int(position.x + 2, position.y));
                    }
                }
            }
        }

        private void RemoveAttackedSquares()
        {
            List<Vector2Int> attackedPositions = new List<Vector2Int>();
            foreach (Vector2Int position in moveablePositions)
            {
                if (GameManager.IsSquareAttacked(Color, position))
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