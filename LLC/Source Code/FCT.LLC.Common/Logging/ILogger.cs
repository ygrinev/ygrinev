using System;

namespace FCT.LLC.Logging
{
    public interface ILogger
    {
        void LogError(Exception ex);

        void LogError(Exception ex, string message);

        void LogError(string message);

        void LogUnhandledError(Exception ex);

        void LogWarning(string message);

        void LogFormattedWarning(string message, params object[] args);
    }
}
