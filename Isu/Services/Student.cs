namespace Isu.Services
{
    public class Student
    {
        private static int _idCounter = 1;

        public Student(string name, string groupName)
        {
            Name = name;
            Id = _idCounter++;
            GroupName = groupName;
        }

        public string Name { get; set; }
        public int Id { get; set; }
        public string GroupName { get; set; }
    }
}