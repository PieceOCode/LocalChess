using System.Collections.Generic;

namespace Chess
{
    // TODO: Add result
    /// <summary>
    /// Contains a history of the game in the form of a list of all moves that happened and the current state/move of the game. 
    /// </summary>
    public class Match
    {
        public List<Move> Moves => moves;
        public int CurrentIndex => currentIndex;

        private readonly List<Move> moves = new List<Move>();
        private int currentIndex = 0;

        public void Undo(GameState gameState)
        {
            if (currentIndex >= 1)
            {
                currentIndex--;
                moves[currentIndex].Undo(gameState);
            }
        }

        public void Redo(GameState gameState)
        {
            if (currentIndex < moves.Count)
            {
                moves[currentIndex].Redo(gameState);
                currentIndex++;
            }
        }

        public void Add(Move move)
        {
            moves.Add(move);
        }

        public void SetIndex(int index)
        {
            this.currentIndex = index;
        }
    }
}
