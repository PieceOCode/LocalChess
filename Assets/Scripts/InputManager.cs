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

        private Figure selectedFigure = null;

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
            }
            else
            {
                selectedFigure = board.GetFigure(square.Position);
                UpdateHighlights();
            }
        }

        private void UpdateHighlights()
        {
            if (selectedFigure == null)
            {
                board.ClearHighlights();
                return;
            }

            board.HighlightSquares(selectedFigure.MoveablePositions);
        }
    }
}
