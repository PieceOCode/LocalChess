using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chess.UI
{
    public class ScoreController : ControllerBase
    {
        [SerializeField]
        private GameManager gameManager = default;
        [SerializeField]
        private Color color;

        private const string scoreLabelName = "score__points-label";
        private const string figureContainerName = "score__figure-container";
        private const string figureImageClass = "score__figure_image";

        private Label scoreLabel = default;
        private VisualElement figureContainer = default;

        override protected void Awake()
        {
            base.Awake();
            gameManager.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            gameManager.OnGameStateChanged -= OnGameStateChanged;
        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            scoreLabel = rootElement.Q<Label>(scoreLabelName);
            figureContainer = rootElement.Q<VisualElement>(figureContainerName);
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

            foreach (FigureEnum figure in Enum.GetValues(typeof(FigureEnum)))
            {
                Type type = figure.FigureEnumToType();
                int fullCount = ChessNotation.StandardNumberOfFigure(type);

                int count = 0;
                Color enemyColor = this.color == Color.White ? Color.Black : Color.White;
                if (enemyColor == Color.White)
                {
                    count = gameState.WhitePieces.Where(f => f.GetType() == type).Count();
                }
                else
                {
                    count = gameState.BlackPieces.Where(f => f.GetType() == type).Count();
                }

                for (int i = 0; i < fullCount - count; i++)
                {
                    FigureElement figureElement = new FigureElement();
                    figureElement.AddToClassList(figureImageClass);
                    figureElement.SetFigure(figure, enemyColor);
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
