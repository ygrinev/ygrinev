using FCT.EPS.WSP.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FCT.EPS.WSP.GEDMA.Resources
{
    public class LoggingHelper : Logging
    {
        #region Audit

        public static void LogAuditingActivity(string logMessage, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogAuditingActivity(logMessage, AgentConstants.EventID.GET_SWIFT_AGENT, TraceEventType.Information, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        public static void LogAuditingActivity(string logMessage, Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogAuditingActivity(new Exception(logMessage, ex), AgentConstants.EventID.GET_SWIFT_AGENT, TraceEventType.Information, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        #endregion

        #region Critical

        public static void LogCriticalActivity(string logMessage, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogCriticalActivity(new Exception(logMessage), AgentConstants.EventID.GET_SWIFT_AGENT, TraceEventType.Critical, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        public static void LogCriticalActivity(string logMessage, Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogCriticalActivity(new Exception(logMessage, ex), AgentConstants.EventID.GET_SWIFT_AGENT, TraceEventType.Critical, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        public static void LogCriticalActivity(Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogCriticalActivity(ex, AgentConstants.EventID.GET_SWIFT_AGENT, TraceEventType.Critical, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        #endregion
        #region Error

        public static void LogErrorActivity(string logMessage, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogErrorActivity(new Exception(logMessage), AgentConstants.EventID.GET_SWIFT_AGENT, TraceEventType.Error, AgentConstants.Misc.LOGGING_APPLICATION_TITLE,AgentConstants.Misc.LOGGING_BUSINESS_RULES_PRIORITY, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        public static void LogErrorActivity(string logMessage, Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogErrorActivity(new Exception(logMessage, ex), AgentConstants.EventID.GET_SWIFT_AGENT, TraceEventType.Error, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, AgentConstants.Misc.LOGGING_BUSINESS_RULES_PRIORITY, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        public static void LogErrorActivity(Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogErrorActivity(ex, AgentConstants.EventID.GET_SWIFT_AGENT, TraceEventType.Error, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, AgentConstants.Misc.LOGGING_BUSINESS_RULES_PRIORITY, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        #endregion

        #region Warning

        public static void LogWarningActivity(string logMessage, Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogWarningsActivity(new Exception(logMessage, ex), AgentConstants.EventID.GET_SWIFT_AGENT, TraceEventType.Warning, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }
        public static void LogWarningActivity(Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogWarningsActivity(ex, AgentConstants.EventID.GET_SWIFT_AGENT, TraceEventType.Warning, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        public static void LogWarningActivity(string logMessage, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            LogWarningsActivity(new Exception(logMessage), AgentConstants.EventID.GET_SWIFT_AGENT, TraceEventType.Warning, AgentConstants.Misc.LOGGING_APPLICATION_TITLE, passedMemberName, passedSourceFilePath, passedSourceLineNumber);
        }

        #endregion
    }
}
