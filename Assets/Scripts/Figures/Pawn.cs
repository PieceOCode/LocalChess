using UnityEngine;

namespace Chess
{
    // TODO: Implement what happens when pawn reaches last row. 
    public class Pawn : Figure
    {
        public override void UpdatePositions()
        {
            UpdateForwardMove();
            UpdateDiagonalMove();
        }

        // Check if pawn can move forward
        private void UpdateForwardMove()
        {
            int rankDirection = Color == Color.White ? 1 : -1;

            Vector2Int possiblePosition = new Vector2Int(position.x, position.y + rankDirection);
            if (Board.SquareIsEmpty(possiblePosition))
            {
                moveablePositions.Add(possiblePosition);

                // Check if pawn can also move two field forwards
                if ((Color == Color.White && position.y == 1) || (Color == Color.Black && position.y == 6))
                {
                    possiblePosition = new Vector2Int(position.x, position.y + rankDirection * 2);
                    if (Board.SquareIsEmpty(possiblePosition))
                    {
                        moveablePositions.Add(possiblePosition);
                    }
                }
            }
        }

        // TODO: Implement en passant
        // Check if pawn can move diagonally by taking out an opponent's piece
        private void UpdateDiagonalMove()
        {
            int rankDirection = Color == Color.White ? 1 : -1;

            Vector2Int possiblePosition = new Vector2Int(position.x + 1, position.y + rankDirection);
            if (Board.IsPositionValid(possiblePosition))
            {
                if (Board.SquareHasEnemyPiece(this.Color, possiblePosition))
                {
                    moveablePositions.Add(possiblePosition);
                }
                attackedPositions.Add(possiblePosition);
            }

            possiblePosition = new Vector2Int(position.x - 1, position.y + rankDirection);
            if (Board.IsPositionValid(possiblePosition))
            {
                if (Board.SquareHasEnemyPiece(this.Color, possiblePosition))
                {
                    moveablePositions.Add(possiblePosition);
                }
                attackedPositions.Add(possiblePosition);
            }
        }
    }
}