using UnityEngine;
using UnityEngine.UIElements;

namespace Chess.UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField]
        private GameManager gameManager = default;

        private Button restartButton = default;
        private Button undoButton = default;
        private Button redoButton = default;
        private Button saveButton = default;
        private Button loadButton = default;

        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();

            restartButton = uiDocument.rootVisualElement.Q<Button>("restart-button");
            undoButton = uiDocument.rootVisualElement.Q<Button>("undo-button");
            redoButton = uiDocument.rootVisualElement.Q<Button>("redo-button");
            saveButton = uiDocument.rootVisualElement.Q<Button>("save-button");
            loadButton = uiDocument.rootVisualElement.Q<Button>("load-button");

            restartButton.RegisterCallback<ClickEvent>(OnRestartClicked);
            undoButton.RegisterCallback<ClickEvent>(OnUndoClicked);
            redoButton.RegisterCallback<ClickEvent>(OnRedoClicked);
            saveButton.RegisterCallback<ClickEvent>(OnSaveClicked);
            loadButton.RegisterCallback<ClickEvent>(OnLoadClicked);
        }

        private void OnDisable()
        {
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
