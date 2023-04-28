using UnityEngine;
using UnityEngine.Assertions;

namespace Chess
{
    public class PawnPromotion : Move
    {
        public PawnPromotion(Pawn figure, Vector2Int from, Vector2Int to) : base(figure, from, to) { }

        public override void Do(GameState gameState)
        {
            base.Do(gameState);

            Figure pawn = gameState.Board.GetFigure(to);
            Assert.IsTrue(pawn is Pawn && pawn.Color == figureData.color);
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

        public override string Serialize()
        {
            string s = base.Serialize();
            return s + "=Q";
        }
    }
}
