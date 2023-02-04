using System.Collections.Generic;

namespace Chess
{
    public class King : Figure
    {
        protected override void UpdatePositions()
        {
            moveablePositions.Clear();

            AddPossibleSquares();
            CheckCastle();
            RemoveAttackedSquares();
        }
        private void AddPossibleSquares()
        {
            List<Position> moveablePositions = new List<Position>
            {
                new Position(position.File + 1, position.Rank),
                new Position(position.File - 1, position.Rank),
                new Position(position.File, position.Rank + 1),
                new Position(position.File, position.Rank - 1),
                new Position(position.File + 1, position.Rank + 1),
                new Position(position.File + 1, position.Rank - 1),
                new Position(position.File - 1, position.Rank + 1),
                new Position(position.File - 1, position.Rank - 1)
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
                Position rookPosition = Color == Color.White ? new Position(0, 0) : new Position(0, 7);
                Figure figure = Board.GetFigure(rookPosition);
                if (figure != null && figure.Color == Color && !figure.HasMoved && figure is Rook)
                {
                    if (Board.GetFigure(new Position(position.File - 1, position.Rank)) == null &&
                        Board.GetFigure(new Position(position.File - 2, position.Rank)) == null &&
                        !GameManager.IsSquareAttacked(Color, position) &&
                        !GameManager.IsSquareAttacked(Color, new Position(position.File - 1, position.Rank)) &&
                        !GameManager.IsSquareAttacked(Color, new Position(position.File - 2, position.Rank))
                    )
                    {
                        moveablePositions.Add(new Position(position.File - 2, position.Rank));
                    }
                }

                rookPosition = Color == Color.White ? new Position(7, 0) : new Position(7, 7);
                figure = Board.GetFigure(rookPosition);
                if (figure != null && figure.Color == Color && !figure.HasMoved && figure is Rook)
                {
                    if (Board.GetFigure(new Position(position.File + 1, position.Rank)) == null &&
                        Board.GetFigure(new Position(position.File + 2, position.Rank)) == null &&
                        !GameManager.IsSquareAttacked(Color, position) &&
                        !GameManager.IsSquareAttacked(Color, new Position(position.File + 1, position.Rank)) &&
                        !GameManager.IsSquareAttacked(Color, new Position(position.File + 2, position.Rank))
                    )
                    {

                        moveablePositions.Add(new Position(position.File + 2, position.Rank));
                    }
                }
            }
        }

        // TODO: This has the problem that the king can move on a tile that e.g. the queen cannot currently reach because
        // the kings stands in between. Mabybe better to implement a way to check if the situation that results from a move is viable. 
        // That would also solve the problem of pinned figures. Also currently pawns remove squares where they move, not which they attack. 
        private void RemoveAttackedSquares()
        {
            List<Position> attackedPositions = new List<Position>();
            foreach (Position position in moveablePositions)
            {
                if (GameManager.IsSquareAttacked(Color, position))
                {
                    attackedPositions.Add(position);
                }
            }
            moveablePositions.RemoveAll((i) => attackedPositions.Contains(i));
        }

        // Check if move is castle to move the corresponding rook
        protected override void OnMove(Position oldPosition, Position newPosition)
        {
            base.OnMove(oldPosition, newPosition);
            int direction = oldPosition.File - newPosition.File;
            if (direction > 1)
            {
                Figure rook = Board.GetFigure(new Position(0, position.Rank));
                rook.Move(new Position(3, position.Rank));
            }
            else if (direction < -1)
            {
                Figure rook = Board.GetFigure(new Position(7, position.Rank));
                rook.Move(new Position(5, position.Rank));
            }
        }
    }
}