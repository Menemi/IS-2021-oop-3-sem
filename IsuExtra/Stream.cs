using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using IsuExtra.Exceptions;

namespace IsuExtra
{
    public class Stream
    {
        private static readonly int MaxCountOfStudents = 20;
        private int _countOfStudents = 0;
        private List<ComplementedStudent> _students;

        public Stream(MegaFaculty megaFaculty, List<Class> timetable)
        {
            _students = new List<ComplementedStudent>();
            MegaFaculty = megaFaculty;
            Timetable = timetable;
        }

        public MegaFaculty MegaFaculty { get; }

        private List<Class> Timetable { get; }

        public ReadOnlyCollection<ComplementedStudent> GetStudents()
        {
            return _students.AsReadOnly();
        }

        public void AddStudent(ComplementedStudent student)
        {
            if (_countOfStudents == MaxCountOfStudents)
            {
                throw new MaxStudentsException();
            }

            if (student.ComplementedGroup.GetMegaFaculty() == MegaFaculty)
            {
                throw new SameMegaFacultyException();
            }

            if (student.GetCountOfJgtd() == 2)
            {
                throw new LimitedNumberOfPlacesException();
            }

            var tempTimetable = new List<Class>();

            foreach (var streamClass in Timetable)
            {
                if (student.GetTimetable()
                    .Any(studentClass => studentClass.GetClassTime() == streamClass.GetClassTime()))
                {
                    throw new IntersectionInTimetable();
                }

                tempTimetable.Add(streamClass);
            }

            ++_countOfStudents;
            student.AddClasses(tempTimetable);
            student.SetCountOfJgtd(student.GetCountOfJgtd() + 1);
            _students.Add(student);
        }

        public void RemoveStudent(ComplementedStudent student)
        {
            if (student.GetCountOfJgtd() == 0 || !_students.Contains(student))
            {
                throw new StreamDoesNotIncludeStudent();
            }

            foreach (var streamClass in Timetable)
            {
                student.RemoveClass(streamClass);
            }

            --_countOfStudents;
            student.SetCountOfJgtd(student.GetCountOfJgtd() - 1);
            _students.Remove(student);
        }
    }
}