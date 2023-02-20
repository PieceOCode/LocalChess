namespace Chess
{
    public class Bishop : Figure
    {
        public override void UpdatePositions()
        {
            UpdateLine(1, 1);
            UpdateLine(1, -1);
            UpdateLine(-1, 1);
            UpdateLine(-1, -1);
        }
    }
}