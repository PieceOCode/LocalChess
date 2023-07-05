using UnityEngine;
using UnityEngine.UIElements;

namespace Chess.UI
{
    public class FigureElement : VisualElement
    {
        public enum FigureType
        {
            Pawn,
            Knight,
            Bishop,
            Rook,
            Queen,
            King
        }

        public FigureType figure;
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
            UxmlEnumAttributeDescription<FigureType> figureAttribute = new UxmlEnumAttributeDescription<FigureType> { name = "figure" };
            UxmlEnumAttributeDescription<Color> colorAttribute = new UxmlEnumAttributeDescription<Color> { name = "color" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                FigureType figure = figureAttribute.GetValueFromBag(bag, cc);
                Color color = colorAttribute.GetValueFromBag(bag, cc);
                ((FigureElement)ve).SetFigure(figure, color);
            }
        }

        public void SetFigure(FigureType figure, Color color)
        {
            this.figure = figure;
            this.color = color;

            Sprite sprite = null;
            if (figure == FigureType.Pawn) sprite = Sprites.GetSprite(typeof(Pawn), color);
            if (figure == FigureType.Knight) sprite = Sprites.GetSprite(typeof(Knight), color);
            if (figure == FigureType.Bishop) sprite = Sprites.GetSprite(typeof(Bishop), color);
            if (figure == FigureType.Rook) sprite = Sprites.GetSprite(typeof(Rook), color);
            if (figure == FigureType.Queen) sprite = Sprites.GetSprite(typeof(Queen), color);
            if (figure == FigureType.King) sprite = Sprites.GetSprite(typeof(King), color);

            this.style.backgroundImage = new StyleBackground(sprite);
            this.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
        }
    }
}
