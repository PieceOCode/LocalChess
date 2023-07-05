using UnityEngine.UIElements;

namespace Chess.UI
{
    // VisualElement that's width always equals its height.
    public class SquarePlaceholder : VisualElement
    {
        public SquarePlaceholder()
        {
            this.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            this.style.flexShrink = 0;
        }

        public new class UxmlFactory : UxmlFactory<SquarePlaceholder> { }

        public void OnGeometryChanged(GeometryChangedEvent evt)
        {
            this.style.width = this.resolvedStyle.height;
        }

        ~SquarePlaceholder()
        {
            this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }
    }
}
