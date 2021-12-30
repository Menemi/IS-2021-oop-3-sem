namespace BackupsExtra.Logging
{
    public interface ILogging
    {
        void CreateLog(bool isTimecodeOn, string message);
    }
}