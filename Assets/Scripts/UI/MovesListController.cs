using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chess.UI
{
    public class MovesListController : MonoBehaviour
    {
        private const string movesListID = "moves-list-view";

        [SerializeField]
        private GameManager gameManager = default;
        [SerializeField]
        private UIDocument rootUIDocument = default;

        private ListView movesListView;
        private List<string> moves = new List<string>();

        private void OnEnable()
        {
            gameManager.OnGameStateChanged += OnGameStateChanged;

            movesListView = rootUIDocument.rootVisualElement.Q<ListView>(movesListID);

            Func<VisualElement> makeItem = () => new Label();
            movesListView.makeItem = makeItem;

            Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = moves[i];
            movesListView.bindItem = bindItem;

            movesListView.itemsSource = moves;
            movesListView.onSelectionChange += OnMoveSelected;
        }

        private void OnDisable()
        {
            gameManager.OnGameStateChanged -= OnGameStateChanged;
            movesListView.onSelectionChange -= OnMoveSelected;
        }

        private void OnGameStateChanged(Match match, GameState gameState)
        {
            moves.Clear();
            for (int i = 0; i < match.Moves.Count; i += 2)
            {
                string move = $"{(i / 2) + 1}. {match.Moves[i].Serialize()}";
                if (i + 1 < match.Moves.Count)
                {
                    move += " " + match.Moves[i + 1].Serialize();
                }
                moves.Add(move);
            }

            movesListView.Rebuild();
        }

        private void OnMoveSelected(IEnumerable<object> objects)
        {
            // TODO: Enable game to jump to a specific move
            Debug.Log(objects.First());
        }
    }
}
