using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Chess.UI
{
    public class WindowController : MonoBehaviour
    {
        [SerializeField]
        private GameManager gameManager = default;
        [SerializeField]
        private UIDocument rootDocument = default;
        [Header("Window Controller")]
        [SerializeField]
        private OverlayController windowOverlay = default;
        [SerializeField]
        private PawnPromotionController pawnPromotion = default;
        [SerializeField]
        private WinScreenController winScreen = default;

        public PawnPromotionController PawnPromotion => pawnPromotion;

        private ControllerBase currentActive = null;


        private void OnEnable()
        {
            gameManager.OnGameEndedEvent += ShowWinscreen;
            windowOverlay.OverlayClickedEvent += HideActive;

            pawnPromotion.ShowEvent += OnWindowShowed;
            pawnPromotion.HideEvent += OnWindowHidden;
            winScreen.ShowEvent += OnWindowShowed;
            winScreen.HideEvent += OnWindowHidden;
        }

        private void OnDestroy()
        {
            gameManager.OnGameEndedEvent -= ShowWinscreen;
            windowOverlay.OverlayClickedEvent -= HideActive;

            pawnPromotion.ShowEvent -= OnWindowShowed;
            pawnPromotion.HideEvent -= OnWindowHidden;
            winScreen.ShowEvent -= OnWindowShowed;
            winScreen.HideEvent -= OnWindowHidden;
        }

        private void OnWindowShowed()
        {
            windowOverlay.Show();
        }

        private void OnWindowHidden()
        {
            windowOverlay.Hide();
        }

        private void HideActive()
        {
            Assert.IsTrue(currentActive != null, "Hiding a window should only happen on callback and therefore be impossible if no window is open.");
            currentActive.Hide();
            currentActive = null;
        }

        public void ShowPawnPromotion(Color color, Action<Type> callback)
        {
            Assert.IsTrue(currentActive == null);
            pawnPromotion.Show();
            pawnPromotion.SetColor(color);
            pawnPromotion.FigureChosenEvent += callback;
            currentActive = pawnPromotion;
        }

        public void ShowWinscreen(GameResult result)
        {
            Assert.IsTrue(currentActive == null);
            winScreen.Show();
            winScreen.SetState(result);
            currentActive = winScreen;
        }
    }
}
