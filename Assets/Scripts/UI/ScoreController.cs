using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chess.UI
{
    public class ScoreController : MonoBehaviour
    {
        [SerializeField]
        private string scoreName = default;
        [SerializeField]
        UIDocument rootDocument = default;
        [SerializeField]
        private GameManager gameManager = default;
        [SerializeField]
        private Color color;

        private const string scoreLabelName = "score__points-label";
        private const string figureContainerName = "score__figure-container";
        private const string figureImageClass = "score__figure_image";
        private Label scoreLabel = default;
        private VisualElement figureContainer = default;

        private void Awake()
        {
            var rootElement = rootDocument.rootVisualElement.Q(scoreName);
            scoreLabel = rootElement.Q<Label>(scoreLabelName);
            figureContainer = rootElement.Q<VisualElement>(figureContainerName);

            gameManager.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            gameManager.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(Match match, GameState gameState)
        {
            UpdateScore(gameState);
            UpdateDeadFigures(gameState);
        }

        private void UpdateScore(GameState gameState)
        {
            int score = SumUpFigures(gameState.WhitePieces) - SumUpFigures(gameState.BlackPieces);
            if (color == Color.Black)
            {
                score *= -1;
            }
            scoreLabel.text = score.ToString();
        }

        private void UpdateDeadFigures(GameState gameState)
        {
            figureContainer.Clear();
            foreach (Figure figure in gameState.DeadPieces.OrderBy(f => ChessNotation.GetFigureValue(f)))
            {
                if (figure.Color != color)
                {
                    FigureElement figureElement = new FigureElement();
                    figureElement.AddToClassList(figureImageClass);
                    figureElement.SetFigure(figure);
                    figureContainer.Add(figureElement);
                }
            }
        }

        private static int SumUpFigures(List<Figure> figures)
        {
            int score = 0;
            foreach (Figure f in figures)
            {
                score += ChessNotation.GetFigureValue(f);
            }
            return score;
        }
    }
}
