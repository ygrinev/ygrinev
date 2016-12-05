using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.DataEntities;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.SRA.Resources;

namespace FCT.EPS.WSP.SRA.BusinessLogic
{
    public class Mapper
    {

        internal static string ReplaceStringValues(string passedStringValue, IList<tblBatchPaymentReportInfo> passedReportData)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            IList<tblBatchPaymentReportInfo> distinctReportData = passedReportData.GroupBy(c => c.FieldName).Select(grp => grp.First()).ToList();
            foreach (tblBatchPaymentReportInfo KeyValuePair in distinctReportData)
            {
                try
                {
                    passedStringValue = passedStringValue.Replace(KeyValuePair.FieldName, KeyValuePair.FieldValue);
                }
                catch (Exception ex)
                {
                    SolutionTraceClass.WriteLineWarning(String.Format("Exception while formating report value. Message was->{0}", ex.Message));
                    LoggingHelper.LogWarningActivity("Exception while formating report value.", ex);
                }
            }

            SolutionTraceClass.WriteLineVerbose("End");
            return passedStringValue;
        }

    }
}
