using System;
using System.Collections.Generic;
using Isu.Tools;

namespace Isu.Services
{
    public class Group
    {
        private int _countOfStudents = 0;
        private static readonly int MaxCountOfStudents = 20;
        public string GroupName { get; set; }
        public List<Student> _students;

        public CourseNumber CourseNumber { get; }

        public Group(string groupName)
        {
            if (groupName.Length != 5)
            {
                throw new GroupNameLengthException();
            }

            if (groupName[2] < 1 || groupName[2] > 4)
            {
                throw new InvalidCourseNumberException();
            }

            int tempInt;
            if (!char.IsUpper(groupName[0]) || !int.TryParse(groupName.Substring(1), out tempInt))
            {
                throw new InvalidGroupNameException();
            }

            GroupName = groupName;
            CourseNumber = new CourseNumber(groupName[2]);
            _students = new List<Student>();
        }

        public void AddStudent(Student student)
        {
            if (_countOfStudents >= MaxCountOfStudents)
            {
                throw new MaxStudentsIsuException();
            }

            ++_countOfStudents;
            _students.Add(student);
        }

        private bool RemoveStudent(Student student)
        {
            return _students.Remove(student);
        }

        public void MoveStudent(Student student, Group oldGroup)
        {
            oldGroup.RemoveStudent(student);
            AddStudent(student);
        }
    }
}