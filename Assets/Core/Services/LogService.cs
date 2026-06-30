using System;
using System.IO;
using Core.Domain;
using Serilog;
using Serilog.Sinks.Unity3D;
using UnityEngine;

namespace Core.Services
{
    public class LogService : ILogService
    {
        private readonly Serilog.ILogger _logger;

        public LogService()
        {
            string logfilePath = Path.Combine(Application.persistentDataPath, "logs", "app_log.txt");

            var logConfig = new LoggerConfiguration().MinimumLevel.Information();
            logConfig.WriteTo.File(
                logfilePath,
                rollingInterval: RollingInterval.Hour,
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
            );

#if UNITY_EDITOR
            logConfig.WriteTo.Unity3D(
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
            );
#endif
            _logger = logConfig.CreateLogger();
        }

        public void Dispose()
        {
            Log.CloseAndFlush();
        }

        public void LogError(string msg, Exception ex = null)
        {
            ServiceLocator.Get<IThreadPoolService>().Post(() =>
            {
                if (ex != null) _logger.Error(ex, msg);
                else _logger.Error(msg);
            });
        }

        public void LogInfo(string msg, object property = null)
        {
            ServiceLocator.Get<IThreadPoolService>().Post(() =>
            {
                if (property != null) _logger.Information(msg + " {@Data}", property);
                else _logger.Information(msg);
            });
        }

        public void LogWarning(string msg, object property = null)
        {
            ServiceLocator.Get<IThreadPoolService>().Post(() =>
            {
                if (property != null) _logger.Warning(msg + " {@Data}", property);
                else _logger.Warning(msg);
            });
        }
    }
}
