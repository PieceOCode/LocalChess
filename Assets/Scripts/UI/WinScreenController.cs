using UnityEngine;
using UnityEngine.UIElements;

namespace Chess.UI
{
    public class WinScreenController : ControllerBase
    {
        private const string labelID = "win-screen__label";

        private Label label = null;

        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            label = rootElement.Q<Label>(labelID);
        }

        public void SetState(GameResult result)
        {
            switch (result)
            {
                case GameResult.WhiteWins:
                    label.text = "White wins!";
                    break;
                case GameResult.BlackWins:
                    label.text = "Black wins!";
                    break;
                case GameResult.Draw:
                    label.text = "Draw!";
                    break;
            }
            Debug.Log("The game ended with result: " + result.ToString());
        }
    }
}
