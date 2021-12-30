using Serilog;

namespace BackupsExtra.Logging
{
    public class FileLogging : ILogging
    {
        public void CreateLog(bool isTimecodeOn, string message)
        {
            const string path = @"D:\ITMOre than a university\1Menemi1\BackupsExtra\log.txt";
            if (isTimecodeOn)
            {
                using var logger = new LoggerConfiguration()
                    .WriteTo.File(path, outputTemplate: "{Timestamp:yyyy.MM.dd HH:mm:ss}: {Message}{NewLine}{Exception}")
                    .CreateLogger();
                logger.Information(message);
            }
            else
            {
                using var logger = new LoggerConfiguration()
                    .WriteTo.File(path, outputTemplate: "{Message}{NewLine}{Exception}")
                    .CreateLogger();
                logger.Information(message);
            }
        }
    }
}