using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess
{
    /// <summary>
    /// Holds and creates in game representations of the pieces on the board. 
    /// </summary>
    public class FigureSpawner : MonoBehaviour
    {
        [SerializeField]
        private BoardRepresentation boardRepresentation = null;
        [SerializeField]
        private GameManager gameManager = null;

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


        private List<FigureRepresentation> figures = new List<FigureRepresentation>();

        private void Awake()
        {
            gameManager.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            gameManager.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(Match match, GameState gameState)
        {
            ClearRepresentations();
            foreach (var f in gameState.Pieces)
            {
                CreateRepresentation(f);
            }
        }

        private void ClearRepresentations()
        {
            foreach (FigureRepresentation figure in figures)
            {
                Destroy(figure.gameObject);
            }
            figures.Clear();
        }

        private void CreateRepresentation(Figure figure)
        {
            FigureRepresentation figureRepresentation = Instantiate(GetPrefab(figure));
            figureRepresentation.position = figure.Position;
            figureRepresentation.transform.position = boardRepresentation.GetWorldPosition(figure.Position);
            figureRepresentation.SetColor(figure.Color);
            figures.Add(figureRepresentation);
        }

        private FigureRepresentation GetPrefab(Figure figure)
        {
            if (figure.GetType() == typeof(Pawn)) { return pawnPrefab; }
            else if (figure.GetType() == typeof(Bishop)) { return bishopPrefab; }
            else if (figure.GetType() == typeof(Knight)) { return knightPrefab; }
            else if (figure.GetType() == typeof(Rook)) { return rookPrefab; }
            else if (figure.GetType() == typeof(Queen)) { return queenPrefab; }
            else if (figure.GetType() == typeof(King)) { return kingPrefab; }
            else return null;
        }

        public FigureRepresentation GetFigureRepresentation(Vector2Int position)
        {
            return figures.Where((figure) => figure.position.x == position.x && figure.position.y == position.y).FirstOrDefault();
        }
    }
}