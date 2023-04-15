
using UnityEngine;
using UnityEngine.EventSystems;

namespace Chess
{
    // Represents a square in the game. Reacts to clicks. 
    public class SquareRepresentation : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private SpriteRenderer sprite = default;

        public delegate void OnSquareSelectedHandler(SquareRepresentation square);
        public event OnSquareSelectedHandler OnSquareSelectedEvent;
        public Vector2Int Position => position;


        private Vector2Int position = default;

        public void SetSquare(Vector2Int position, UnityEngine.Color color)
        {
            this.position = position;
            sprite.color = color;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnSquareSelectedEvent?.Invoke(this);
        }
    }
}
