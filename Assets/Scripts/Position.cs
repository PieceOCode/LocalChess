using UnityEngine;

namespace Chess
{
    public readonly struct Position
    {
        public int File { get; }
        public int Rank { get; }

        public Position(int file, int rank)
        {
            this.File = file;
            this.Rank = rank;
        }

        public override readonly string ToString()
        {
            return $"{(Files)File}{Rank + 1}";
        }

        // Manhattan distance between squares
        public readonly int Distance(Position other)
        {
            return Mathf.Abs(this.File - other.File) + Mathf.Abs(this.Rank - this.File);
        }

        public readonly bool IsValid()
        {
            return (File >= 0 && File <= 7) && (Rank >= 0 && Rank <= 7);
        }

        public static bool operator ==(Position left, Position right)
        {
            return left.Rank == right.Rank && left.File == right.File;
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }
    }

    public enum Files
    {
        A, B, C, D, E, F, G, H
    }
}
