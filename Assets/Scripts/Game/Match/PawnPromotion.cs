using System.IO;
using UnityEngine;

namespace Chess
{
    public class PawnPromotion : Move
    {
        public PawnPromotion(Pawn figure, Vector2Int from, Vector2Int to) : base(figure, from, to) { }

        public override void Do(GameState gameState)
        {
            base.Do(gameState);

            Figure pawn = gameState.Board.GetFigure(to);
            gameState.RemoveFigure(pawn);

            Queen queen = new Queen(to, figureData.color, gameState);
            gameState.AddFigure(queen);
            queen.Move(to);
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

        public override void Serialize(StreamWriter sw)
        {
            base.Serialize(sw);
            sw.Write("=Q");
        }
    }
}
