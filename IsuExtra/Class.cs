using System;

namespace IsuExtra
{
    public class Class
    {
        public Class(string name, WeekDayTime classTime, string teacher, int auditorium)
        {
            Name = name;
            ClassTime = classTime;
            Teacher = teacher;
            Auditorium = auditorium;
        }

        private string Name { get; }

        private WeekDayTime ClassTime { get; }

        private string Teacher { get; }

        private int Auditorium { get; }

        public WeekDayTime GetClassTime()
        {
            return ClassTime;
        }

        public override bool Equals(object obj)
        {
            return obj is Class @class && Equals(@class);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, ClassTime, Teacher, Auditorium);
        }

        private bool Equals(Class other)
        {
            return Name == other.Name && ClassTime == other.ClassTime &&
                   Teacher == other.Teacher && Auditorium == other.Auditorium;
        }
    }
}