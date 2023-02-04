using System.Collections.Generic;

namespace Chess
{
    public class Knight : Figure
    {
        protected override void UpdatePositions()
        {
            base.moveablePositions.Clear();

            List<Position> moveablePositions = new List<Position>
            {
                new Position(position.File + 2, position.Rank + 1),
                new Position(position.File + 2, position.Rank - 1),
                new Position(position.File - 2, position.Rank + 1),
                new Position(position.File - 2, position.Rank - 1),
                new Position(position.File + 1, position.Rank + 2),
                new Position(position.File + 1, position.Rank - 2),
                new Position(position.File - 1, position.Rank + 2),
                new Position(position.File - 1, position.Rank - 2)
            };

            foreach (var position in moveablePositions)
            {
                UpdateField(position);
            }
        }
    }
}