namespace IsuExtra.Exceptions
{
    public class LimitedNumberOfPlacesException : Isu.Tools.IsuException
    {
        public LimitedNumberOfPlacesException(string message)
            : base(message)
        {
        }
    }
}