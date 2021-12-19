namespace Banks
{
    public class PercentOfTheAmount
    {
        public PercentOfTheAmount(double lowerBound, double upperBound, double percent)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Percent = percent;
        }

        public double LowerBound { get; }

        public double UpperBound { get; }

        public double Percent { get; }
    }
}