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
        private UIDocument rootDocument;

        protected VisualElement rootElement = default;

        protected virtual void Awake()
        {
            if (rootDocument == null)
            {
                rootDocument = GetComponentInChildren<UIDocument>();
            }

            if (rootDocument == null)
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
            rootElement = rootDocument.rootVisualElement.Q(screenID);
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
            rootElement.ShowVisualElement();
        }

        public virtual void Hide()
        {
            rootElement.HideVisualElement();
        }
    }
}

