using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Chess.UI
{
    /// <summary>
    /// Base class for the controllers for all major UI elements like windows, screens, panels and HUD elements. 
    /// Provides access to basic functionality like the root visual element, show/hide and override-able setup functions.
    /// </summary>
    public abstract class ControllerBase : MonoBehaviour
    {
        [Tooltip("String ID from the UXML for the controlled panel/screen.")]
        [SerializeField]
        protected string screenID = default;
        [Tooltip("Set the UI Document here explicitly (or get automatically from current GameObject).")]
        [SerializeField]
        private UIManager uiManager = default;

        public event Action ShowEvent = default;
        public event Action HideEvent = default;

        protected VisualElement rootElement = default;

        protected virtual void Awake()
        {
            if (uiManager == null)
            {
                uiManager = GetComponentInParent<UIManager>();
            }

            if (uiManager == null)
            {
                Debug.LogWarning($"Controller {gameObject.name} has no assigned UI Document.");
            }
            else
            {
                SetVisualElements();
                RegisterCallbacks();
            }
        }

        private void OnDestroy()
        {
            UnregisterCallbacks();
        }

        protected virtual void SetVisualElements()
        {
            rootElement = uiManager.rootDocument.rootVisualElement.Q(screenID);
            Assert.IsTrue(rootElement != null, $"The controller {gameObject.name} is not able to find the root visual element with id {screenID}.");
        }

        protected virtual void RegisterCallbacks()
        {

        }

        protected virtual void UnregisterCallbacks()
        {

        }

        public virtual void Show()
        {
            ShowEvent?.Invoke();
            rootElement.ShowVisualElement();
        }

        public virtual void Hide()
        {
            HideEvent?.Invoke();
            rootElement.HideVisualElement();
        }
    }
}

