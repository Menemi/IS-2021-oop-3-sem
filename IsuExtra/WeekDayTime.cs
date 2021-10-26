using System;

namespace IsuExtra
{
    public enum WeekDay
    {
        Monday = 1,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday,
    }

    public enum ClassTime
    {
        Zero = 0,
        First,
        Second,
        Third,
        Fourth,
        Fifth,
    }

    public class WeekDayTime
    {
        public WeekDayTime(WeekDay day, ClassTime time)
        {
            Day = day;
            Time = time;
        }

        private WeekDay Day { get; }

        private ClassTime Time { get; }

        public override int GetHashCode()
        {
            return HashCode.Combine(Day, Time);
        }

        protected bool Equals(WeekDayTime other)
        {
            return Day == other.Day && Time == other.Time;
        }
    }
}