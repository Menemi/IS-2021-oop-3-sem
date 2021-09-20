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

        public virtual bool Equals(Group leftGroup, Group rightGroup)
        {
            return leftGroup.GroupName == rightGroup.GroupName || leftGroup.Students == rightGroup.Students;
        }

        private bool RemoveStudent(Student student)
        {
            return Students.Remove(student);
        }
    }
}