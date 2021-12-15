namespace Banks
{
    public class Passport
    {
        public Passport(int series, int number)
        {
            Series = series;
            Number = number;
        }

        public int Series { get; set; }

        public int Number { get; set; }
    }
}