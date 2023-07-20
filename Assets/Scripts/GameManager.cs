using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    /// <summary>
    /// Handles all setup that is required to start and render a game.
    /// </summary>
    public sealed class GameManager : MonoBehaviour
    {
        public delegate void GameStateChangedEventHandler(Match match, GameState state);
        public event GameStateChangedEventHandler OnGameStateChanged;

        public delegate void GameEndedEventHandler(GameResult gameResult);
        public event GameEndedEventHandler OnGameEndedEvent;

        public Game ActiveGame => game;
        private Game game;

        private void Awake()
        {
            game = new Game();
            game.MoveCompletedEvent += OnMoveCompleted;
            game.GameEndedEvent += OnGameEnded;
        }

        private void Start()
        {
            game.StartGame();
        }

        private void OnDestroy()
        {
            game.MoveCompletedEvent -= OnMoveCompleted;
            game.GameEndedEvent -= OnGameEnded;
        }

        private void OnMoveCompleted(List<Figure> figures)
        {
            OnGameStateChanged?.Invoke(ActiveGame.Match, ActiveGame.GameState);
        }

        private void OnGameEnded(GameResult result)
        {
            OnGameEndedEvent?.Invoke(result);
            game.GameEndedEvent -= OnGameEnded;
        }

        public void Restart()
        {
            game.MoveCompletedEvent -= OnMoveCompleted;
            game.GameEndedEvent -= OnGameEnded;

            game = new Game();
            game.MoveCompletedEvent += OnMoveCompleted;
            game.GameEndedEvent += OnGameEnded;
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