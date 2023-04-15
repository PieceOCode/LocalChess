using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Chess
{
    // TODO: Add result
    public class Match
    {
        private readonly List<Move> moves = new List<Move>();
        private int currentIndex = 0;

        public void Undo(Board board, SpawnManager spawnManager)
        {
            if (currentIndex >= 1)
            {
                currentIndex--;
                moves[currentIndex].Undo(board, spawnManager);
            }
        }

        public void Redo(Board board)
        {
            if (currentIndex <= moves.Count - 1)
            {
                moves[currentIndex].Redo(board);
                currentIndex++;
            }
        }

        public void Add(Move move)
        {
            moves.Add(move);
        }

        public void Serialize()
        {
            string path = Application.persistentDataPath + "/match.pgn";
            Debug.Log(path);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (StreamWriter sw = new StreamWriter(path))
            {
                for (int i = 0; i < moves.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        sw.Write($"{(i / 2) + 1}. ");
                    }

                    moves[i].Serialize(sw);
                    sw.Write(" ");
                }
            }
        }
    }

}
