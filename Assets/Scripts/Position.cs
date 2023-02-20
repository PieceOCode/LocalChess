using UnityEngine;

namespace Chess
{
    public enum Files
    {
        A, B, C, D, E, F, G, H
    }

    public static class ChessVectorExtensions
    {
        public static string ToString(this Vector2Int pos)
        {
            return $"{(Files)pos.x}{pos.y + 1}";
        }

        public static bool IsValid(this Vector2Int pos)
        {
            return (pos.x >= 0 && pos.x <= 7) && (pos.y >= 0 && pos.y <= 7);
        }

        // Manhattan distance between squares
        public static float Distance(this Vector2Int pos, Vector2Int otherPos)
        {
            return Mathf.Abs(pos.x - otherPos.x) + Mathf.Abs(pos.y - otherPos.y);
        }
    }
}
