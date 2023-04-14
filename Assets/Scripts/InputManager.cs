using UnityEngine;

namespace Chess
{
    /// <summary>
    /// Handles all input on figures and squares and currently also movement. 
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        private GameManager gameManager = default;
        [SerializeField]
        private Highlights highlights = default;

        private Board Board => gameManager.Board;
        private Figure selectedFigure = null;
        private Color activePlayer = Color.White;

        public void RegisterSquare(Square square)
        {
            square.OnSquareSelectedEvent += OnSquareSelected;
        }

        private void OnSquareSelected(Square square)
        {
            Debug.Log($"OnSqaureSelected in Board: on square {square.Position}");
            if (selectedFigure != null && selectedFigure.CanMove(square.Position))
            {
                Move move = new Move(selectedFigure, selectedFigure.Position, square.Position);
                gameManager.Match.Add(move);
                gameManager.Match.Redo(Board);

                selectedFigure = null;
                UpdateHighlights();
                activePlayer = activePlayer == Color.White ? Color.Black : Color.White;
            }
            else if (!Board.SquareIsEmpty(square.Position))
            {
                selectedFigure = Board.GetFigure(square.Position);
                if (selectedFigure.Color != activePlayer)
                {
                    selectedFigure = null;
                }
                UpdateHighlights();
            }
            else
            {
                selectedFigure = null;
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
