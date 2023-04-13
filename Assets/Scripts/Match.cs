using System.Collections.Generic;

namespace Chess
{
    public class Match
    {
        private readonly List<Move> moves = new List<Move>();
        private int currentIndex = 0;

        public void Undo()
        {
            if (currentIndex > 1)
            {
                currentIndex--;
                moves[currentIndex].Undo();
            }
        }

        public void Redo()
        {
            if (currentIndex <= moves.Count - 1)
            {
                moves[currentIndex].Redo();
                currentIndex++;
            }
        }

        public void Add(Move move)
        {
            moves.Add(move);
        }
    }
}
