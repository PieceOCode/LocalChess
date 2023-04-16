using Chess;
using NUnit.Framework;
using Color = Chess.Color;

public class GameStateTests
{
    public class ActivePlayerTests
    {
        [Test]
        public void first_player_is_white()
        {
            GameState state = new GameState();
            Assert.That(state.ActivePlayer, Is.EqualTo(Color.White));
        }

        [Test]
        public void switching_player_from_white_gives_black()
        {
            GameState state = new GameState();
            state.SwitchActivePlayer();
            Assert.That(state.ActivePlayer, Is.EqualTo(Color.Black));
        }
    }

}
