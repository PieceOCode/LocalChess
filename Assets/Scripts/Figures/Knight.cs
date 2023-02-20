using System.Collections.Generic;

namespace Chess
{
    public class Knight : Figure
    {
        public override void UpdatePositions()
        {
            base.UpdatePositions();

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

        // Knight cannot move if he is pinned. 
        public override void UpdatePinned()
        {
            moveablePositions.Clear();
        }
    }
}