using System;
using UnityEngine.UIElements;

namespace Chess.UI
{
    public class OverlayController : ControllerBase
    {
        public event Action OverlayClickedEvent;

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();
            rootElement.RegisterCallback<ClickEvent>(OnClick);
        }

        protected override void UnregisterCallbacks()
        {
            rootElement.UnregisterCallback<ClickEvent>(OnClick);
        }

        private void OnClick(ClickEvent evt)
        {
            OverlayClickedEvent?.Invoke();
        }
    }
}
