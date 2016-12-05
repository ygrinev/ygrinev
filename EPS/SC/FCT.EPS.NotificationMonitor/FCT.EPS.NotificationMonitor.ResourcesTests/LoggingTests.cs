using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.WSP.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using FCT.EPS.NotificationMonitor.Resources;
namespace FCT.EPS.WSP.Resources.Tests
{
    [TestClass(), ExcludeFromCodeCoverage]
    public class LoggingTests
    {
        [ExcludeFromCodeCoverage, TestInitialize()]
        public void Initialize()
        {
            //DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
            IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();
            LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);
            Logger.SetLogWriter(logWriterFactory.Create(), false);
            //ExceptionPolicyFactory exceptionFactory = new ExceptionPolicyFactory(configurationSource);
            //ExceptionPolicy.SetExceptionManager(exceptionFactory.CreateManager());
        }


        [ExcludeFromCodeCoverage, TestCategory("Int"), TestMethod()]
        public void Logging_LogAuditingActivityTest()
        {
            Logging.LogAuditingActivity(new Exception(), Constants.EventID.NOTIFICATION_MONITOR, System.Diagnostics.TraceEventType.Information, Constants.Misc.TRACE_SWITCH_TITLE);
        }

        [ExcludeFromCodeCoverage, TestCategory("Int"), TestMethod()]
        public void Logging_LogWarningsActivityTest()
        {
            Logging.LogWarningsActivity(new Exception(), Constants.EventID.NOTIFICATION_MONITOR, System.Diagnostics.TraceEventType.Warning, Constants.Misc.TRACE_SWITCH_TITLE);
        }

        [ExcludeFromCodeCoverage, TestCategory("Int"), TestMethod()]
        public void Logging_LogCriticalActivityTest()
        {
            Logging.LogCriticalActivity(new Exception(), Constants.EventID.NOTIFICATION_MONITOR, System.Diagnostics.TraceEventType.Critical, Constants.Misc.TRACE_SWITCH_TITLE);
        }
    }
}
