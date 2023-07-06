using UnityEngine.UIElements;

namespace Chess.UI
{
    public static class VisualElementExtensions
    {
        public static void ShowVisualElement(this VisualElement visualElement)
        {
            visualElement.style.display = DisplayStyle.Flex;
        }

        public static void HideVisualElement(this VisualElement visualElement)
        {
            visualElement.style.display = DisplayStyle.None;
        }
    }
}
