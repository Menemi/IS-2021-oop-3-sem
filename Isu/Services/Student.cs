namespace Isu.Services
{
    public class Student
    {
        private static int _idCounter = 1;

        public Student(string name, Group group)
        {
            Name = name;
            Group = group;
            Id = _idCounter++;
        }

        public string Name { get; set; }

        public int Id { get; set; }

        public Group Group { get; }
    }
}