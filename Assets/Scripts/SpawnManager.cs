using UnityEngine;

namespace Chess
{
    /// <summary>
    /// Handles spawning and despawning figures from prefabs and registers their callbacks at the appropriate managers.
    /// </summary>
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField]
        private GameManager gameManager = null;
        [SerializeField]
        private BoardRepresentation boardRepresentation = null;

        [Header("Figure Prefabs")]
        [SerializeField]
        private FigureRepresentation pawnPrefab = default;
        [SerializeField]
        private FigureRepresentation bishopPrefab = default;
        [SerializeField]
        private FigureRepresentation knightPrefab = default;
        [SerializeField]
        private FigureRepresentation rookPrefab = default;
        [SerializeField]
        private FigureRepresentation queenPrefab = default;
        [SerializeField]
        private FigureRepresentation kingPrefab = default;

        private void CreateFigure<FigureType>(FigureRepresentation prefab, Color color, Vector2Int position)
            where FigureType : Figure, new()
        {
            FigureType figure = new FigureType();

            figure.SetFigure(position, color, gameManager);
            gameManager.RegisterFigure(figure);
            gameManager.Board.SetFigureToSquare(figure, position);

            FigureRepresentation figureRepresentation = Instantiate(prefab);
            figureRepresentation.Initialize(figure, boardRepresentation);
        }

        public void ResetBoardStandard()
        {
            CreateFigure<Rook>(rookPrefab, Color.White, new Vector2Int(0, 0));
            CreateFigure<Rook>(rookPrefab, Color.White, new Vector2Int(7, 0));
            CreateFigure<Rook>(rookPrefab, Color.Black, new Vector2Int(0, 7));
            CreateFigure<Rook>(rookPrefab, Color.Black, new Vector2Int(7, 7));

            CreateFigure<Knight>(knightPrefab, Color.White, new Vector2Int(1, 0));
            CreateFigure<Knight>(knightPrefab, Color.White, new Vector2Int(6, 0));
            CreateFigure<Knight>(knightPrefab, Color.Black, new Vector2Int(1, 7));
            CreateFigure<Knight>(knightPrefab, Color.Black, new Vector2Int(6, 7));

            CreateFigure<Bishop>(bishopPrefab, Color.White, new Vector2Int(2, 0));
            CreateFigure<Bishop>(bishopPrefab, Color.White, new Vector2Int(5, 0));
            CreateFigure<Bishop>(bishopPrefab, Color.Black, new Vector2Int(2, 7));
            CreateFigure<Bishop>(bishopPrefab, Color.Black, new Vector2Int(5, 7));

            CreateFigure<Queen>(queenPrefab, Color.White, new Vector2Int(3, 0));
            CreateFigure<Queen>(queenPrefab, Color.Black, new Vector2Int(3, 7));

            CreateFigure<King>(kingPrefab, Color.White, new Vector2Int(4, 0));
            CreateFigure<King>(kingPrefab, Color.Black, new Vector2Int(4, 7));

            for (int i = 0; i < gameManager.Board.Width; i++)
            {
                CreateFigure<Pawn>(pawnPrefab, Color.White, new Vector2Int(i, 1));
                CreateFigure<Pawn>(pawnPrefab, Color.Black, new Vector2Int(i, 6));
            }
        }
    }
}