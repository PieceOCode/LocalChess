using UnityEngine;

namespace Chess
{
    public class Rook : Figure
    {
        public Rook(Vector2Int position, Color color, GameState gameState) : base(position, color, gameState) { }

        public override void UpdatePositions()
        {
            UpdateLine(1, 0);
            UpdateLine(-1, 0);
            UpdateLine(0, 1);
            UpdateLine(0, -1);
        }
    }
}