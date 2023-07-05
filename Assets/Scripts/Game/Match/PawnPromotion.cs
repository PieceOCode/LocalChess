using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Chess
{
    public class PawnPromotion : Move
    {
        private readonly FigureData promotedFigureData;

        public PawnPromotion(Pawn figure, Type promotedFigureType, Vector2Int from, Vector2Int to) : base(figure, from, to)
        {
            Assert.IsTrue(promotedFigureType == typeof(Bishop)
                || promotedFigureType == typeof(Knight)
                || promotedFigureType == typeof(Rook)
                || promotedFigureType == typeof(Queen));
            this.promotedFigureData = new FigureData(promotedFigureType, figure.Color, to, true);
        }

        public override void Do(GameState gameState)
        {
            base.Do(gameState);

            Figure pawn = gameState.Board.GetFigure(to);
            Assert.IsTrue(pawn is Pawn && pawn.Color == figureData.color);
            gameState.RemoveFigure(pawn);

            Figure promotedFigure = FigureData.CreateFigureFrom(promotedFigureData, gameState);
            gameState.AddFigure(promotedFigure);
            promotedFigure.Move(to);
        }

        public override void Undo(GameState gameState)
        {
            Figure figure = gameState.Board.GetFigure(to);
            gameState.RemoveFigure(figure);

            Pawn pawn = new Pawn(to, figureData.color, gameState);
            gameState.AddFigure(pawn);
            pawn.Move(to);

            base.Undo(gameState);
        }

        public override string Serialize()
        {
            string s = base.Serialize();
            return s + "=" + ChessNotation.GetPieceNotation(promotedFigureData.type);
        }
    }
}
