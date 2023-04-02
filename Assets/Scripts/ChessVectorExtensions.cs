using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Chess
{
    public enum Files
    {
        A, B, C, D, E, F, G, H
    }

    // TODO: Write unit tests for extension methods
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

        public static float ManhattanDistance(this Vector2Int pos, Vector2Int otherPos)
        {
            return Mathf.Abs(pos.x - otherPos.x) + Mathf.Abs(pos.y - otherPos.y);
        }

        public static int ChebyshevDistance(this Vector2Int pos, Vector2Int otherPos)
        {
            Vector2Int direction = pos - otherPos;
            return Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
        }

        public static bool IsOnSameLine(this Vector2Int pos, Vector2Int otherPos)
        {
            Vector2Int d = pos - otherPos;
            return d.x == 0 || d.y == 0 || Mathf.Abs(d.x) == Mathf.Abs(d.y);
        }

        public static List<Vector2Int> GetPositionsBetween(this Vector2Int pos, Vector2Int otherPos)
        {
            Assert.IsTrue(pos.IsOnSameLine(otherPos));
            List<Vector2Int> positionsBetween = new List<Vector2Int>();

            int distance = pos.ChebyshevDistance(otherPos);
            Vector2Int direction = otherPos - pos;
            for (int i = 1; i < distance; i++)
            {
                positionsBetween.Add(pos + ((direction / distance) * i));
            }

            return positionsBetween;
        }
    }
}