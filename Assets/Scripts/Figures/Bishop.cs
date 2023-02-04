namespace Chess
{
    public class Bishop : Figure
    {
        protected override void UpdatePositions()
        {
            moveablePositions.Clear();

            UpdateLine(1, 1);
            UpdateLine(1, -1);
            UpdateLine(-1, 1);
            UpdateLine(-1, -1);
        }
    }
}