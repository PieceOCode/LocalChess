using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace Chess
{
    public class Move
    {
        private readonly Figure figure;
        private readonly Vector2Int from;
        private readonly Vector2Int to;
        private bool kicked;
        private Figure kickedFigure;

        public Move(Figure figure, Vector2Int from, Vector2Int to)
        {
            this.figure = figure;
            this.from = from;
            this.to = to;
        }

        public void Execute(Board board)
        {
            Assert.IsTrue(figure.CanMove(to));
            board.RemoveFigureFromSquare(from);

            if (!board.SquareIsEmpty(to))
            {
                Assert.IsTrue(board.GetFigure(to).Color != figure.Color);
                kickedFigure = board.GetFigure(to);
                kickedFigure.RaiseDestroyedEvent();
                kicked = true;
                board.RemoveFigureFromSquare(to);
            }

            board.SetFigureToSquare(figure, to);
            figure.Move(to);
        }

        public void Undo(Board board/*, SpawnManager spawnManager*/)
        {
            // Preconditions

            board.RemoveFigureFromSquare(to);
            if (kicked)
            {
                board.SetFigureToSquare(kickedFigure, to);
            }

            board.SetFigureToSquare(figure, from);
            figure.Move(from);
        }

        public void Redo(Board board)
        {
            Execute(board);
        }

        // TODO: Resolve ambiguities by mentioning which square the piece moved from if needed.
        // TODO: Add king and queen side casteling (O-O, O-O-O)
        // TODO: Add Pawn promotions
        // TODO: Add check (+) or checkmate (#)
        // TODO: Should castle and pawn promotions be subclasses?
        public void Serialize(StreamWriter sw)
        {
            string figureText = GetFigureCharacter(figure);
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

        private string GetFileCharacter(int x)
        {
            return ((Files)x).ToString().ToLower();
        }

        private string GetFigureCharacter(Figure figure)
        {
            switch (figure)
            {
                case Pawn p:
                    return "";
                case Knight k:
                    return "N";
                case Bishop b:
                    return "B";
                case Rook r:
                    return "R";
                case Queen q:
                    return "Q";
                case King k:
                    return "K";
                default:
                    Debug.LogError("There should not be another type of figure");
                    return "";
            }
        }
    }
}
