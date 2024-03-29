using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Chess
{
    public static class ChessVectorExtensions
    {
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

        public static bool IsBetween(this Vector2Int pos, Vector2Int start, Vector2Int end)
        {
            Assert.IsTrue(start != end, "The start and end position arguments cannot be the same.");
            Vector2 line = end - start;
            Vector2 unitDirection = line / (float)start.ChebyshevDistance(end);
            int distance = start.ChebyshevDistance(pos);
            return Vector2.Distance(pos, (start + distance * unitDirection)) <= Vector2.kEpsilon;
        }

        public static List<Vector2Int> GetPositionsBetween(this Vector2Int pos, Vector2Int otherPos)
        {
            Assert.IsTrue(pos.IsOnSameLine(otherPos), "Function call is invalid because pos and otherPos are not on the same line.");
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
