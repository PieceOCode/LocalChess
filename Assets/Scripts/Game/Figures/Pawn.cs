using UnityEngine;

namespace Chess
{
    public class Pawn : Figure
    {
        public bool HasMovedByTwoLastMove { get; private set; }

        public Pawn(Vector2Int position, Color color, GameState gameState) : base(position, color, gameState) { }

        public override void UpdatePositions()
        {
            UpdateForwardMove();
            UpdateDiagonalMoves();
            UpdateEnPassantMoves();
        }

        public void UpdatedHasMovedByTwo(Vector2Int lastMoveDestination)
        {
            if (lastMoveDestination == this.position)
            {
                HasMovedByTwoLastMove = true;
            }
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

        private void UpdateDiagonalMoves()
        {
            int rankDirection = Color == Color.White ? 1 : -1;

            Vector2Int possiblePosition = new Vector2Int(position.x + 1, position.y + rankDirection);
            UpdateDiagonalMove(possiblePosition);

            possiblePosition = new Vector2Int(position.x - 1, position.y + rankDirection);
            UpdateDiagonalMove(possiblePosition);
        }

        private void UpdateDiagonalMove(Vector2Int possiblePosition)
        {
            if (Board.IsPositionValid(possiblePosition))
            {
                if (Board.SquareHasEnemyPiece(this.Color, possiblePosition))
                {
                    moveablePositions.Add(possiblePosition);
                }
                attackedPositions.Add(possiblePosition);
            }
        }

        private void UpdateEnPassantMoves()
        {
            Vector2Int enemyPawnPosition = new Vector2Int(position.x + 1, position.y);
            UpdateEnPassantMove(enemyPawnPosition);

            enemyPawnPosition = new Vector2Int(position.x - 1, position.y);
            UpdateEnPassantMove(enemyPawnPosition);
        }

        private void UpdateEnPassantMove(Vector2Int enemyPawnPosition)
        {
            if (Board.IsPositionValid(enemyPawnPosition) && Board.SquareHasEnemyPiece(Color, enemyPawnPosition))
            {
                Figure figure = Board.GetFigure(enemyPawnPosition);
                if (figure is Pawn && (figure as Pawn).HasMovedByTwoLastMove)
                {
                    int rankDirection = Color == Color.White ? 1 : -1;
                    moveablePositions.Add(enemyPawnPosition + Vector2Int.up * rankDirection);
                }
            }
        }
    }
}