using System.Collections.Generic;
using Isu.Tools;

namespace Isu.Services
{
    public class Group
    {
        private static readonly int MaxCountOfStudents = 20;
        private int _countOfStudents = 0;

        public Group(string groupName)
        {
            if (groupName.Length != 5)
            {
                throw new GroupNameLengthIsuException();
            }

            if (groupName[2] - '0' < 1 || groupName[2] - '0' > 4)
            {
                throw new InvalidCourseNumberIsuException();
            }

            int tempInt;
            if (!char.IsUpper(groupName[0]) || !int.TryParse(groupName.Substring(1), out tempInt))
            {
                throw new InvalidGroupNameException();
            }

            GroupName = groupName;
            CourseNumber = new CourseNumber(groupName[2] - '0');
            Students = new List<Student>();
        }

        public string GroupName { get; set; }

        public List<Student> Students { get; }

        public CourseNumber CourseNumber { get; }

        public void AddStudent(Student student)
        {
            if (_countOfStudents >= MaxCountOfStudents)
            {
                throw new MaxStudentsIsuException();
            }

            ++_countOfStudents;
            Students.Add(student);
        }

        public void MoveStudent(Student student, Group oldGroup)
        {
            oldGroup.RemoveStudent(student);
            AddStudent(student);
        }

        private bool RemoveStudent(Student student)
        {
            return Students.Remove(student);
        }
    }
}