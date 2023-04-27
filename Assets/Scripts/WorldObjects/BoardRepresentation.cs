using TMPro;
using UnityEngine;

namespace Chess
{
    public class BoardRepresentation : MonoBehaviour
    {
        [SerializeField]
        GameManager gameManager;
        [SerializeField]
        InputManager inputManager = default;

        [SerializeField]
        private UnityEngine.Color brightColor = UnityEngine.Color.white;
        [SerializeField]
        private UnityEngine.Color darkColor = UnityEngine.Color.black;

        [SerializeField]
        private SquareRepresentation squarePrefab = default;
        [SerializeField]
        private Transform squareContainer = default;
        [SerializeField]
        private TMP_Text fileUIPrefab = default;
        [SerializeField]
        private TMP_Text rankUIPrefab = default;

        public int Width => width;
        public int Height => height;

        private int width = default;
        private int height = default;

        public Vector3 GetWorldPosition(Vector2Int position)
        {
            return new Vector3(position.x, position.y, transform.position.z);
        }

        public void SpawnRepresentation(int width, int height)
        {
            this.width = width;
            this.height = height;
            SpawnSquares();
            SpawnRankAndFileTexts();
        }

        private void SpawnSquares()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    SquareRepresentation square = Instantiate(squarePrefab, squareContainer);

                    Vector2Int pos = new Vector2Int(x, y);
                    Color color = (Color)((x + y + 1) % 2);
                    UnityEngine.Color squareColor = color == Color.White ? brightColor : darkColor;

                    square.SetSquare(pos, squareColor);
                    inputManager.RegisterSquare(square);

                    square.transform.position = GetWorldPosition(pos);
                }
            }
        }

        private void SpawnRankAndFileTexts()
        {
            for (int x = 0; x < width; x++)
            {
                TMP_Text fileUI = Instantiate(fileUIPrefab, transform);
                fileUI.text = ChessNotation.GetFileNotation(x);

                Vector2Int pos = new Vector2Int(x, 0);
                fileUI.transform.position = GetWorldPosition(pos) + new Vector3(0.5f, -0.5f, 0);
                Color color = (Color)((x + 1) % 2);
                fileUI.color = color == Color.White ? darkColor : brightColor;
            }

            for (int y = 0; y < height; y++)
            {
                TMP_Text rankUI = Instantiate(rankUIPrefab);
                rankUI.text = ChessNotation.GetRankNotation(y);

                Vector2Int pos = new Vector2Int(0, y);
                rankUI.transform.position = GetWorldPosition(pos) + new Vector3(-0.5f, 0.5f, 0);
                Color color = (Color)((y + 1) % 2);
                rankUI.color = color == Color.White ? darkColor : brightColor;
            }
        }
    }
}
