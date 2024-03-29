﻿using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private static readonly List<Group> _listGroup = new List<Group>();

        public Group AddGroup(string name)
        {
            var newGroup = new Group(name);
            _listGroup.Add(newGroup);
            return newGroup;
        }

        public Student AddStudent(Group group, string name) // void -> Student
        {
            var newStudent = new Student(name, group);
            group.AddStudent(newStudent);
            return newStudent;
        }

        public Student GetStudent(int id)
        {
            foreach (Group group in _listGroup)
            {
                foreach (Student student in group.Students)
                {
                    if (student.Id == id)
                    {
                        return student;
                    }
                }
            }

            throw new StudentWasNotFound();
        }

        public Student FindStudent(string name)
        {
            return FindStudentByPredicate(student => student.Name == name, true);
        }

        public Student FindStudentByPredicate(Func<Student, bool> pred, bool shouldThrow)
        {
            foreach (Group group in _listGroup)
            {
                foreach (Student student in group.Students)
                {
                    if (pred(student))
                    {
                        return student;
                    }
                }
            }

            if (shouldThrow)
            {
                throw new StudentWasNotFound();
            }

            return null;
        }

        public List<Student> FindStudents(string groupName)
        {
            foreach (Group group in _listGroup)
            {
                if (group.GroupName == groupName)
                    return group.Students;
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
                    foreach (Student student in group.Students)
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
            return _listGroup.Where(gr => gr.CourseNumber == courseNumber).ToList();
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            foreach (Group group in _listGroup)
            {
                if (group == student.Group)
                {
                    newGroup.MoveStudent(student, group);
                    return;
                }
            }
        }
    }
}