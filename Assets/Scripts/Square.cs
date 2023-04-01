
using UnityEngine;
using UnityEngine.EventSystems;

namespace Chess
{
    public class Square : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private UnityEngine.Color brightColor = UnityEngine.Color.white;
        [SerializeField]
        private UnityEngine.Color darkColor = UnityEngine.Color.black;

        private Vector2Int position = default;
        private Color color = default;
        private SpriteRenderer sprite = default;
        private Figure figure = null;


        public delegate void OnSquareSelectedHandler(Square square);
        public event OnSquareSelectedHandler OnSquareSelectedEvent;

        private SpriteRenderer Sprite
        {
            get
            {
                if (sprite == null)
                {
                    sprite = GetComponent<SpriteRenderer>();
                }
                return sprite;
            }
        }

        public Figure Figure
        {
            get
            {
                return figure;
            }
            set
            {
                figure = value;
            }
        }

        public Vector2Int Position => position;
        public Color Color => color;
        public bool IsEmpty => figure == null;


        public void SetSquare(Vector2Int position, Color color)
        {
            this.position = position;
            this.color = color;
            Sprite.color = this.color == Color.White ? brightColor : darkColor;
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            OnSquareSelectedEvent?.Invoke(this);
        }
    }
}
