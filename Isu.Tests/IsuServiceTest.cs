using System.Collections.Generic;
using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IIsuService _isuService;

        [SetUp]
        public void Setup()
        {
            //fixed: implement
            _isuService = new IsuService();
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Group tempGroup = _isuService.AddGroup("M3204");
            Student tempStudent = _isuService.AddStudent(tempGroup, "Titov Daniil Yaroslavovich");

            if (tempStudent.GroupName != _isuService.FindGroup("M3204").GroupName)
            {
                Assert.Fail("Group was not found!");
            }

            foreach (Student student in _isuService.FindStudents(tempStudent.GroupName))
            {
                if (student == tempStudent)
                    return;
            }

            Assert.Fail("Student was not found!");
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                Group tempGroup = _isuService.AddGroup("M3205");
                for (int i = 0; i < 22; i++)
                {
                    _isuService.AddStudent(tempGroup, "Ivanov Ivan Ivanovich" + i);
                }
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                var invalidGroupNameFirst = new Group("Sheeeeeesh");
                var invalidCourseNumber = new Group("M3604");
                var invalidGroupNameLengthFirst = new Group("M32044");
                var invalidGroupNameSecond = new Group("M32O4"); // m32o4
                var invalidGroupNameLengthSecond = new Group("M320");
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            Assert.Catch<IsuException>(() =>
            {
            });
        }
    }
}