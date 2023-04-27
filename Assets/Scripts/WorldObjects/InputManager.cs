using UnityEngine;

namespace Chess
{
    /// <summary>
    /// Handles input on figures and squares. 
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        private GameManager gameManager = default;
        [SerializeField]
        private Highlights highlights = default;

        private Board Board => gameManager.ActiveGame.Board;
        private Figure selectedFigure = null;

        public void RegisterSquare(SquareRepresentation square)
        {
            square.OnSquareSelectedEvent += OnSquareSelected;
        }

        private void OnSquareSelected(SquareRepresentation square)
        {
            Debug.Log($"OnSqaureSelected in Board: on square {square.Position}");
            if (selectedFigure != null && selectedFigure.CanMoveTo(square.Position))
            {
                Move move = null;
                if (selectedFigure is King && Vector2Int.Distance(selectedFigure.Position, square.Position) == 2)
                {
                    move = new CastleMove(selectedFigure as King, selectedFigure.Position, square.Position);
                }
                else if (selectedFigure is Pawn && (square.Position.y == 0 || square.Position.y == Board.Height - 1))
                {
                    move = new PawnPromotion(selectedFigure as Pawn, selectedFigure.Position, square.Position);
                }
                else
                {
                    move = new Move(selectedFigure, selectedFigure.Position, square.Position);
                }
                gameManager.ActiveGame.ExecuteMove(move);

                selectedFigure = null;
                UpdateHighlights();
            }
            else if (!Board.SquareIsEmpty(square.Position))
            {
                selectedFigure = Board.GetFigure(square.Position);
                if (selectedFigure.Color != gameManager.ActiveGame.ActivePlayer)
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
