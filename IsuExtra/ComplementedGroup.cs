using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Isu.Tools;

namespace IsuExtra
{
    public class ComplementedGroup : Isu.Services.Group
    {
        private static readonly int MaxCountOfStudents = 20;
        private List<ComplementedStudent> _complementedStudent;
        private int _countOfStudents = 0;

        public ComplementedGroup(string groupName, List<Class> timetable, MegaFaculty megaFaculty) // M3204
            : base(groupName)
        {
            Timetable = timetable;
            MegaFaculty = megaFaculty;
            _complementedStudent = new List<ComplementedStudent>();
        }

        private List<Class> Timetable { get; }

        private MegaFaculty MegaFaculty { get; }

        public ReadOnlyCollection<ComplementedStudent> GetStudents()
        {
            return _complementedStudent.AsReadOnly();
        }

        public ReadOnlyCollection<Class> GetTimetable()
        {
            return Timetable.AsReadOnly();
        }

        public MegaFaculty GetMegaFaculty()
        {
            return MegaFaculty;
        }

        public void AddStudent(ComplementedStudent student)
        {
            if (_countOfStudents >= MaxCountOfStudents)
            {
                throw new MaxStudentsIsuException();
            }

            ++_countOfStudents;
            _complementedStudent.Add(student);
        }

        public List<ComplementedStudent> GetNotSignedUp()
        {
            return _complementedStudent.Where(student => student.GetCountOfJgtd() == 0).ToList();
        }
    }
}