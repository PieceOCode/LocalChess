using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Chess
{
    public class CastleMove : Move
    {
        private bool IsQueensideCastle => from.x > to.x;

        public CastleMove(King king, Vector2Int from, Vector2Int to) : base(king, from, to)
        {
            Assert.IsTrue(Vector2Int.Distance(from, to) == 2);
        }

        override public void Do(GameState gameState)
        {
            Figure king = gameState.Board.GetFigure(from);
            Assert.IsTrue(king is King && king.Color == figureData.color);
            Assert.IsTrue(!king.HasMoved);
            Assert.IsTrue(king.CanMoveTo(to));

            gameState.MoveFigure(king, to);
            if (IsQueensideCastle)
            {
                Figure rook = gameState.Board.GetFigure(new Vector2Int(0, from.y));
                Assert.IsTrue(rook is Rook && rook.Color == figureData.color);
                Assert.IsTrue(!rook.HasMoved);
                gameState.MoveFigure(rook, to + Vector2Int.right);
            }
            else
            {
                Figure rook = gameState.Board.GetFigure(new Vector2Int(gameState.Board.Width - 1, from.y));
                Assert.IsTrue(rook is Rook && rook.Color == figureData.color);
                Assert.IsTrue(!rook.HasMoved);
                gameState.MoveFigure(rook, to + Vector2Int.left);
            }
        }

        override public void Undo(GameState gameState)
        {
            Figure king = gameState.Board.GetFigure(to);
            Assert.IsTrue(king is King && king.Color == figureData.color);

            gameState.MoveFigure(king, from);
            king.HasMoved = false;

            if (IsQueensideCastle)
            {
                Figure rook = gameState.Board.GetFigure(to + Vector2Int.right);
                Assert.IsTrue(rook is Rook && rook.Color == figureData.color);
                gameState.MoveFigure(rook, new Vector2Int(0, from.y));
                rook.HasMoved = false;
            }
            else
            {
                Figure rook = gameState.Board.GetFigure(to + Vector2Int.left);
                Assert.IsTrue(rook is Rook && rook.Color == figureData.color);
                gameState.MoveFigure(rook, new Vector2Int(gameState.Board.Width - 1, from.y));
                rook.HasMoved = false;
            }
        }

        public override string Serialize()
        {
            if (IsQueensideCastle)
            {
                return "O-O-O";
            }
            else
            {
                return "O-O";
            }
        }

        public static new CastleMove Deserialize(string moveText, Game game)
        {
            King king = game.GameState.ActivePlayer == Color.White ? game.GameState.WhiteKing : game.GameState.BlackKing;
            if (moveText.Count(x => x == 'O') == 2)
            {
                return new CastleMove(king, king.Position, king.Position + Vector2Int.right * 2); // King-side castle
            }
            else
            {
                return new CastleMove(king, king.Position, king.Position + Vector2Int.left * 2); // Queen-side castle
            }
        }
    }
}
