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

        public static int GetFileNumber(char c)
        {
            Enum.TryParse<Files>(c.ToString().ToUpper(), out Files file);
            return (int)file;
        }

        public static string GetRankNotation(int y)
        {
            return (y + 1).ToString();
        }

        public static int GetRankNumber(char c)
        {
            return int.Parse(c.ToString()) - 1;
        }

        public static string GetPieceNotation(Type type)
        {
            if (type == typeof(Pawn)) return "";
            else if (type == typeof(Knight)) return "N";
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

        public static Type GetTypeFromNotation(char c)
        {
            if (c == 'N') return typeof(Knight);
            else if (c == 'B') return typeof(Bishop);
            else if (c == 'R') return typeof(Rook);
            else if (c == 'Q') return typeof(Queen);
            else if (c == 'K') return typeof(King);
            else return null;
        }

        public static int GetFigureValue(Figure figure)
        {
            if (figure.GetType() == typeof(Pawn)) return 1;
            if (figure.GetType() == typeof(Knight)) return 3;
            if (figure.GetType() == typeof(Bishop)) return 3;
            if (figure.GetType() == typeof(Rook)) return 5;
            if (figure.GetType() == typeof(Queen)) return 9;
            return 0;
        }

        public static int StandardNumberOfFigure(Type figureType)
        {
            if (figureType == typeof(Pawn)) return 8;
            if (figureType == typeof(Knight)) return 2;
            if (figureType == typeof(Bishop)) return 2;
            if (figureType == typeof(Rook)) return 2;
            if (figureType == typeof(Queen)) return 1;
            if (figureType == typeof(King)) return 1;
            return 0;
        }
    }
}
