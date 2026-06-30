using System;

namespace Core.Domain
{
    public interface ILogService
    {
        void LogInfo(string msg, object property = null);
        void LogWarning(string msg, object property = null);
        void LogError(string msg, Exception ex = null);
    }
}