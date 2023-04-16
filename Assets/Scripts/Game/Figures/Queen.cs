using UnityEngine;

namespace Chess
{
    public class Queen : Figure
    {
        public Queen(Vector2Int position, Color color, GameState gameState) : base(position, color, gameState) { }

        public override void UpdatePositions()
        {
            UpdateStraight();
            UpdateDiagonals();
        }

        private void UpdateStraight()
        {
            UpdateLine(1, 0);
            UpdateLine(-1, 0);
            UpdateLine(0, 1);
            UpdateLine(0, -1);
        }

        private void UpdateDiagonals()
        {
            UpdateLine(1, 1);
            UpdateLine(1, -1);
            UpdateLine(-1, 1);
            UpdateLine(-1, -1);
        }
    }
}