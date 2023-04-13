using UnityEngine;

namespace Chess
{
    public class Move
    {
        private readonly Figure figure;
        private readonly Vector2Int from;
        private readonly Vector2Int to;

        public Move(Figure figure, Vector2Int from, Vector2Int to)
        {
            this.figure = figure;
            this.from = from;
            this.to = to;
        }

        public void Execute()
        {
            figure.Move(to);
        }

        public void Undo() { }

        public void Redo()
        {
            Execute();
        }
    }
}
