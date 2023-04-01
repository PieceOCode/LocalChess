using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Chess
{
    public class BoardRepresentation : MonoBehaviour
    {
        [SerializeField]
        GameManager gameManager;
        [SerializeField]
        InputManager inputManager = default;

        [SerializeField]
        private Square squarePrefab = default;
        [SerializeField]
        private Transform squareContainer = default;
        [SerializeField]
        private TMP_Text fileUIPrefab = default;
        [SerializeField]
        private TMP_Text rankUIPrefab = default;

        public int Width => board.Width;
        public int Height => board.Height;
        private Board board;

        public Vector3 GetWorldPosition(Vector2Int position)
        {
            Assert.IsTrue(position.IsValid());
            return new Vector3(position.x, position.y, transform.position.z);
        }

        void Start()
        {
            board = gameManager.Board;
            SpawnSquares();
            SpawnRankAndFileTexts();
        }

        private void SpawnSquares()
        {
            for (int x = 0; x < board.Width; x++)
            {
                for (int y = 0; y < board.Height; y++)
                {
                    Square square = Instantiate(squarePrefab, squareContainer);

                    Vector2Int pos = new Vector2Int(x, y);
                    Color color = (Color)((x + y + 1) % 2);

                    square.SetSquare(pos, color);
                    inputManager.RegisterSquare(square);

                    square.transform.position = GetWorldPosition(pos);
                }
            }
        }

        private void SpawnRankAndFileTexts()
        {
            for (int x = 0; x < board.Width; x++)
            {
                TMP_Text fileUI = Instantiate(fileUIPrefab, transform);
                fileUI.text = ((Files)x).ToString();

                Vector2Int pos = new Vector2Int(x, 0);
                fileUI.transform.position = GetWorldPosition(pos) + new Vector3(0.5f, -0.5f, 0);
                //fileUI.color = this.color == Color.White ? darkColor : brightColor;
            }

            for (int y = 0; y < board.Height; y++)
            {
                TMP_Text rankUI = Instantiate(rankUIPrefab);
                rankUI.text = (y + 1).ToString();

                Vector2Int pos = new Vector2Int(0, y);
                rankUI.transform.position = GetWorldPosition(pos) + new Vector3(-0.5f, 0.5f, 0);
                //rankUI.color = this.color == Color.White ? darkColor : brightColor;
            }
        }
    }
}
