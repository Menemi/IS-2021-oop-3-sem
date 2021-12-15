namespace Banks
{
    public class PercentOfTheAmount
    {
        public PercentOfTheAmount(float lowerBound, float upperBound, float percent)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Percent = percent;
        }

        public float LowerBound { get; }

        public float UpperBound { get; }

        public float Percent { get; }
    }
}