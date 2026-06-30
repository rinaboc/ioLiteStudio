using System;
using System.IO;
using System.Threading.Tasks.Dataflow;
using Serilog;
using Serilog.Sinks.Unity3D;
using UnityEngine;

namespace Core.Services
{
    public interface ILogService
    {
        void LogInfo(string msg, object property = null);
        void LogWarning(string msg, object property = null);
        void LogError(string msg, Exception ex = null);
    }

    public class LogService : ILogService, IDisposable
    {
        private readonly Serilog.ILogger _logger;
        private readonly ActionBlock<Action> _loggingBlock;

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
            _loggingBlock = new(action => action?.Invoke(), new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });
        }

        public void Dispose()
        {
            _loggingBlock.Complete();
            Log.CloseAndFlush();
        }

        public void LogError(string msg, Exception ex = null)
        {
            _loggingBlock.Post(() =>
            {
                if (ex != null) _logger.Error(ex, msg);
                else _logger.Error(msg);
            });
        }

        public void LogInfo(string msg, object property = null)
        {
            _loggingBlock.Post(() =>
            {
                if (property != null) _logger.Information(msg + " {@Data}", property);
                else _logger.Information(msg);
            });
        }

        public void LogWarning(string msg, object property = null)
        {
            _loggingBlock.Post(() =>
            {
                if (property != null) _logger.Warning(msg + " {@Data}", property);
                else _logger.Warning(msg);
            });
        }
    }
}
