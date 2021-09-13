using System.Collections.Generic;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private readonly List<Group> _listGroup;

        public IsuService()
        {
            _listGroup = new List<Group>();
        }

        public Group AddGroup(string name)
        {
            var newGroup = new Group(name);
            return newGroup;
        }

        public Student AddStudent(Group group, string name) // void -> Student
        {
            var newStudent = new Student(name, group.GroupName);
            group.AddStudent(newStudent);
            return newStudent;
        }

        public Student GetStudent(int id)
        {
            foreach (Group group in _listGroup)
            {
                foreach (Student student in group._students)
                {
                    if (student.Id == id)
                        return student;
                }
            }

            return null;
        }

        public Student FindStudent(string name)
        {
            foreach (Group group in _listGroup)
            {
                foreach (Student student in group._students)
                {
                    if (student.Name == name)
                        return student;
                }
            }

            return null;
        }

        public List<Student> FindStudents(string groupName)
        {
            foreach (Group group in _listGroup)
            {
                if (group.GroupName == groupName)
                    return group._students;
            }

            return null;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            var allStudents = new List<Student>();
            foreach (Group group in _listGroup)
            {
                if (group.CourseNumber == courseNumber)
                {
                    foreach (Student student in group._students)
                        allStudents.Add(student);
                }
            }

            return allStudents;
        }

        public Group FindGroup(string groupName)
        {
            foreach (Group group in _listGroup)
            {
                if (group.GroupName == groupName)
                    return group;
            }

            return null;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            var tempListGroup = new List<Group>();
            foreach (Group group in _listGroup)
            {
                if (group.CourseNumber == courseNumber)
                    tempListGroup.Add(group);
            }

            return tempListGroup;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            foreach (Group group in _listGroup)
            {
                if (group.GroupName == student.GroupName)
                {
                    newGroup.MoveStudent(student, group);
                    return;
                }
            }
        }
    }
}