using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chess.UI
{
    public class Score : MonoBehaviour
    {
        [SerializeField]
        VisualTreeAsset figureElementTemplate = default;
        [SerializeField]
        private string scoreName = default;
        [SerializeField]
        UIDocument rootDocument = default;
        [SerializeField]
        private GameManager gameManager = default;
        [SerializeField]
        private Color color;

        [Header("Figure Sprites")]
        [SerializeField]
        private Sprite pawnSprite = default;
        [SerializeField]
        private Sprite knightSprite = default;
        [SerializeField]
        private Sprite bishopSprite = default;
        [SerializeField]
        private Sprite rookSprite = default;
        [SerializeField]
        private Sprite queenSprite = default;

        private const string scoreLabelName = "score__points-label";
        private const string figureContainerName = "score__figure-container";
        private const string figureImageName = "figure_element-image";
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
                    Sprite sprite = pawnSprite;
                    if (figure.GetType() == typeof(Knight)) sprite = knightSprite;
                    else if (figure.GetType() == typeof(Bishop)) sprite = bishopSprite;
                    else if (figure.GetType() == typeof(Rook)) sprite = rookSprite;
                    else if (figure.GetType() == typeof(Queen)) sprite = queenSprite;

                    var figureElement = figureElementTemplate.CloneTree();
                    VisualElement image = figureElement.Q<VisualElement>(figureImageName);
                    image.style.backgroundImage = new StyleBackground(sprite);
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
