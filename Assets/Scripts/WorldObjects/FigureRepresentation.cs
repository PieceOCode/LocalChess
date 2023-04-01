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

        private Board board;

        public void Initialize(Figure figure, Board board)
        {
            this.board = board;
            transform.position = board.GetWorldPosition(figure.Position);

            whiteSprite.SetActive(figure.Color == Color.White);
            blackSprite.SetActive(figure.Color == Color.Black);

            figure.OnFigureDestroyedEvent += OnFigureDestroyed;
            figure.OnFigureMovedEvent += OnFigureMoved;
        }

        private void OnFigureMoved(Figure figure)
        {
            transform.position = board.GetWorldPosition(figure.Position);
        }

        private void OnFigureDestroyed(Figure figure)
        {
            figure.OnFigureDestroyedEvent -= OnFigureDestroyed;
            figure.OnFigureMovedEvent -= OnFigureMoved;
            Destroy(gameObject);
        }
    }
}
