using Chess;
using NUnit.Framework;
using System;
using UnityEngine;
using Color = Chess.Color;

public class BoardTests
{
    private Board board;
    private Pawn pawn;
    private Vector2Int pos0;
    private Vector2Int posOutside;

    [SetUp]
    public void SetUp()
    {
        board = new Board(8, 8);
        pawn = new Pawn();

        pos0 = new Vector2Int();
        posOutside = new Vector2Int(8, 8);
    }

    public class IsEmptyMethod : BoardTests
    {
        [Test]
        public void new_board_is_empty()
        {
            for (int x = 0; x < board.Width; x++)
            {
                for (int y = 0; y < board.Height; y++)
                {
                    Assert.That(board.SquareIsEmpty(new Vector2Int(x, y)), Is.True);
                }
            }
        }

        [Test]
        public void square_with_figure_is_not_empty()
        {
            board.SetFigureToSquare(pawn, pos0);
            Assert.That(board.SquareIsEmpty(pos0), Is.False);
        }

        [Test]
        public void throws_exception_for_position_outside_board()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => board.SquareIsEmpty(posOutside));
        }
    }

    public class GetFigureMethod : BoardTests
    {
        [Test]
        public void non_empty_square_returns_figure()
        {
            board.SetFigureToSquare(pawn, pos0);
            Assert.That(board.GetFigure(pos0), Is.SameAs(pawn));
        }

        [Test]
        public void throws_exception_for_empty_square()
        {
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() => board.GetFigure(pos0));
        }

        [Test]
        public void throws_exception_for_position_outside_board()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => board.GetFigure(posOutside));
        }
    }

    public class SetFigureMethod : BoardTests
    {
        [Test]
        public void setting_figure_to_square_makes_it_not_empty()
        {
            board.SetFigureToSquare(pawn, pos0);
            Assert.That(board.SquareIsEmpty(pos0), Is.False);
        }

        [Test]
        public void throws_exception_if_square_is_full()
        {
            board.SetFigureToSquare(pawn, pos0);
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() => board.SetFigureToSquare(pawn, pos0));
        }

        [Test]
        public void throws_exception_when_figure_is_null()
        {
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() => board.SetFigureToSquare(null, pos0));
        }

        [Test]
        public void throws_exception_for_position_outside_board()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => board.SetFigureToSquare(pawn, posOutside));
        }
    }

    public class RemoveFigureMethod : BoardTests
    {
        [Test]
        public void empty_square_stays_empty()
        {
            board.RemoveFigureFromSquare(pos0);
            Assert.That(board.SquareIsEmpty(pos0));
        }

        [Test]
        public void figure_is_removed_from_square()
        {
            board.SetFigureToSquare(pawn, pos0);
            board.RemoveFigureFromSquare(pos0);
            Assert.That(board.SquareIsEmpty(pos0), Is.True);
        }

        [Test]
        public void throws_exception_for_position_outside_board()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => board.RemoveFigureFromSquare(posOutside));
        }
    }

    public class SquareHasEnemyPieceMethod : BoardTests
    {
        [Test]
        public void empty_square_has_no_enemy_piece()
        {
            Assert.That(board.SquareHasEnemyPiece(Color.White, pos0), Is.False);
        }

        [Test]
        public void square_with_own_figure_has_no_enemy_piece()
        {
            pawn.SetFigure(new Vector2Int(), Color.White, null);
            board.SetFigureToSquare(pawn, pos0);
            Assert.That(board.SquareHasEnemyPiece(Color.White, pos0), Is.False);
        }

        [Test]
        public void square_with_white_piece_has_enemy_piece()
        {
            pawn.SetFigure(new Vector2Int(), Color.White, null);
            board.SetFigureToSquare(pawn, pos0);
            Assert.That(board.SquareHasEnemyPiece(Color.Black, pos0), Is.True);
        }

        [Test]
        public void square_with_black_piece_has_enemy_piece()
        {
            pawn.SetFigure(new Vector2Int(), Color.Black, null);
            board.SetFigureToSquare(pawn, pos0);
            Assert.That(board.SquareHasEnemyPiece(Color.White, pos0), Is.True);
        }

        [Test]
        public void throws_exception_for_position_outside_board()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => board.SquareHasEnemyPiece(Color.White, posOutside));
        }
    }

    public class PositionIsValidMethod : BoardTests
    {
        [Test]
        public void valid_position_is_valid()
        {
            Assert.That(board.IsPositionValid(pos0), Is.True);
        }

        public void invalid_position_is_invalid()
        {
            Assert.That(board.IsPositionValid(posOutside), Is.False);
        }
    }
}
