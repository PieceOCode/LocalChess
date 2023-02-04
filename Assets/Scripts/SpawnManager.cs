using UnityEngine;

namespace Chess
{
    /// <summary>
    /// Handles spawning and despawning figures from prefabs and registers their callbacks at the appropriate managers.
    /// </summary>
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField]
        private Board board = default;
        [SerializeField]
        private InputManager inputManager = default;
        [SerializeField]
        private GameManager gameManager = null;

        [Header("Figure Prefabs")]
        [SerializeField]
        private Pawn pawnPrefab = default;
        [SerializeField]
        private Bishop bishopPrefab = default;
        [SerializeField]
        private Knight knightPrefab = default;
        [SerializeField]
        private Rook rookPrefab = default;
        [SerializeField]
        private Queen queenPrefab = default;
        [SerializeField]
        private King kingPrefab = default;


        private void Start()
        {
            CreateStandardFigures();
        }

        private void CreateFigure(Figure prefab, Color color, Position position)
        {
            Figure figure = Instantiate(prefab);
            figure.SetFigure(position, color, gameManager);
            gameManager.RegisterFigure(figure);
            inputManager.RegisterFigure(figure);
            board.SetFigureToSquare(figure, position);
        }

        public void SpawnFigure<FigureType>() where FigureType : Figure
        {

        }

        private void CreateStandardFigures()
        {
            CreateFigure(rookPrefab, Color.White, new Position(0, 0));
            CreateFigure(rookPrefab, Color.White, new Position(7, 0));
            CreateFigure(rookPrefab, Color.Black, new Position(0, 7));
            CreateFigure(rookPrefab, Color.Black, new Position(7, 7));

            CreateFigure(knightPrefab, Color.White, new Position(1, 0));
            CreateFigure(knightPrefab, Color.White, new Position(6, 0));
            CreateFigure(knightPrefab, Color.Black, new Position(1, 7));
            CreateFigure(knightPrefab, Color.Black, new Position(6, 7));

            CreateFigure(bishopPrefab, Color.White, new Position(2, 0));
            CreateFigure(bishopPrefab, Color.White, new Position(5, 0));
            CreateFigure(bishopPrefab, Color.Black, new Position(2, 7));
            CreateFigure(bishopPrefab, Color.Black, new Position(5, 7));

            CreateFigure(queenPrefab, Color.White, new Position(3, 0));
            CreateFigure(queenPrefab, Color.Black, new Position(3, 7));

            CreateFigure(kingPrefab, Color.White, new Position(4, 0));
            CreateFigure(kingPrefab, Color.Black, new Position(4, 7));

            for (int i = 0; i < board.Width; i++)
            {
                CreateFigure(pawnPrefab, Color.White, new Position(i, 1));
                CreateFigure(pawnPrefab, Color.Black, new Position(i, 6));
            }
        }
    }
}