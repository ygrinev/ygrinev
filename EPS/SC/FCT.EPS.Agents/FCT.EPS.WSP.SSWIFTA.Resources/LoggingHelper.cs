using System;
using System.Diagnostics;
using FCT.EPS.WSP.Resources;

namespace FCT.EPS.WSP.SSWIFTA.Resources
{
    public class LoggingHelper : Logging
    {
        public static void LogAuditingActivity(string logMessage, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogAuditingActivity(logMessage, AgentConstants.EventID.SEND_SINGLE_WIRE_SWIFT_AGENT, TraceEventType.Information, AgentConstants.Misc.LOGGING_APPLICATION_TITLE,passedMemberName,passedSourceFilePath,passedSourceLineNumber);
        }

        public static void LogAuditingActivity(string logMessage, Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogAuditingActivity(new Exception(logMessage, ex), AgentConstants.EventID.SEND_SINGLE_WIRE_SWIFT_AGENT, TraceEventType.Information, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        public static void LogCriticalActivity(string logMessage, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogCriticalActivity(new Exception(logMessage), AgentConstants.EventID.SEND_SINGLE_WIRE_SWIFT_AGENT, TraceEventType.Critical, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        public static void LogCriticalActivity(string logMessage, Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogCriticalActivity(new Exception(logMessage, ex), AgentConstants.EventID.SEND_SINGLE_WIRE_SWIFT_AGENT, TraceEventType.Critical, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        public static void LogCriticalActivity(Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)        
        {
            LogCriticalActivity(ex, AgentConstants.EventID.SEND_SINGLE_WIRE_SWIFT_AGENT, TraceEventType.Critical, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        public static void LogWarningActivity(string logMessage, Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogWarningsActivity(new Exception(logMessage, ex), AgentConstants.EventID.SEND_SINGLE_WIRE_SWIFT_AGENT, TraceEventType.Warning, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }
        public static void LogWarningActivity(Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogWarningsActivity(ex, AgentConstants.EventID.SEND_SINGLE_WIRE_SWIFT_AGENT, TraceEventType.Warning, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        public static void LogWarningActivity(string logMessage, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogWarningsActivity(new Exception(logMessage), AgentConstants.EventID.SEND_SINGLE_WIRE_SWIFT_AGENT, TraceEventType.Warning, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        #region Error

        public static void LogErrorActivity(string logMessage, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogErrorActivity(new Exception(logMessage), AgentConstants.EventID.SEND_SINGLE_WIRE_SWIFT_AGENT, TraceEventType.Error, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, AgentConstants.Misc.LOGGING_BUSINESS_RULES_PRIORITY, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        public static void LogErrorActivity(string logMessage, Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogErrorActivity(new Exception(logMessage, ex), AgentConstants.EventID.SEND_SINGLE_WIRE_SWIFT_AGENT, TraceEventType.Error, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, AgentConstants.Misc.LOGGING_BUSINESS_RULES_PRIORITY, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        public static void LogErrorActivity(Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogErrorActivity(ex, AgentConstants.EventID.SEND_SINGLE_WIRE_SWIFT_AGENT, TraceEventType.Error, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, AgentConstants.Misc.LOGGING_BUSINESS_RULES_PRIORITY, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        #endregion

    }
}
