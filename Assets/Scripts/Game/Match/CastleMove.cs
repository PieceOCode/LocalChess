using System.IO;
using UnityEngine;

namespace Chess
{
    public class CastleMove : Move
    {
        public CastleMove(King king, Vector2Int from, Vector2Int to) : base(king, from, to) { }

        override public void Do(GameState gameState)
        {
            Figure king = gameState.Board.GetFigure(from);
            gameState.MoveFigure(king, to);

            if (from.x > to.x)
            {
                // Queen side castle
                Figure rook = gameState.Board.GetFigure(new Vector2Int(0, from.y));
                gameState.MoveFigure(rook, to + Vector2Int.right);
            }
            else
            {
                // King side castle
                Figure rook = gameState.Board.GetFigure(new Vector2Int(gameState.Board.Width - 1, from.y));
                gameState.MoveFigure(rook, to + Vector2Int.left);
            }
        }

        override public void Undo(GameState gameState)
        {
            Figure king = gameState.Board.GetFigure(to);
            gameState.MoveFigure(king, from);

            if (from.x > to.x)
            {
                // Queen side castle
                Figure rook = gameState.Board.GetFigure(to + Vector2Int.right);
                gameState.MoveFigure(rook, new Vector2Int(0, from.y));
            }
            else
            {
                // King side castle
                Figure rook = gameState.Board.GetFigure(to + Vector2Int.left);
                gameState.MoveFigure(rook, new Vector2Int(gameState.Board.Width - 1, from.y));
            }
        }

        public override void Serialize(StreamWriter sw)
        {
            // TODO: Add king and queen side casteling (O-O, O-O-O)
            if (from.x > to.x)
            {
                // Queen side castle
                sw.Write("O-O-O");
            }
            else
            {
                // King side castle
                sw.Write("O-O");
            }
        }
    }
}
