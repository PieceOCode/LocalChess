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
    }
}
