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
            Assert.Contains(tempStudent, tempGroup.Students);
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
            Group tempGroup1 = _isuService.AddGroup("M3100");
            Group tempGroup2 = _isuService.AddGroup("M3101");
            Student tempStudent = _isuService.AddStudent(tempGroup1, "Petr");
            _isuService.ChangeStudentGroup(tempStudent, tempGroup2);
            Assert.Contains(tempStudent, tempGroup1.Students);
        }
    }
}