using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chess.UI
{
    public class MoveElement : VisualElement
    {
        public event Action<Move> OnMoveSelectedEvent;

        private int index = default;
        private Move firstMove = default;
        private Move secondMove = default;

        private Label numberLabel = default;
        private Label firstMoveLabel = default;
        private Label secondMoveLabel = default;

        private const string numberLabelID = "move-element__index-label";
        private const string firstMoveLabelID = "move-element__first-label";
        private const string secondMoveLabelID = "move-element__second-label";

        public MoveElement()
        {
            var asset = Resources.Load<VisualTreeAsset>("VisualElements/MoveElement");
            asset.CloneTree(this);

            numberLabel = this.Q<Label>(numberLabelID);
            firstMoveLabel = this.Q<Label>(firstMoveLabelID);
            secondMoveLabel = this.Q<Label>(secondMoveLabelID);

            firstMoveLabel.RegisterCallback<ClickEvent>(OnMoveClicked);
            secondMoveLabel.RegisterCallback<ClickEvent>(OnMoveClicked);
        }

        public new class UxmlFactory : UxmlFactory<MoveElement> { }

        public void Init(int index, Move firstMove, Move secondMove)
        {
            this.index = index;
            this.firstMove = firstMove;
            this.secondMove = secondMove;

            numberLabel.text = $"{this.index}.";
            firstMoveLabel.text = firstMove.ToString();
            secondMoveLabel.text = secondMove != null ? secondMove.ToString() : "";
        }

        private void OnMoveClicked(ClickEvent evt)
        {
            if (evt.target == firstMoveLabel)
            {
                Debug.Log(firstMove.ToString());
                OnMoveSelectedEvent?.Invoke(firstMove);
            }
            else
            {
                Debug.Log(secondMove.ToString());
                OnMoveSelectedEvent?.Invoke(secondMove);
            }
        }
    }
}
