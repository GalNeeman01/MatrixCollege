
using Serilog;

namespace Matrix;

public class LogCleanerService : BackgroundService
{
    private string _logsDirectoryPath = "logs";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Set clean-up delay
        TimeSpan cleanUpDelay = new TimeSpan(1, 0, 0, 0); // 1 day

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Clean logs
                CleanLogs(7); // Remove week old logs
            }
            catch (Exception e)
            {
                Log.Error("An error has occured while trying to remove a log file:\n" + e.Message);
            }

            // Wait until next cleanup date
            await Task.Delay(cleanUpDelay);
        }
    }

    private void CleanLogs(int maxDays)
    {
        if (!Directory.Exists(_logsDirectoryPath))
        {
            Log.Information("No logs directory detected. Skipping log cleanup.");
        }
        else
        {
            string[] logNames = Directory.GetFiles(_logsDirectoryPath);

            foreach (string logName in logNames)
            {
                FileInfo logFileInfo = new FileInfo(logName);

                // Remove if older than max age
                if (logFileInfo.CreationTime < DateTime.Now.AddDays(-maxDays))
                {
                    Log.Information("Removing old log: " + logName);
                    logFileInfo.Delete();
                }
            }
        }
    }


}
