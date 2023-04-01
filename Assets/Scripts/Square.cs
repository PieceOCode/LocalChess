
using UnityEngine;
using UnityEngine.EventSystems;

namespace Chess
{
    // Represents a square in the game. Reacts to clicks. 
    public class Square : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private SpriteRenderer sprite = default;
        [SerializeField]
        private UnityEngine.Color brightColor = UnityEngine.Color.white;
        [SerializeField]
        private UnityEngine.Color darkColor = UnityEngine.Color.black;

        public delegate void OnSquareSelectedHandler(Square square);
        public event OnSquareSelectedHandler OnSquareSelectedEvent;
        public Vector2Int Position => position;


        private Vector2Int position = default;

        public void SetSquare(Vector2Int position, Color color)
        {
            this.position = position;
            sprite.color = color == Color.White ? brightColor : darkColor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnSquareSelectedEvent?.Invoke(this);
        }
    }
}
