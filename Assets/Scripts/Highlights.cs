using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Highlights : MonoBehaviour
    {
        [SerializeField]
        private GameObject highlightPrefab = default;
        [SerializeField]
        private BoardRepresentation board = default;

        private List<GameObject> highlightList = new List<GameObject>();

        public void HighlightSquares(List<Vector2Int> squarePositions)
        {
            ClearHighlights();
            for (int i = 0; i < squarePositions.Count; i++)
            {
                if (i >= highlightList.Count)
                {
                    GameObject highlight = Instantiate(highlightPrefab, transform);
                    highlightList.Add(highlight);
                }

                highlightList[i].SetActive(true);
                highlightList[i].transform.position = board.GetWorldPosition(squarePositions[i]);
            }
        }

        public void ClearHighlights()
        {
            foreach (var highlight in highlightList)
            {
                highlight.gameObject.SetActive(false);
            }
        }
    }
}
