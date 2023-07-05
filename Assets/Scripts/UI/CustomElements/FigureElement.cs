using UnityEngine;
using UnityEngine.UIElements;

namespace Chess.UI
{
    public class FigureElement : VisualElement
    {
        public FigureEnum figure;
        public Color color;

        private FigureSpritesSO sprites;
        private FigureSpritesSO Sprites
        {
            get
            {
                if (sprites == null)
                {
                    sprites = Resources.Load<FigureSpritesSO>("Data/FigureSprites");
                }
                return sprites;
            }
        }

        public new class UxmlFactory : UxmlFactory<FigureElement, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlEnumAttributeDescription<FigureEnum> figureAttribute = new UxmlEnumAttributeDescription<FigureEnum> { name = "figure" };
            UxmlEnumAttributeDescription<Color> colorAttribute = new UxmlEnumAttributeDescription<Color> { name = "color" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                FigureEnum figure = figureAttribute.GetValueFromBag(bag, cc);
                Color color = colorAttribute.GetValueFromBag(bag, cc);
                ((FigureElement)ve).SetFigure(figure, color);
            }
        }

        public void SetFigure(FigureEnum figure, Color color)
        {
            this.figure = figure;
            this.color = color;
            Sprite sprite = Sprites.GetSprite(figure.FigureEnumToType(), color);
            this.style.backgroundImage = new StyleBackground(sprite);
            this.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
        }

        public void SetFigure(Figure figure)
        {
            SetFigure(FigureEnumExtensions.FigureToEnum(figure), figure.Color);
        }
    }
}
