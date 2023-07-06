using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Chess.UI
{
    public class PawnPromotionController : ControllerBase
    {
        public event Action<Type> FigureChosenEvent;
        private List<FigureElement> figureElements = new List<FigureElement>();

        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            figureElements = rootElement.Query<FigureElement>().ToList();
        }

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();
            foreach (FigureElement el in figureElements)
            {
                el.RegisterCallback<ClickEvent>(OnFigureClicked);
            }
        }

        protected override void UnregisterCallbacks()
        {
            base.UnregisterCallbacks();
            foreach (FigureElement el in figureElements)
            {
                el.UnregisterCallback<ClickEvent>(OnFigureClicked);
            }
        }

        public void SetColor(Color color)
        {
            foreach (FigureElement el in figureElements)
            {
                el.SetFigure(el.figure, color);
            }
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
