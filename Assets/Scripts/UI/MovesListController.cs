using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chess.UI
{
    public class MovesListController : ControllerBase
    {
        [SerializeField]
        private GameManager gameManager = default;

        private ListView movesListView;
        private List<Tuple<Move, Move>> moves = new List<Tuple<Move, Move>>();

        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            movesListView = rootElement.Q<ListView>();

            Func<VisualElement> makeItem = () =>
            {
                var el = new MoveElement();
                el.OnMoveSelectedEvent += OnMoveSelected;
                return el;
            };
            movesListView.makeItem = makeItem;

            movesListView.destroyItem = (e) => (e as MoveElement).OnMoveSelectedEvent -= OnMoveSelected;

            Action<VisualElement, int> bindItem = (e, i) => (e as MoveElement).Init(i + 1, moves[i].Item1, moves[i].Item2);
            movesListView.bindItem = bindItem;

            movesListView.itemsSource = moves;
        }

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();
            gameManager.OnGameStateChanged += OnGameStateChanged;
        }

        protected override void UnregisterCallbacks()
        {
            base.UnregisterCallbacks();
            gameManager.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnMoveSelected(Move move)
        {
            gameManager.ActiveGame.JumpToMove(move);
        }

        private void OnGameStateChanged(Match match, GameState gameState)
        {
            moves.Clear();
            for (int i = 0; i < match.Moves.Count; i += 2)
            {
                if (i + 1 >= match.Moves.Count)
                {
                    moves.Add(new Tuple<Move, Move>(match.Moves[i], null));
                }
                else
                {
                    moves.Add(new Tuple<Move, Move>(match.Moves[i], match.Moves[i + 1]));
                }
            }

            movesListView.Rebuild();
        }
    }
}
