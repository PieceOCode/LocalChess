namespace Chess
{
    // TODO: Implement what happens when pawn reaches last row. 
    public class Pawn : Figure
    {
        public override void UpdatePositions()
        {
            base.UpdatePositions();

            UpdateForwardMove();
            UpdateDiagonalMove();
        }

        // Check if pawn can move forward
        private void UpdateForwardMove()
        {
            int rankDirection = Color == Color.White ? 1 : -1;

            Position possiblePosition = new Position(position.File, position.Rank + rankDirection);
            if (Board.SquareIsEmpty(possiblePosition))
            {
                moveablePositions.Add(possiblePosition);

                // Check if pawn can also move two field forwards
                if ((Color == Color.White && position.Rank == 1) || (Color == Color.Black && position.Rank == 6))
                {
                    possiblePosition = new Position(position.File, position.Rank + rankDirection * 2);
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

            Position possiblePosition = new Position(position.File + 1, position.Rank + rankDirection);
            if (possiblePosition.IsValid())
            {
                if (Board.SquareHasEnemyPiece(this.Color, possiblePosition))
                {
                    moveablePositions.Add(possiblePosition);
                }
                attackedPositions.Add(possiblePosition);
            }

            possiblePosition = new Position(position.File - 1, position.Rank + rankDirection);
            if (possiblePosition.IsValid())
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