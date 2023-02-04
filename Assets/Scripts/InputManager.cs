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

        public void DeregisterFigure(Figure figure)
        {
            figure.OnFigureSelectedEvent -= OnFigureSelected;
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
                    if (selectedFigure.CanMove(figure.Position))
                    {
                        //MoveSelectedFigure(figure.Position);
                        selectedFigure.Move(figure.Position);
                    }
                }
            }
            UpdateHighlights();
        }

        private void OnSquareSelected(Square square)
        {
            Debug.Log($"OnSqaureSelected in Board: on square {square.Position}");
            if (selectedFigure != null && selectedFigure.CanMove(square.Position))
            {
                //MoveSelectedFigure(square.Position);
                selectedFigure.Move(square.Position);
            }
            selectedFigure = null;
            UpdateHighlights();
        }

        private void MoveSelectedFigure(Position position)
        {
            // TODO: Think about who moves a piece and which script holds the functionality to move it.
            //board.GetSquare(selectedFigure.Position).Figure = null;
            //selectedFigure.Move(position);
            //if (GetSquare(position).Figure)
            //{
            //    Destroy(GetSquare(position).Figure.gameObject);
            //}

            //GetSquare(position).Figure = selectedFigure;
            //selectedFigure = null;
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
