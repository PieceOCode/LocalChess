using UnityEngine;

namespace Chess
{
    /// <summary>
    /// Handles all input on figures and squares and currently also movement. 
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        private Board board;
        [SerializeField]
        Highlights highlights;

        private Figure selectedFigure = null;
        private Color activePlayer;

        public void RegisterSquare(Square square)
        {
            square.OnSquareSelectedEvent += OnSquareSelected;
        }

        private void OnSquareSelected(Square square)
        {
            Debug.Log($"OnSqaureSelected in Board: on square {square.Position}");
            if (selectedFigure != null && selectedFigure.CanMove(square.Position))
            {
                selectedFigure.Move(square.Position);
                selectedFigure = null;
                UpdateHighlights();
                activePlayer = activePlayer == Color.White ? Color.Black : Color.White;
            }
            else
            {
                selectedFigure = board.GetFigure(square.Position);
                if (selectedFigure != null && selectedFigure.Color != activePlayer)
                {
                    selectedFigure = null;
                }
                UpdateHighlights();
            }
        }

        private void UpdateHighlights()
        {
            if (selectedFigure == null)
            {
                highlights.ClearHighlights();
                return;
            }

            highlights.HighlightSquares(selectedFigure.MoveablePositions);
        }
    }
}
