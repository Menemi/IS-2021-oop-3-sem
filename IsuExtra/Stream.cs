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
                throw new MaxStudentsException(
                    $"{MaxCountOfStudents} is the limit of students in a {MegaFaculty} stream");
            }

            if (student.ComplementedGroup.GetMegaFaculty() == MegaFaculty)
            {
                throw new SameMegaFacultyException();
            }

            if (student.GetCountOfJgtd() == 2)
            {
                throw new LimitedNumberOfPlacesException($"2 is the max count of jgtd");
            }

            if (Timetable.Any(streamClass =>
                student.GetTimetable().Any(studentClass => studentClass.GetClassTime() == streamClass.GetClassTime())))
            {
                throw new IntersectionInTimetable();
            }

            ++_countOfStudents;
            student.AddClasses(Timetable);
            student.SetCountOfJgtd(student.GetCountOfJgtd() + 1);
            _students.Add(student);
        }

        public void RemoveStudent(ComplementedStudent student)
        {
            if (student.GetCountOfJgtd() == 0 || !_students.Contains(student))
            {
                throw new StreamDoesNotIncludeStudent($"{MegaFaculty} stream doesn't include student");
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