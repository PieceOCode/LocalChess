using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        public bool isCheck = false;
        public bool isCheckmate = false;

        public Move(Figure figure, Vector2Int from, Vector2Int to)
        {
            Assert.IsNotNull(figure);
            this.figureData = new FigureData(figure);
            this.from = from;
            this.to = to;
        }

        public virtual void Do(GameState gameState)
        {
            Board board = gameState.Board;

            Figure figure = board.GetFigure(from);
            Assert.IsTrue(figure.GetType() == figureData.type && figure.Color == figureData.color); // Asserts that we got the correct figure.
            Assert.IsTrue(figure.CanMoveTo(to));

            // Kick figure if square is blocked by enemy piece
            if (!board.SquareIsEmpty(to))
            {
                Assert.IsTrue(board.SquareHasEnemyPiece(figureData.color, to));
                kickedData = new FigureData(board.GetFigure(to));
                kicked = true;
                gameState.RemoveFigure(board.GetFigure(to));
            }

            gameState.MoveFigure(figure, to);
        }

        public virtual void Undo(GameState gameState)
        {
            Figure figure = gameState.Board.GetFigure(to);
            Assert.IsTrue(figure.GetType() == figureData.type && figure.Color == figureData.color);

            gameState.MoveFigure(figure, from);
            figure.HasMoved = figureData.hasMoved;

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

        public override string ToString()
        {
            return Serialize();
        }

        // TODO: Resolve ambiguities by mentioning which square the piece moved from if needed.
        // TODO: Add check (+) or checkmate (#)
        public virtual string Serialize()
        {
            string serializedMove = "";
            string figureText = ChessNotation.GetPieceNotation(figureData.type);
            serializedMove += figureText;
            if (kicked)
            {
                if (figureText.Length == 0)
                {
                    serializedMove += ChessNotation.GetFileNotation(from.x);
                }
                serializedMove += 'x';
            }
            serializedMove += ChessNotation.GetSquareNotation(to);

            if (isCheckmate)
            {
                serializedMove += "#";
            }
            else if (isCheck)
            {
                serializedMove += "+";
            }

            return serializedMove;
        }

        public static Move Deserialize(string moveText, Game game)
        {
            Type type = ChessNotation.GetTypeFromNotation(moveText[0]);
            if (type == null)
            {
                type = typeof(Pawn);
            }

            MatchCollection fileCollection = new Regex(@"[a-h]").Matches(moveText);
            int file = ChessNotation.GetFileNumber(fileCollection.Last().Value[0]);

            MatchCollection rankCollection = new Regex(@"\d").Matches(moveText);
            int rank = ChessNotation.GetRankNumber(rankCollection.Last().Value[0]);

            Vector2Int destination = new Vector2Int(file, rank);

            IEnumerable<Figure> figures = game.GameState.Pieces.Where((fig) =>
                fig.Color == game.ActivePlayer &&
                fig.GetType() == type &&
                fig.CanMoveTo(destination)
            );

            // If two figures of the same type can move to this tile, find out which one moved (specified by its rank or file e.g Nec3 or B3d4)
            Figure figure = figures.First();
            if (figures.Count() > 1)
            {
                if (fileCollection.Count() > 1)
                {
                    int fileNumber = ChessNotation.GetFileNumber(fileCollection.First().Value[0]);
                    figure = figures.Where(fig => fig.Position.x == fileNumber).First();
                }
                else if (rankCollection.Count() > 1)
                {
                    int rankNumber = ChessNotation.GetRankNumber(rankCollection.First().Value[0]);
                    figure = figures.Where(fig => fig.Position.y == rankNumber).First();
                }
                else
                {
                    throw new Exception("If more than one figure can move to destination, the move should specify the rank or file of the figure that moved.");
                }
            }

            // Check if the move is a pawn promotion.
            if (moveText.Contains('='))
            {
                MatchCollection promotedFigureCollection = new Regex(@"[N, B, R, Q]", RegexOptions.IgnoreCase).Matches(moveText);
                Assert.IsTrue(promotedFigureCollection.Count() == 1, "A pawn promotion move should not contain more than one of the letters [N,B,R,Q].");
                Type figureType = ChessNotation.GetTypeFromNotation(promotedFigureCollection.Last().Value[0]);
                return new PawnPromotion(figure as Pawn, figureType, figure.Position, destination);
            }

            return new Move(figure, figure.Position, destination);
        }
    }
}
