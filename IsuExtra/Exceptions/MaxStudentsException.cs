namespace IsuExtra.Exceptions
{
    public class MaxStudentsException : Isu.Tools.IsuException
    {
        public MaxStudentsException(string message)
            : base(message)
        {
        }
    }
}