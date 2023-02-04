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

        public void RegisterFigure(Figure figure)
        {
            figure.OnFigureSelectedEvent += OnFigureSelected;
        }

        public void RegisterSquare(Square square)
        {
            square.OnSquareSelectedEvent += OnSquareSelected;
        }

        private void OnFigureSelected(Figure figure)
        {
            Debug.Log($"OnFigureSelected in Board: Figure {figure.GetType().Name} on square {figure.Position}");
            if (selectedFigure == null)
            {
                selectedFigure = figure;
            }
            else
            {
                if (figure.Color != selectedFigure.Color)
                {
                    MoveSelectedFigure(figure.Position);
                }
            }
            UpdateHighlights();
        }

        private void OnSquareSelected(Square square)
        {
            Debug.Log($"OnSqaureSelected in Board: on square {square.Position}");
            if (selectedFigure != null)
            {
                MoveSelectedFigure(square.Position);
            }
        }

        private void MoveSelectedFigure(Position position)
        {
            if (selectedFigure.CanMove(position))
            {
                selectedFigure.Move(position);
            }
            selectedFigure = null;
            UpdateHighlights();
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
