using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chess.UI
{
    public class PawnPromotionController : MonoBehaviour
    {
        private const string pawnPromotionID = "pawn-promotion";

        public event Action<Type> FigureChosenEvent;

        [SerializeField]
        private UIDocument rootUIDocument = default;

        private VisualElement root = default;

        private List<FigureElement> figureElements = new List<FigureElement>();

        private void OnEnable()
        {
            root = rootUIDocument.rootVisualElement.Q(pawnPromotionID);

            figureElements = root.Query<FigureElement>().ToList();

            foreach (FigureElement el in figureElements)
            {
                el.RegisterCallback<ClickEvent>(OnFigureClicked);
            }
        }

        public void Show(Color color)
        {
            foreach (FigureElement el in figureElements)
            {
                el.SetFigure(el.figure, color);
            }

            root.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            root.style.display = DisplayStyle.None;
        }

        private void OnFigureClicked(ClickEvent evt)
        {
            FigureElement element = (FigureElement)evt.target;
            Type figureType = element.figure.FigureEnumToType();
            FigureChosenEvent.Invoke(figureType);
            FigureChosenEvent = null;
            this.Hide();
        }
    }
}
