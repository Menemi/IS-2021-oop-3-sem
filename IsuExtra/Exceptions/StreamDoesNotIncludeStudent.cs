namespace IsuExtra.Exceptions
{
    public class StreamDoesNotIncludeStudent : Isu.Tools.IsuException
    {
        public StreamDoesNotIncludeStudent(string message)
            : base(message)
        {
        }
    }
}