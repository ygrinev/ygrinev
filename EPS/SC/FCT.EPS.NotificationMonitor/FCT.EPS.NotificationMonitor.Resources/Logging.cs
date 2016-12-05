using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FCT.EPS.NotificationMonitor.Resources
{
    public static class Logging
    {

        ///<summary>
        ///</summary>
        ///<param name="message"></param>
        public static void LogAuditingActivity(Exception ex, int eventID, TraceEventType severity, string passedTitle, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            if (ex.InnerException != null)
                LogAuditingActivity(ex.InnerException, eventID, severity, passedTitle);

            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                string localMessage = ex.Message + AdditionalExceptionErrors(ex);

                LogEntry myLogEntry = new LogEntry { EventId = eventID, Severity = severity, Message = String.Format("Exception Type = '{1}'.  Message = '{0}'", localMessage, ex.GetType().ToString()), Title = passedTitle, ExtendedProperties = CreateDictionaryOfString(new string[] { "passedMemberName=" + passedMemberName, "passedSourceFilePath=" + passedSourceFilePath, "passedSourceLineNumber=" + passedSourceLineNumber.ToString() }) };

                myLogEntry.Categories.Add("Auditing");

                Logger.Write(myLogEntry);
            }
        }
        public static void LogAuditingActivity(string message, int eventID, TraceEventType severity, string passedTitle, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                LogEntry myLogEntry = new LogEntry { EventId = eventID, Severity = severity, Message = message, Title = passedTitle, ExtendedProperties = CreateDictionaryOfString(new string[] { "passedMemberName=" + passedMemberName, "passedSourceFilePath=" + passedSourceFilePath, "passedSourceLineNumber=" + passedSourceLineNumber.ToString() }) };

                myLogEntry.Categories.Add("Auditing");

                Logger.Write(myLogEntry);
            }
        }
        public static void LogWarningsActivity(string message, int eventID, TraceEventType severity, string passedTitle, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                LogEntry myLogEntry = new LogEntry { EventId = eventID, Severity = severity, Message = message, Title = passedTitle, ExtendedProperties = CreateDictionaryOfString(new string[] { "passedMemberName=" + passedMemberName, "passedSourceFilePath=" + passedSourceFilePath, "passedSourceLineNumber=" + passedSourceLineNumber.ToString() }) };

                myLogEntry.Categories.Add("Warning");

                Logger.Write(myLogEntry);
            }
        }
        public static void LogWarningsActivity(Exception ex, int eventID, TraceEventType severity, string passedTitle, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            if (ex.InnerException != null)
                LogWarningsActivity(ex.InnerException, eventID, severity, passedTitle);

            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                string localMessage = ex.Message + AdditionalExceptionErrors(ex);

                LogEntry myLogEntry = new LogEntry { EventId = eventID, Severity = severity, Message = String.Format("Exception Type = '{1}'.  Message = '{0}'", localMessage, ex.GetType().ToString()), Title = passedTitle, ExtendedProperties = CreateDictionaryOfString(new string[] { "passedMemberName=" + passedMemberName, "passedSourceFilePath=" + passedSourceFilePath, "passedSourceLineNumber=" + passedSourceLineNumber.ToString() }) };

                myLogEntry.Categories.Add("Warning");

                Logger.Write(myLogEntry);
            }
        }
        public static void LogErrorActivity(string message, int eventID, TraceEventType severity, string passedTitle, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                LogEntry myLogEntry = new LogEntry { EventId = eventID, Severity = severity, Message = message, Title = passedTitle, ExtendedProperties = CreateDictionaryOfString(new string[] { "passedMemberName=" + passedMemberName, "passedSourceFilePath=" + passedSourceFilePath, "passedSourceLineNumber=" + passedSourceLineNumber.ToString() }) };

                myLogEntry.Categories.Add("Error");

                Logger.Write(myLogEntry);
            }
        }
        public static void LogErrorActivity(Exception ex, int eventID, TraceEventType severity, string passedTitle, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            if (ex.InnerException != null)
                LogErrorActivity(ex.InnerException, eventID, severity, passedTitle);

            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                string localMessage = ex.Message + AdditionalExceptionErrors(ex);

                LogEntry myLogEntry = new LogEntry { EventId = eventID, Severity = severity, Message = String.Format("Exception Type = '{1}'.  Message = '{0}'", localMessage, ex.GetType().ToString()), Title = passedTitle, ExtendedProperties = CreateDictionaryOfString(new string[] { "passedMemberName=" + passedMemberName, "passedSourceFilePath=" + passedSourceFilePath, "passedSourceLineNumber=" + passedSourceLineNumber.ToString() }) };

                myLogEntry.Categories.Add("Error");

                Logger.Write(myLogEntry);
            }
        }
        public static void LogCriticalActivity(string message, int eventID, TraceEventType severity, string passedTitle, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                LogEntry myLogEntry = new LogEntry { EventId = eventID, Severity = severity, Message = message, Title = passedTitle, ExtendedProperties = CreateDictionaryOfString(new string[] { "passedMemberName=" + passedMemberName, "passedSourceFilePath=" + passedSourceFilePath, "passedSourceLineNumber=" + passedSourceLineNumber.ToString() }) };

                myLogEntry.Categories.Add("Critical");

                Logger.Write(myLogEntry);
            }
        }
        public static void LogCriticalActivity(Exception ex, int eventID, TraceEventType severity, string passedTitle, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            if (ex.InnerException != null)
                LogCriticalActivity(ex.InnerException, eventID, severity, passedTitle);

            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                string localMessage = ex.Message + AdditionalExceptionErrors(ex);

                LogEntry myLogEntry = new LogEntry { EventId = eventID, Severity = severity, Message = String.Format("Exception Type = '{1}'.  Message = '{0}'", localMessage, ex.GetType().ToString()), Title = passedTitle, ExtendedProperties = CreateDictionaryOfString(new string[] { "passedMemberName=" + passedMemberName, "passedSourceFilePath=" + passedSourceFilePath, "passedSourceLineNumber=" + passedSourceLineNumber.ToString() }) };

                myLogEntry.Categories.Add("Critical");

                Logger.Write(myLogEntry);
            }
        }


        private static string AdditionalExceptionErrors(Exception ex)
        {
            string localMessage = string.Empty;
            if (ex.GetType() == typeof(ReflectionTypeLoadException))
            {
                localMessage += "ReflectionTypeLoadException errors -> " + GetReflectionTypeLoadExceptionDetails((ReflectionTypeLoadException)ex);
            }
            return localMessage;
        }

        private static string GetReflectionTypeLoadExceptionDetails(ReflectionTypeLoadException ex)
        {
            string localMessage = string.Empty;
            foreach (Exception myException in ex.LoaderExceptions)
            {
                    localMessage += "->" + myException.Message;
            }
            return localMessage;
        }

        private static IDictionary<string, object> CreateDictionaryOfString(string[] passedStrings)
        {
            IDictionary<string, object> myDictionary = new Dictionary<string, object>();
            if (passedStrings != null)
            {
                foreach (string tempString in passedStrings)
                {
                    myDictionary.Add(tempString.GetHashCode().ToString(), tempString);
                }
            }
            return myDictionary;
        }

    }
}
