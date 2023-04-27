using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    /// <summary>
    /// Handles all setup that is required to start and render a game.
    /// </summary>
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField]
        private FigureSpawner spawnManager = default;
        [SerializeField]
        private BoardRepresentation boardRepresentation = default;

        public Game ActiveGame => game;
        private Game game;

        private void Awake()
        {
            game = new Game();
            game.OnMoveCompletedEvent += UpdateRepresentation;
            game.StartGame();

            boardRepresentation.SpawnRepresentation(game.Board.Width, game.Board.Height);
        }

        // FEATURE: Implement proper debug menu
        private void Update()
        {
            // BUG: Only switch active player if undo was successful
            if (Input.GetKeyDown(KeyCode.Q))
            {
                game.Undo();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                game.Redo();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                game.SerializeMatch();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                game.DeserializeMatch();
            }
        }

        private void UpdateRepresentation(List<Figure> pieces)
        {
            spawnManager.ClearRepresentations();
            foreach (Figure figure in pieces)
            {
                spawnManager.CreateRepresentation(figure);
            }
        }
    }
}