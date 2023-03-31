using UnityEngine;

namespace Chess
{
    /// <summary>
    /// Handles spawning and despawning figures from prefabs and registers their callbacks at the appropriate managers.
    /// </summary>
    public class SpawnManager : MonoBehaviour
    {
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

        private void CreateFigure(Figure prefab, Color color, Vector2Int position)
        {
            Figure figure = Instantiate(prefab);
            figure.SetFigure(position, color, gameManager);
            gameManager.RegisterFigure(figure);
            gameManager.Board.SetFigureToSquare(figure, position);
        }

        public void SpawnFigure<FigureType>() where FigureType : Figure
        {

        }

        public void ResetBoardStandard()
        {
            CreateFigure(rookPrefab, Color.White, new Vector2Int(0, 0));
            CreateFigure(rookPrefab, Color.White, new Vector2Int(7, 0));
            CreateFigure(rookPrefab, Color.Black, new Vector2Int(0, 7));
            CreateFigure(rookPrefab, Color.Black, new Vector2Int(7, 7));

            CreateFigure(knightPrefab, Color.White, new Vector2Int(1, 0));
            CreateFigure(knightPrefab, Color.White, new Vector2Int(6, 0));
            CreateFigure(knightPrefab, Color.Black, new Vector2Int(1, 7));
            CreateFigure(knightPrefab, Color.Black, new Vector2Int(6, 7));

            CreateFigure(bishopPrefab, Color.White, new Vector2Int(2, 0));
            CreateFigure(bishopPrefab, Color.White, new Vector2Int(5, 0));
            CreateFigure(bishopPrefab, Color.Black, new Vector2Int(2, 7));
            CreateFigure(bishopPrefab, Color.Black, new Vector2Int(5, 7));

            CreateFigure(queenPrefab, Color.White, new Vector2Int(3, 0));
            CreateFigure(queenPrefab, Color.Black, new Vector2Int(3, 7));

            CreateFigure(kingPrefab, Color.White, new Vector2Int(4, 0));
            CreateFigure(kingPrefab, Color.Black, new Vector2Int(4, 7));

            for (int i = 0; i < gameManager.Board.Width; i++)
            {
                CreateFigure(pawnPrefab, Color.White, new Vector2Int(i, 1));
                CreateFigure(pawnPrefab, Color.Black, new Vector2Int(i, 6));
            }
        }
    }
}