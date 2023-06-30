using System;
using UnityEngine;

namespace Chess.UI
{
    [CreateAssetMenu(fileName = "FigureSprites", menuName = "ScriptableObjects/FigureSprites", order = 1)]
    public class FigureSpritesSO : ScriptableObject
    {
        [Header("White Figures")]
        public Sprite whitePawn;
        public Sprite whiteKnight;
        public Sprite whiteBishop;
        public Sprite whiteRook;
        public Sprite whiteQueen;
        public Sprite whiteKing;

        [Header("Black Figures")]
        public Sprite blackPawn;
        public Sprite blackKnight;
        public Sprite blackBishop;
        public Sprite blackRook;
        public Sprite blackQueen;
        public Sprite blackKing;

        public Sprite GetSprite(Type t, Color color)
        {
            if (t == typeof(Pawn)) return color == Color.White ? whitePawn : blackPawn;
            else if (t == typeof(Knight)) return color == Color.White ? whiteKnight : blackKnight;
            else if (t == typeof(Bishop)) return color == Color.White ? whiteBishop : blackBishop;
            else if (t == typeof(Rook)) return color == Color.White ? whiteRook : blackRook;
            else if (t == typeof(Queen)) return color == Color.White ? whiteQueen : blackQueen;
            else if (t == typeof(King)) return color == Color.White ? whiteKing : blackKing;
            return null;
        }

        public Sprite GetSprite(Figure figure)
        {
            return GetSprite(figure.GetType(), figure.Color);
        }
    }
}
