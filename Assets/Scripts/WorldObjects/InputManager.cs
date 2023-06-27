using UnityEngine;
using UnityEngine.InputSystem;

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
        [SerializeField]
        private PlayerInput inputSystemActions = default;
        [SerializeField]
        private FigureSpawner figureSpawner = default;
        [SerializeField]
        private float pointerDragDistance = 0.3f;

        private InputAction pressAction = default;
        private InputAction positionAction = default;

        private bool pointerIsPressed = false;
        private Vector2 pointerPressedStartPosition = default;

        private FigureRepresentation figureRepresentation = null;
        private Vector2 draggedFigureStartPosition = default;
        private bool isDragging = false;

        private Board Board => gameManager.ActiveGame.Board;
        private Figure selectedFigure = null;

        private void Awake()
        {
            pressAction = inputSystemActions.actions.FindAction("Click");
            positionAction = inputSystemActions.actions.FindAction("PointerPosition");
        }

        private void OnEnable()
        {
            pressAction.performed += OnPressStarted;
            pressAction.canceled += OnPressEnded;
        }

        private void OnDisable()
        {
            pressAction.performed -= OnPressStarted;
            pressAction.canceled -= OnPressEnded;
        }
        private void OnPressStarted(InputAction.CallbackContext context)
        {
            pointerIsPressed = true;
            pointerPressedStartPosition = positionAction.ReadValue<Vector2>();
            SquareRepresentation square = RaycastAtPointer();

            if (square != null)
            {
                if (selectedFigure != null && selectedFigure.CanMoveTo(square.Position))
                {
                    MoveFigure(selectedFigure, square.Position);
                    selectedFigure = null;
                }
                else if (!Board.SquareIsEmpty(square.Position))
                {
                    Figure figure = Board.GetFigure(square.Position);
                    if (figure.Color == gameManager.ActiveGame.ActivePlayer)
                    {
                        selectedFigure = figure;
                    }
                }
                else
                {
                    selectedFigure = null;
                }
            }
            else
            {
                selectedFigure = null;
            }

            UpdateHighlights();
        }

        private void OnPressEnded(InputAction.CallbackContext context)
        {
            pointerIsPressed = false;

            if (!isDragging)
            {
                return;
            }

            SquareRepresentation square = RaycastAtPointer();
            if (square != null && selectedFigure.CanMoveTo(square.Position))
            {
                MoveFigure(selectedFigure, square.Position);
                selectedFigure = null;
            }
            else
            {
                // If move is canceled move dragged figure back to its original position
                figureRepresentation.DisableHighlight();
                figureRepresentation.transform.position = draggedFigureStartPosition;
            }

            isDragging = false;
            figureRepresentation = null;

            UpdateHighlights();
        }

        private void Update()
        {
            Vector2 pointerPosition = positionAction.ReadValue<Vector2>();

            // Start dragging if a figure is selected and the pointer moved a certain distance from the first press.
            if (pointerIsPressed
                && !isDragging
                && selectedFigure != null
                && Vector2.Distance(pointerPressedStartPosition, pointerPosition) > pointerDragDistance)
            {
                isDragging = true;
                figureRepresentation = figureSpawner.GetFigureRepresentation(selectedFigure.Position);
                draggedFigureStartPosition = figureRepresentation.transform.position;
            }

            // Attach the dragged figure to the pointer.
            if (isDragging)
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pointerPosition);
                worldPosition.z = figureRepresentation.transform.position.z;
                figureRepresentation.EnableHighlight();
                figureRepresentation.transform.position = worldPosition;
            }
        }

        private SquareRepresentation RaycastAtPointer()
        {
            Vector2 pos = positionAction.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                SquareRepresentation square = hit.collider.gameObject.GetComponent<SquareRepresentation>();
                return square;
            }

            return null;
        }

        private void MoveFigure(Figure figure, Vector2Int position)
        {
            Move move = null;
            if (selectedFigure is King && Vector2Int.Distance(selectedFigure.Position, position) == 2)
            {
                move = new CastleMove(selectedFigure as King, selectedFigure.Position, position);
            }
            else if (selectedFigure is Pawn && (position.y == 0 || position.y == Board.Height - 1))
            {
                move = new PawnPromotion(selectedFigure as Pawn, selectedFigure.Position, position);
            }
            else
            {
                move = new Move(selectedFigure, selectedFigure.Position, position);
            }
            gameManager.ActiveGame.ExecuteMove(move);
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
