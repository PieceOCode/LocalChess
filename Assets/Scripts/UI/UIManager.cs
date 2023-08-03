using UnityEngine;
using UnityEngine.UIElements;

namespace Chess.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private UIDocument HUDDocument = default;
        [SerializeField]
        private VisualTreeAsset defaultHUD = default;
        [SerializeField]
        private PanelSettings defaultPanelSettings = default;
        [SerializeField]
        private VisualTreeAsset portraitHUD = default;
        [SerializeField]
        private PanelSettings portraitPanelSettings = default;

        private bool isInitiated = false;
        private bool isPortrait => Screen.height >= Screen.width;

        public UIDocument rootDocument
        {
            get
            {
                if (!isInitiated)
                {
                    InitiateByOrientation();
                }
                return HUDDocument;
            }
        }

        private void InitiateByOrientation()
        {
            if (isPortrait)
            {
                HUDDocument.visualTreeAsset = portraitHUD;
                HUDDocument.panelSettings = portraitPanelSettings;
            }
            else
            {
                HUDDocument.visualTreeAsset = defaultHUD;
                HUDDocument.panelSettings = defaultPanelSettings;
            }

            FixScreenOrientationOnStartup();
            isInitiated = true;
        }

        private void FixScreenOrientationOnStartup()
        {
            Screen.autorotateToPortrait = isPortrait;
            Screen.autorotateToPortraitUpsideDown = isPortrait;
            Screen.autorotateToLandscapeLeft = !isPortrait;
            Screen.autorotateToLandscapeRight = !isPortrait;
        }
    }
}
