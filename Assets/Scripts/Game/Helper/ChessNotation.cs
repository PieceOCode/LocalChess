using System;
using UnityEngine;

namespace Chess
{
    public static class ChessNotation
    {
        public enum Files
        {
            A, B, C, D, E, F, G, H
        }

        public static string ToChessNotation(this Vector2Int pos)
        {
            return $"{(Files)pos.x}{pos.y + 1}".ToLower();
        }

        public static string GetSquareNotation(Vector2Int pos)
        {
            return pos.ToChessNotation();
        }

        public static string GetFileNotation(int x)
        {
            return ((Files)x).ToString().ToLower();
        }

        public static string GetRankNotation(int y)
        {
            return (y + 1).ToString();
        }

        public static string GetPieceNotation(Type type)
        {
            if (type == typeof(Pawn)) return "";
            else if (type == typeof(Knight)) return "K";
            else if (type == typeof(Bishop)) return "B";
            else if (type == typeof(Rook)) return "R";
            else if (type == typeof(Queen)) return "Q";
            else if (type == typeof(King)) return "K";
            else
            {
                Debug.LogError("There should not be another type of figure");
                return "";
            }
        }
    }
}
