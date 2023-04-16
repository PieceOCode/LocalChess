using UnityEngine;

namespace Chess
{
    // Represents a figure as a sprite on the chess board. 
    public class FigureRepresentation : MonoBehaviour
    {
        [SerializeField]
        private GameObject whiteSprite = default;
        [SerializeField]
        private GameObject blackSprite = default;

        public void SetColor(Color color)
        {
            whiteSprite.SetActive(color == Color.White);
            blackSprite.SetActive(color == Color.Black);
        }
    }
}
