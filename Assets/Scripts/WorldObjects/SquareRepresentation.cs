
using UnityEngine;

namespace Chess
{
    // Represents a square in the game.
    public class SquareRepresentation : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer sprite = default;

        public Vector2Int Position => position;

        private Vector2Int position = default;

        public void SetSquare(Vector2Int position, UnityEngine.Color color)
        {
            this.position = position;
            sprite.color = color;
        }
    }
}
