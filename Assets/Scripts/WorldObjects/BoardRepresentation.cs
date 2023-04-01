using TMPro;
using UnityEngine;

namespace Chess
{
    public class BoardRepresentation : MonoBehaviour
    {
        [SerializeField]
        Board board = default;
        [SerializeField]
        private TMP_Text fileUIPrefab = default;
        [SerializeField]
        private TMP_Text rankUIPrefab = default;

        // Start is called before the first frame update
        void Start()
        {
            for (int x = 0; x < board.Width; x++)
            {
                TMP_Text fileUI = Instantiate(fileUIPrefab, transform);
                fileUI.text = ((Files)x).ToString();

                Vector2Int pos = new Vector2Int(x, 0);
                fileUI.transform.position = board.GetWorldPosition(pos) + new Vector3(0.5f, -0.5f, 0);
                //fileUI.color = this.color == Color.White ? darkColor : brightColor;
            }


            for (int y = 0; y < board.Height; y++)
            {
                TMP_Text rankUI = Instantiate(rankUIPrefab);
                rankUI.text = (y + 1).ToString();

                Vector2Int pos = new Vector2Int(0, y);
                rankUI.transform.position = board.GetWorldPosition(pos) + new Vector3(-0.5f, 0.5f, 0);
                //rankUI.color = this.color == Color.White ? darkColor : brightColor;
            }
        }
    }
}
