using System.Collections.Generic;
using System.Linq;

namespace IsuExtra
{
    public class ComplementedStudent : Isu.Services.Student
    {
        private int _countJgtd;

        private List<Class> _timetable;

        public ComplementedStudent(string name, ComplementedGroup group)
            : base(name, group)
        {
            _timetable = group.GetTimetable().ToList();
            _countJgtd = 0;
            ComplementedGroup = group;
        }

        public ComplementedGroup ComplementedGroup { get; }

        public List<Class> GetTimetable()
        {
            return _timetable;
        }

        public void AddClasses(List<Class> sClasses)
        {
            _timetable.AddRange(sClasses);
        }

        public void RemoveClass(Class sClass)
        {
            _timetable.Remove(sClass);
        }

        public int GetCountOfJgtd()
        {
            return _countJgtd;
        }

        public void SetCountOfJgtd(int countJgtd)
        {
            _countJgtd = countJgtd;
        }
    }
}