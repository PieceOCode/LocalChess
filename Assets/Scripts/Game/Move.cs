using System;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace Chess
{
    public class Move
    {
        private readonly FigureData figureData;
        private readonly Vector2Int from;
        private readonly Vector2Int to;
        private bool kicked;
        private FigureData kickedData;

        public Move(Figure figure, Vector2Int from, Vector2Int to)
        {
            this.figureData = new FigureData(figure);
            this.from = from;
            this.to = to;
        }

        // BUG: Because the rook uses only the figure.Move function his position is not updated anymore.
        public void Execute(GameState gameState)
        {
            // Check if the specified figure exists and get it. 
            Board board = gameState.Board;
            Assert.IsTrue(!board.SquareIsEmpty(from));
            Figure figure = board.GetFigure(from);
            Assert.IsTrue(figure.GetType() == figureData.type
                && figure.Color == figureData.color);

            Assert.IsTrue(figure.CanMoveTo(to));
            board.RemoveFigureFromSquare(from);

            // Kick figure if square is blocked by enemy piece
            if (!board.SquareIsEmpty(to))
            {
                Assert.IsTrue(board.GetFigure(to).Color != figure.Color);
                kickedData = new FigureData(board.GetFigure(to));
                kicked = true;
                gameState.RemoveFigure(board.GetFigure(to));
            }

            board.SetFigureToSquare(figure, to);
            figure.Move(to);
        }

        public void Undo(GameState gameState)
        {
            // Preconditions

            // Check if the specified figure exists and get it. 
            Board board = gameState.Board;
            Assert.IsTrue(!board.SquareIsEmpty(to));
            Figure figure = board.GetFigure(to);
            Assert.IsTrue(figure.GetType() == figureData.type
                && figure.Color == figureData.color);

            board.RemoveFigureFromSquare(to);

            // Recreate figure if the moved kicked one.
            if (kicked)
            {
                Figure kickedFigure = CreateFigure(kickedData, gameState);

            }

            board.SetFigureToSquare(figure, from);
            figure.Move(from);
        }

        public void Redo(GameState gameState)
        {
            Execute(gameState);
        }

        // TODO: Resolve ambiguities by mentioning which square the piece moved from if needed.
        // TODO: Add king and queen side casteling (O-O, O-O-O)
        // TODO: Add Pawn promotions
        // TODO: Add check (+) or checkmate (#)
        // TODO: Should castle and pawn promotions be subclasses?
        public void Serialize(StreamWriter sw)
        {
            string figureText = GetFigureCharacter(figureData.type);
            sw.Write(figureText);
            if (kicked)
            {
                if (figureText.Length == 0)
                {
                    sw.Write(GetFileCharacter(from.x));
                }
                sw.Write('x');
            }
            sw.Write(to.ToChessNotation());
        }

        private Figure CreateFigure(FigureData data, GameState gameState)
        {
            if (data.type == typeof(Pawn)) { return new Pawn(data.position, data.color, gameState); }
            else if (data.type == typeof(Bishop)) { return new Bishop(data.position, data.color, gameState); }
            else if (data.type == typeof(Knight)) { return new Knight(data.position, data.color, gameState); }
            else if (data.type == typeof(Rook)) { return new Rook(data.position, data.color, gameState); }
            else if (data.type == typeof(Queen)) { return new Queen(data.position, data.color, gameState); }
            else if (data.type == typeof(King)) { return new King(data.position, data.color, gameState); }
            else return null;
        }


        private string GetFileCharacter(int x)
        {
            return ((Files)x).ToString().ToLower();
        }

        private string GetFigureCharacter(Type type)
        {
            if (type == typeof(Pawn)) return "";
            else if (type == typeof(Knight)) return "K";
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
    }
}
