using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    /// <summary>
    /// Handles spawning and despawning figures from prefabs and registers their callbacks at the appropriate managers.
    /// </summary>
    public class FigureSpawner : MonoBehaviour
    {
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


        private List<FigureRepresentation> figures = new List<FigureRepresentation>();

        public void ClearRepresentations()
        {
            foreach (FigureRepresentation figure in figures)
            {
                Destroy(figure.gameObject);
            }
            figures.Clear();
        }

        public void CreateRepresentation(Figure figure)
        {
            FigureRepresentation figureRepresentation = Instantiate(GetPrefab(figure));
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
    }
}