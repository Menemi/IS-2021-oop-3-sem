using Serilog;

namespace BackupsExtra.Logging
{
    public class ConsoleLogging : ILogging
    {
        public void CreateLog(bool isTimecodeOn, string message)
        {
            if (isTimecodeOn)
            {
                using var logger = new LoggerConfiguration()
                    .WriteTo.Console(outputTemplate: "{Timestamp:yy.MM.dd HH:mm:ss}: {Message}{NewLine}{Exception}")
                    .CreateLogger();
                logger.Information(message);
            }
            else
            {
                using var logger = new LoggerConfiguration()
                    .WriteTo.Console(outputTemplate: "{Message}{NewLine}{Exception}")
                    .CreateLogger();
                logger.Information(message);
            }
        }
    }
}