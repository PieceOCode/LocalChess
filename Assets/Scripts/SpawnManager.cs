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


        public Pawn CreatePawn(Color color, Vector2Int position)
        {
            Pawn pawn = new Pawn(this);
            InitializeFigure(pawn, pawnPrefab, color, position);
            return pawn;
        }

        public Bishop CreateBishop(Color color, Vector2Int position)
        {
            Bishop bishop = new Bishop();
            InitializeFigure(bishop, bishopPrefab, color, position);
            return bishop;
        }

        public Knight CreateKnight(Color color, Vector2Int position)
        {
            Knight knight = new Knight();
            InitializeFigure(knight, knightPrefab, color, position);
            return knight;
        }

        public Rook CreateRook(Color color, Vector2Int position)
        {
            Rook rook = new Rook();
            InitializeFigure(rook, rookPrefab, color, position);
            return rook;
        }

        public Queen CreateQueen(Color color, Vector2Int position)
        {
            Queen queen = new Queen();
            InitializeFigure(queen, queenPrefab, color, position);
            return queen;
        }

        public King CreateKing(Color color, Vector2Int position)
        {
            King king = new King();
            InitializeFigure(king, kingPrefab, color, position);
            return king;
        }

        // TODO: Change this to only spawn a figure representation and hook up in game callbacks. Figures should be able to be spawned independently.
        public void InitializeFigure(Figure figure, FigureRepresentation prefab, Color color, Vector2Int position)
        {
            figure.SetFigure(position, color, gameManager);
            gameManager.Board.SetFigureToSquare(figure, position);

            gameManager.RegisterFigure(figure);
            FigureRepresentation figureRepresentation = Instantiate(prefab);
            figureRepresentation.Initialize(figure, boardRepresentation);
        }

        // TODO: this only spawns figures but does not reset.
        public void ResetBoardStandard()
        {
            CreateRook(Color.White, new Vector2Int(0, 0));
            CreateRook(Color.White, new Vector2Int(7, 0));
            CreateRook(Color.Black, new Vector2Int(0, 7));
            CreateRook(Color.Black, new Vector2Int(7, 7));

            CreateKnight(Color.White, new Vector2Int(1, 0));
            CreateKnight(Color.White, new Vector2Int(6, 0));
            CreateKnight(Color.Black, new Vector2Int(1, 7));
            CreateKnight(Color.Black, new Vector2Int(6, 7));

            CreateBishop(Color.White, new Vector2Int(2, 0));
            CreateBishop(Color.White, new Vector2Int(5, 0));
            CreateBishop(Color.Black, new Vector2Int(2, 7));
            CreateBishop(Color.Black, new Vector2Int(5, 7));

            CreateQueen(Color.White, new Vector2Int(3, 0));
            CreateQueen(Color.Black, new Vector2Int(3, 7));

            CreateKing(Color.White, new Vector2Int(4, 0));
            CreateKing(Color.Black, new Vector2Int(4, 7));

            for (int i = 0; i < gameManager.Board.Width; i++)
            {
                CreatePawn(Color.White, new Vector2Int(i, 1));
                CreatePawn(Color.Black, new Vector2Int(i, 6));
            }
        }
    }
}