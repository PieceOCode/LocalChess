using System.IO;
using UnityEngine;
using UnityEngine.Assertions;


namespace Chess
{
    public class Move
    {
        protected readonly FigureData figureData;
        protected readonly Vector2Int from;
        protected readonly Vector2Int to;
        protected bool kicked;
        protected FigureData kickedData;

        public Move(Figure figure, Vector2Int from, Vector2Int to)
        {
            this.figureData = new FigureData(figure);
            this.from = from;
            this.to = to;
        }

        public virtual void Do(GameState gameState)
        {
            // Check if the specified figure exists and get it. 
            Board board = gameState.Board;
            Assert.IsTrue(!board.SquareIsEmpty(from));
            Figure figure = board.GetFigure(from);
            Assert.IsTrue(figure.GetType() == figureData.type
                && figure.Color == figureData.color);
            Assert.IsTrue(figure.CanMoveTo(to));

            // Kick figure if square is blocked by enemy piece
            if (!board.SquareIsEmpty(to))
            {
                Assert.IsTrue(board.GetFigure(to).Color != figure.Color);
                kickedData = new FigureData(board.GetFigure(to));
                kicked = true;
                gameState.RemoveFigure(board.GetFigure(to));
            }

            gameState.MoveFigure(figure, to);
        }

        public virtual void Undo(GameState gameState)
        {
            // Preconditions

            // Check if the specified figure exists and get it. 
            Board board = gameState.Board;
            Assert.IsTrue(!board.SquareIsEmpty(to));
            Figure figure = board.GetFigure(to);
            Assert.IsTrue(figure.GetType() == figureData.type
                && figure.Color == figureData.color);

            gameState.MoveFigure(figure, from);

            // Recreate figure if the moved kicked one.
            if (kicked)
            {
                Figure kickedFigure = FigureData.CreateFigureFrom(kickedData, gameState);
                gameState.AddFigure(kickedFigure);
            }
        }

        public void Redo(GameState gameState)
        {
            Do(gameState);
        }

        // TODO: Resolve ambiguities by mentioning which square the piece moved from if needed.
        // TODO: Add check (+) or checkmate (#)
        public virtual void Serialize(StreamWriter sw)
        {
            string figureText = ChessNotation.GetPieceNotation(figureData.type);
            sw.Write(figureText);
            if (kicked)
            {
                if (figureText.Length == 0)
                {
                    sw.Write(ChessNotation.GetFileNotation(from.x));
                }
                sw.Write('x');
            }
            sw.Write(ChessNotation.GetSquareNotation(to));
        }
    }
}
