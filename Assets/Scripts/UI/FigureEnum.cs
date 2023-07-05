using System;

namespace Chess.UI
{
    public enum FigureEnum
    {
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    public static class FigureEnumExtensions
    {
        public static FigureEnum FigureToEnum(Figure figure)
        {
            if (figure.GetType() == typeof(Pawn)) return FigureEnum.Pawn;
            if (figure.GetType() == typeof(Knight)) return FigureEnum.Knight;
            if (figure.GetType() == typeof(Bishop)) return FigureEnum.Bishop;
            if (figure.GetType() == typeof(Rook)) return FigureEnum.Rook;
            if (figure.GetType() == typeof(Queen)) return FigureEnum.Queen;
            return FigureEnum.King;
        }

        public static Type FigureEnumToType(this FigureEnum figureEnum)
        {
            if (figureEnum == FigureEnum.Pawn) return typeof(Pawn);
            if (figureEnum == FigureEnum.Knight) return typeof(Knight);
            if (figureEnum == FigureEnum.Bishop) return typeof(Bishop);
            if (figureEnum == FigureEnum.Rook) return typeof(Rook);
            if (figureEnum == FigureEnum.Queen) return typeof(Queen);
            return typeof(King);
        }
    }
}
