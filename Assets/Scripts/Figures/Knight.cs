using System.Collections.Generic;
using UnityEngine;


namespace Chess
{
    public class Knight : Figure
    {
        public Knight(Vector2Int position, Color color, GameState gameState) : base(position, color, gameState) { }

        public override void UpdatePositions()
        {
            List<Vector2Int> moveablePositions = new List<Vector2Int>
            {
                new Vector2Int(position.x + 2, position.y + 1),
                new Vector2Int(position.x + 2, position.y - 1),
                new Vector2Int(position.x - 2, position.y + 1),
                new Vector2Int(position.x - 2, position.y - 1),
                new Vector2Int(position.x + 1, position.y + 2),
                new Vector2Int(position.x + 1, position.y - 2),
                new Vector2Int(position.x - 1, position.y + 2),
                new Vector2Int(position.x - 1, position.y - 2)
            };

            foreach (var position in moveablePositions)
            {
                UpdateField(position);
            }
        }

        // Knight cannot move if he is pinned. 
        public override void UpdatePinned()
        {
            if (pinnedBy != null)
            {
                moveablePositions.Clear();
            }
        }
    }
}