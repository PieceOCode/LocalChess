using Chess;
using NUnit.Framework;
using System.Linq;
using UnityEngine;
using Color = Chess.Color;

public class GameStateTests
{
    private GameState emptyState;
    private GameState fullState;
    private Pawn whitePawn;
    private Pawn blackPawn;

    [SetUp]
    public void Setup()
    {
        emptyState = new GameState();
        fullState = new GameState();
        fullState.SpawnFigures();
        whitePawn = new Pawn(new Vector2Int(0, 0), Color.White, null);
        blackPawn = new Pawn(new Vector2Int(7, 7), Color.Black, null);
    }

    public class PiecesPropertyTests : GameStateTests
    {
        [Test]
        public void empty_state_has_no_pieces()
        {
            Assert.That(emptyState.Pieces.Count, Is.EqualTo(0));
        }

        [Test]
        public void state_with_pawn_has_pieces()
        {
            emptyState.AddFigure(whitePawn);
            Assert.That(emptyState.Pieces.Count, Is.EqualTo(1));
        }

        [Test]
        public void full_state_has_pieces()
        {
            Assert.That(fullState.Pieces.Count, Is.GreaterThan(0));
        }
    }

    public class WhitePiecesPropertyTests : GameStateTests
    {
        [Test]
        public void empty_state_has_no_pieces()
        {
            Assert.That(emptyState.WhitePieces.Count, Is.EqualTo(0));
        }

        [Test]
        public void state_with_white_pawn_has_pieces()
        {
            emptyState.AddFigure(whitePawn);
            Assert.That(emptyState.WhitePieces.Count, Is.EqualTo(1));
        }

        [Test]
        public void state_with_black_pawn_has_no_pieces()
        {
            emptyState.AddFigure(blackPawn);
            Assert.That(emptyState.WhitePieces.Count, Is.EqualTo(0));
        }

        [Test]
        public void full_state_has_pieces()
        {
            Assert.That(fullState.WhitePieces.Count, Is.GreaterThan(0));
        }
    }

    public class BlackPiecesPropertyTests : GameStateTests
    {
        [Test]
        public void empty_state_has_no_pieces()
        {
            Assert.That(emptyState.BlackPieces.Count, Is.EqualTo(0));
        }

        [Test]
        public void state_with_white_pawn_has_no_pieces()
        {
            emptyState.AddFigure(whitePawn);
            Assert.That(emptyState.BlackPieces.Count, Is.EqualTo(0));
        }

        [Test]
        public void state_with_black_pawn_has_no_pieces()
        {
            emptyState.AddFigure(blackPawn);
            Assert.That(emptyState.BlackPieces.Count, Is.EqualTo(1));
        }

        [Test]
        public void full_state_has_pieces()
        {
            Assert.That(fullState.BlackPieces.Count, Is.GreaterThan(0));
        }
    }

    public class KingPropertyTests : GameStateTests
    {
        [Test]
        public void empty_state_has_no_kings()
        {
            Assert.That(emptyState.WhiteKing, Is.Null);
            Assert.That(emptyState.BlackKing, Is.Null);
        }

        [Test]
        public void full_state_has_kings()
        {
            Assert.That(fullState.WhiteKing, Is.Not.Null);
            Assert.That(fullState.BlackKing, Is.Not.Null);
        }
    }

    public class ActivePlayerTests : GameStateTests
    {
        [Test]
        public void first_player_is_white()
        {
            Assert.That(emptyState.ActivePlayer, Is.EqualTo(Color.White));
        }

        [Test]
        public void switching_player_from_white_gives_black()
        {
            emptyState.SwitchActivePlayer();
            Assert.That(emptyState.ActivePlayer, Is.EqualTo(Color.Black));
        }
    }

    public class BoardPropertyTests : GameStateTests
    {
        [Test]
        public void has_a_board()
        {
            Assert.That(emptyState.Board, Is.Not.Null);
            Assert.That(fullState.Board, Is.Not.Null);
        }
    }

    public class AddFigureMethodTests : GameStateTests
    {
        [Test]
        public void figure_is_added()
        {
            emptyState.AddFigure(whitePawn);
            Assert.That(emptyState.Pieces.First(), Is.EqualTo(whitePawn));
            Assert.That(emptyState.WhitePieces.First(), Is.EqualTo(whitePawn));
        }

        [Test]
        public void throws_error_if_square_already_full()
        {
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() => fullState.AddFigure(whitePawn));
        }

        [Test]
        public void throws_error_if_figure_already_exists()
        {
            Bishop bishop = new Bishop(new Vector2Int(0, 0), Color.White, null);
            emptyState.AddFigure(bishop);
            bishop.Move(new Vector2Int(1, 1));
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() => emptyState.AddFigure(bishop));
        }

        [Test]
        public void throws_error_if_figure_is_null()
        {
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() => fullState.AddFigure(null));
        }
    }

    public class RemoveFigureMethodTests : GameStateTests
    {
        [Test]
        public void figure_is_removed_from_pieces()
        {
            Figure figure = fullState.Pieces.First();
            fullState.RemoveFigure(figure);
            Assert.That(fullState.Pieces.Contains(figure), Is.False);
        }

        [Test]
        public void throws_error_if_figure_does_not_exist()
        {
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() => fullState.RemoveFigure(whitePawn));
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() => emptyState.RemoveFigure(whitePawn));
        }

        [Test]
        public void throws_error_if_figure_is_null()
        {
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() => fullState.RemoveFigure(null));
        }
    }

    public class SpawnFiguresMethodTests : GameStateTests
    {
        [Test]
        public void spawns_correctly()
        {
            emptyState.SpawnFigures();
            Assert.That(emptyState.Pieces.Count, Is.EqualTo(32));
            Assert.That(emptyState.WhitePieces.Count, Is.EqualTo(16));
            Assert.That(emptyState.WhitePieces.Count, Is.EqualTo(16));

            Assert.That(emptyState.WhiteKing, Is.Not.Null);
            Assert.That(emptyState.BlackKing, Is.Not.Null);

            Assert.That(emptyState.Pieces.Where(p => p is Pawn).Count, Is.EqualTo(16));
            Assert.That(emptyState.Pieces.Where(p => p is Knight).Count, Is.EqualTo(4));
            Assert.That(emptyState.Pieces.Where(p => p is Bishop).Count, Is.EqualTo(4));
            Assert.That(emptyState.Pieces.Where(p => p is Rook).Count, Is.EqualTo(4));
            Assert.That(emptyState.Pieces.Where(p => p is Queen).Count, Is.EqualTo(2));
            Assert.That(emptyState.Pieces.Where(p => p is King).Count, Is.EqualTo(2));
        }
    }

}
