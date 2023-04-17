using System;
using UnityEngine;

namespace Chess
{
    public readonly struct FigureData
    {
        public readonly Type type;
        public readonly Color color;
        public readonly Vector2Int position;
        public readonly bool hasMoved;

        public FigureData(Figure figure)
        {
            type = figure.GetType();
            color = figure.Color;
            position = figure.Position;
            hasMoved = figure.HasMoved;
        }

        public static Figure CreateFigureFrom(FigureData data, GameState gameState)
        {
            if (data.type == typeof(Pawn)) { return new Pawn(data.position, data.color, gameState); }
            else if (data.type == typeof(Bishop)) { return new Bishop(data.position, data.color, gameState); }
            else if (data.type == typeof(Knight)) { return new Knight(data.position, data.color, gameState); }
            else if (data.type == typeof(Rook)) { return new Rook(data.position, data.color, gameState); }
            else if (data.type == typeof(Queen)) { return new Queen(data.position, data.color, gameState); }
            else if (data.type == typeof(King)) { return new King(data.position, data.color, gameState); }
            else return null;
        }
    }
}
