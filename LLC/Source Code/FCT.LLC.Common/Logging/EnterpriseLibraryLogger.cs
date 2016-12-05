using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;


namespace FCT.LLC.Logging
{
    public class EnterpriseLibraryLogger:ILogger
    {
        private readonly LogWriter _logWriter;
        private LogEntry _logEntry;

        public EnterpriseLibraryLogger()
        {

          _logWriter=  EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();
        }

        public void LogError(Exception ex)
        {
            var sb = new StringBuilder();
            _logEntry = new LogEntry {TimeStamp = DateTime.Now, Severity = TraceEventType.Error, Message = ex.Message};
            sb.AppendFormat("{0}; {1}", ex.Message, ex.StackTrace);

            if (ex.InnerException != null)
            {
                FormatInnerException(ex.InnerException, sb);
            }

            _logEntry.Message = sb.ToString();

            _logWriter.Write(_logEntry);
        }

        public void LogError(Exception ex, string message)
        {
            _logEntry = new LogEntry { TimeStamp = DateTime.Now, Severity = TraceEventType.Error, Message = message};
            LogErrorWithMessage(ex, message);
        }

        private void LogErrorWithMessage(Exception ex, string message)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}; {1}", message, ex.StackTrace);

            if (ex.InnerException != null)
            {
                FormatInnerException(ex.InnerException, sb);
            }

            _logEntry.Message = sb.ToString();

            _logWriter.Write(_logEntry);
        }

        public void LogError(string message)
        {
            _logWriter.Write(message);
        }

        public void LogUnhandledError(Exception ex)
        {
            const string unhandledConst = "Unhandled Exception";
            string message = string.Format("{0}:{1}", unhandledConst, ex.Message);
            _logEntry = new LogEntry { TimeStamp = DateTime.Now, Severity = TraceEventType.Error, Message = message };
            LogErrorWithMessage(ex, message);
        }

        public void LogWarning(string message)
        {
            throw new NotImplementedException();
        }

        public void LogFormattedWarning(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        private void FormatInnerException(Exception e, StringBuilder sb)
        {
            if (e != null)
            {
                sb.AppendFormat("Inner Exception: {0};{1};", e.Message, e.StackTrace);

                if (e.InnerException != null)
                {
                    FormatInnerException(e.InnerException, sb);
                }
            }
        }
    }

}
