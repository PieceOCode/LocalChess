using UnityEngine;

namespace Chess
{
    // Represents a figure as a sprite on the chess board. 
    public class FigureRepresentation : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer whiteSprite = default;
        [SerializeField]
        private SpriteRenderer blackSprite = default;
        [SerializeField]
        private int hightlightedSortingOrder = default;

        private int normalSortingOrder = default;
        [HideInInspector]
        public Vector2Int position = default;

        public void SetColor(Color color)
        {
            whiteSprite.gameObject.SetActive(color == Color.White);
            blackSprite.gameObject.SetActive(color == Color.Black);
        }

        public void EnableHighlight()
        {
            normalSortingOrder = whiteSprite.sortingOrder;
            whiteSprite.sortingOrder = 10;
            blackSprite.sortingOrder = 10;
        }

        public void DisableHighlight()
        {
            whiteSprite.sortingOrder = normalSortingOrder;
            blackSprite.sortingOrder = normalSortingOrder;
        }
    }
}
