using System;

namespace IsuExtra
{
    public class WeekDayTime
    {
        public WeekDayTime(WeekDay day, ClassNumber number)
        {
            Day = day;
            Number = number;
        }

        private WeekDay Day { get; }

        private ClassNumber Number { get; }

        public override bool Equals(object obj)
        {
            return obj is WeekDayTime time && Equals(time);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Day, Number);
        }

        private bool Equals(WeekDayTime other)
        {
            return Day == other.Day && Number == other.Number;
        }
    }
}