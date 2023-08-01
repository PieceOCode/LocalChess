using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.Assertions;

namespace Chess
{
    public static class MatchSerializer
    {
        public static void SerializeMatch(Match match, string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (StreamWriter sw = new StreamWriter(path))
            {
                for (int i = 0; i < match.Moves.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        sw.Write($"{(i / 2) + 1}. ");
                    }

                    sw.Write(match.Moves[i].Serialize());
                    sw.Write(" ");
                }
            }
        }

        public static Match DeserializeMatch(string path)
        {
            Assert.IsTrue(File.Exists(path), "No file exists at the given position: " + path);

            Game game = new Game();
            game.StartGame();

            using (StreamReader sr = new StreamReader(path))
            {
                string line = ReadMetaData(sr);

                MatchCollection movesMatches = SplitMoves(sr, line);
                foreach (System.Text.RegularExpressions.Match moveMatch in movesMatches)
                {
                    Move move = CreateMove(game, moveMatch.Value);
                    if (move != null)
                    {
                        game.ExecuteMove(move);
                    }
                }

                game.Match.SetIndex(0);
                return game.Match;
            }
        }

        private static Move CreateMove(Game game, string moveText)
        {
            Move move;
            if (moveText[0] == 'O')
            {
                return CastleMove.Deserialize(moveText, game);
            }
            else if (new Regex(@"[01]-[01]").IsMatch(moveText))
            {
                move = null; // Surrender move?
            }
            else
            {
                move = Move.Deserialize(moveText, game);
            }

            return move;
        }

        private static string ReadMetaData(StreamReader sr)
        {
            string line = sr.ReadLine();
            while (line.Length > 0 && line[0] == '[')
            {
                line = sr.ReadLine();
            }

            return line;
        }

        private static MatchCollection SplitMoves(StreamReader sr, string line)
        {
            string gameHistory = line + sr.ReadToEnd();
            Regex rx = new Regex(@"(?!\d*\.)[^\s]+");
            MatchCollection movesMatches = rx.Matches(gameHistory);
            return movesMatches;
        }
    }
}
