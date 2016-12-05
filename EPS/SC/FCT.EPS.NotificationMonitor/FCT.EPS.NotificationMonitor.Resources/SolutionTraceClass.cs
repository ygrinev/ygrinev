#define TRACE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FCT.EPS.NotificationMonitor.Resources
{

    public static class SolutionTraceClass
    {
        private static TraceSwitch appTraceSwitch = new TraceSwitch(Constants.Misc.TRACE_SWITCH_TITLE, "GetChequeState Defined in config file.");

        public static void WriteLineError(string passedMessage, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "",[System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "",[System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            if (appTraceSwitch.TraceError)
            {
                Trace.WriteLine(FormatLine(passedMessage,passedMemberName,passedSourceFilePath,passedSourceLineNumber));
            }
        }
        public static void WriteLineWarning(string passedMessage, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            if (appTraceSwitch.TraceWarning)
            {
                Trace.WriteLine(FormatLine(passedMessage, passedMemberName, passedSourceFilePath, passedSourceLineNumber));
            }
        }
        public static void WriteLineInfo(string passedMessage, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            if (appTraceSwitch.TraceInfo)
            {
                Trace.WriteLine(FormatLine(passedMessage, passedMemberName, passedSourceFilePath, passedSourceLineNumber));
            }
        }
        public static void WriteLineVerbose(string passedMessage, [System.Runtime.CompilerServices.CallerMemberName] string passedMemberName = "", [System.Runtime.CompilerServices.CallerFilePath] string passedSourceFilePath = "", [System.Runtime.CompilerServices.CallerLineNumber] int passedSourceLineNumber = 0)
        {
            if (appTraceSwitch.TraceVerbose)
            {
                Trace.WriteLine(FormatLine(passedMessage, passedMemberName, passedSourceFilePath, passedSourceLineNumber));
            }
        }

        private static string FormatLine(string passedMessage,string passedMemberName, string passedSourceFilePath, int passedSourceLineNumber)
        {
            return String.Format("Time: {4}. Member Name: {1}. Message: {0}. Source File Path: {2}. Source Line Number: {3}.", passedMessage, passedMemberName, passedSourceFilePath, passedSourceLineNumber,DateTime.Now);
        }
    }
}
