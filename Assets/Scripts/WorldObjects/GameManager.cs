using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    /// <summary>
    /// Handles all setup that is required to start and render a game.
    /// </summary>
    public sealed class GameManager : MonoBehaviour
    {
        public delegate void GameStateChanged(Match match, GameState state);
        public event GameStateChanged OnGameStateChanged;

        public Game ActiveGame => game;
        private Game game;

        private void Awake()
        {
            game = new Game();
            game.OnMoveCompletedEvent += OnMoveCompleted;
        }

        private void Start()
        {
            game.StartGame();
        }

        private void OnDestroy()
        {
            game.OnMoveCompletedEvent -= OnMoveCompleted;
        }

        private void OnMoveCompleted(List<Figure> figures)
        {
            OnGameStateChanged?.Invoke(ActiveGame.Match, ActiveGame.GameState);
        }

        public void Restart()
        {
            game.OnMoveCompletedEvent -= OnMoveCompleted;

            game = new Game();
            game.OnMoveCompletedEvent += OnMoveCompleted;
            game.StartGame();
        }

        public void Undo()
        {
            game.Undo();
        }

        public void Redo()
        {
            game.Redo();
        }

        public void LoadGame()
        {
            Restart();
            game.DeserializeMatch();
            game.StartGame();
        }

        public void SaveGame()
        {
            MatchSerializer.SerializeMatch(game.Match, Application.persistentDataPath + "/match.pgn");
        }
    }
}