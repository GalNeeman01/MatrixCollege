using Microsoft.Extensions.Options;
using Serilog;

namespace Matrix;

public class LogCleanerService : BackgroundService
{
    private LogSettings _logSettings;

    public LogCleanerService(IOptions<LogSettings> options)
    {
        _logSettings = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Set clean-up interval
        TimeSpan cleanUpInterval = new TimeSpan(_logSettings.CleanupInterval, 0, 0, 0); // 1 day

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Clean logs
                CleanLogs(); // Remove week old logs
            }
            catch (Exception e)
            {
                Log.Error("An error has occured while trying to remove a log file:\n" + e.Message);
            }

            // Wait until next cleanup date
            await Task.Delay(cleanUpInterval);
        }
    }

    private void CleanLogs()
    {
        if (!Directory.Exists(_logSettings.DirectoryPath))
        {
            Log.Information("No logs directory detected. Skipping log cleanup.");
        }
        else
        {
            string[] logNames = Directory.GetFiles(_logSettings.DirectoryPath);

            foreach (string logName in logNames)
            {
                FileInfo logFileInfo = new FileInfo(logName);

                // Remove if older than max age
                if (logFileInfo.CreationTime <DateTime.Now.AddDays(-_logSettings.LogRetentionDays))
                {
                    Log.Information("Removing old log: " + logName);
                    logFileInfo.Delete();
                }
            }
        }
    }


}
