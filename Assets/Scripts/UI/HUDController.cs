using UnityEngine;
using UnityEngine.UIElements;

namespace Chess.UI
{
    public class HUDController : ControllerBase
    {
        [SerializeField]
        private GameManager gameManager = default;

        private Button restartButton = default;
        private Button undoButton = default;
        private Button redoButton = default;
        private Button saveButton = default;
        private Button loadButton = default;

        private const string restartButtonID = "restart-button";
        private const string undoButtonID = "undo-button";
        private const string redoButtonID = "redo-button";
        private const string saveButtonID = "save-button";
        private const string loadButtonID = "load-button";

        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            restartButton = rootElement.Q<Button>(restartButtonID);
            undoButton = rootElement.Q<Button>(undoButtonID);
            redoButton = rootElement.Q<Button>(redoButtonID);
            saveButton = rootElement.Q<Button>(saveButtonID);
            loadButton = rootElement.Q<Button>(loadButtonID);
        }

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();
            restartButton.RegisterCallback<ClickEvent>(OnRestartClicked);
            undoButton.RegisterCallback<ClickEvent>(OnUndoClicked);
            redoButton.RegisterCallback<ClickEvent>(OnRedoClicked);
            saveButton.RegisterCallback<ClickEvent>(OnSaveClicked);
            loadButton.RegisterCallback<ClickEvent>(OnLoadClicked);
        }

        protected override void UnregisterCallbacks()
        {
            base.UnregisterCallbacks();
            restartButton.UnregisterCallback<ClickEvent>(OnRestartClicked);
            undoButton.UnregisterCallback<ClickEvent>(OnUndoClicked);
            redoButton.UnregisterCallback<ClickEvent>(OnRedoClicked);
            saveButton.UnregisterCallback<ClickEvent>(OnSaveClicked);
            loadButton.UnregisterCallback<ClickEvent>(OnLoadClicked);
        }

        private void OnRestartClicked(ClickEvent e)
        {
            Debug.Log("Restart");
            gameManager.Restart();
        }

        private void OnRedoClicked(ClickEvent e)
        {
            gameManager.Redo();
        }

        private void OnUndoClicked(ClickEvent e)
        {
            gameManager.Undo();
        }

        private void OnSaveClicked(ClickEvent e)
        {
            gameManager.SaveGame();
        }

        private void OnLoadClicked(ClickEvent e)
        {
            gameManager.LoadGame();
        }
    }
}
