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

        private void UpdateRepresentation(List<Figure> pieces)
        {
            spawnManager.ClearRepresentations();
            foreach (Figure figure in pieces)
            {
                spawnManager.CreateRepresentation(figure);
            }
        }

        public void Restart()
        {
            game.OnMoveCompletedEvent -= UpdateRepresentation;

            game = new Game();
            game.OnMoveCompletedEvent += UpdateRepresentation;
            game.StartGame();
        }

        // BUG: Only switch active player if undo was successful
        public void Undo()
        {
            game.Undo();
        }

        // BUG: Only switch active player if undo was successful
        public void Redo()
        {
            game.Redo();
        }

        public void LoadGame()
        {
            Restart();
            game.DeserializeMatch();
        }

        public void SaveGame()
        {
            MatchSerializer.SerializeMatch(game.Match, Application.persistentDataPath + "/match.pgn");
        }
    }
}