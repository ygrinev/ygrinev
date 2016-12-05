using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace LoggingTestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger defaultWriter
          = //EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();
          new EnterpriseLibraryLogger();
            //if (defaultWriter.IsLoggingEnabled())
            //{
            //    // Create a Dictionary of extended properties
            //    Dictionary<string, object> exProperties = new Dictionary<string, object>();
            //    exProperties.Add("Extra Information", "Some Special Value");

            //    // Create a LogEntry using the constructor parameters.
            //    defaultWriter.Write("Log entry with category, priority, event ID, severity, "
            //                        + "title, and extended properties.", "Database",
            //                        5, 9008, TraceEventType.Error,
            //                        "Logging Block Examples", exProperties);

            //    // Create a LogEntry using the constructor parameters. 
            //    LogEntry entry = new LogEntry("LogEntry with category, priority, event ID, "
            //                                  + "severity, title, and extended properties.",
            //                                  "Database", 8, 9009, TraceEventType.Error,
            //                                  "Logging Block Examples", exProperties);
            //    defaultWriter.Write(entry);
            //}
            //else
            //{
            //    Console.WriteLine("Logging is disabled in the configuration.");
            //}
            defaultWriter.LogError("testing 1");
        }
    }
}
