namespace Chess
{
    public class Rook : Figure
    {
        protected override void UpdatePositions()
        {
            base.UpdatePositions();

            UpdateLine(1, 0);
            UpdateLine(-1, 0);
            UpdateLine(0, 1);
            UpdateLine(0, -1);
        }
    }
}