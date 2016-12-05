using ChinhDo.Transactions;
using FCT.EPS.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.Resources
{
    public class BPSTransactionReportGenerator
    {
        public static string GenerateReport(IList<tblPaymentRequest> passedPaymentRequestList, string passedElectronicReportFileFolderPath)
        {
            StringBuilder myStringBuilder = new StringBuilder();
            TxFileManager fileMgr = new TxFileManager();
            //Add Header
            myStringBuilder.AppendLine("WireBatchNumber\tWireBatchAmount\tDisbursementRequestDate\tFCT URN Short (deal)\tPaymentTransactionID\tError\tPayeeName\tCompany ID\tBPSInputFileSequenceNumber\tCCIN #\tAmount");
            foreach (tblPaymentRequest mytblPaymentRequest in passedPaymentRequestList)
            {
                myStringBuilder.AppendLine
                    (
                        mytblPaymentRequest.tblPaymentTransaction.tblServiceBatch.tblServiceBatchStatus.ServiceBatchID + "\t" +
                        passedPaymentRequestList.Sum(c => c.PaymentAmount) + "\t" +
                        mytblPaymentRequest.PaymentRequestDate.ToString("yyyyMMdd") + "\t" +
                        mytblPaymentRequest.FCTURNShort + "\t" +
                        mytblPaymentRequest.PaymentTransactionID + "\t" +
                        (mytblPaymentRequest.tblPaymentTransaction.StatusID == Constants.DataBase.Tables.tblEPSStatus.RBCBPSError ? "ERROR" : "") + "\t" +
                        mytblPaymentRequest.PayeeName + "\t" +
                        mytblPaymentRequest.tblPayeeInfo.tblCreditorList.CompanyID + "\t" +
                        mytblPaymentRequest.tblPaymentTransaction.tblServiceBatch.tblServiceBatchStatus.BPSSequenceNumber + "\t" +
                        mytblPaymentRequest.tblPayeeInfo.tblCreditorList.CCIN + "\t" +
                        mytblPaymentRequest.PaymentAmount
                    );
            }

            DateTime myDateTime = DateTime.Now;
            string myFileName = "ElectronicDelivery_EasyFund_PaymentDetails_" + myDateTime.ToString("yyyyMMdd_HHmmss") + ".csv";
            string myFilePath = System.IO.Path.Combine(new string[] { passedElectronicReportFileFolderPath, myDateTime.Year.ToString("0000"), myDateTime.Month.ToString("00"), myDateTime.Day.ToString("00") });
            string myFullNameAndPath = System.IO.Path.Combine(myFilePath, myFileName);

            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            fileMgr.CreateDirectory(myFilePath);
            fileMgr.WriteAllBytes(myFullNameAndPath, ascii.GetBytes(myStringBuilder.ToString()));

            return myFullNameAndPath;
        }

    }
}
