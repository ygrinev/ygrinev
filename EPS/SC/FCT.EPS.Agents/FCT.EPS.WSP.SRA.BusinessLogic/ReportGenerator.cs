using Aspose.Cells;
using FCT.EPS.DataEntities;
using FCT.EPS.WSP.DataAccess;
using FCT.EPS.WSP.ExternalResources;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.SRA.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Document = Aspose.Cells.Workbook;

namespace FCT.EPS.WSP.SRA.BusinessLogic
{
    public class ReportGenerator : IDisposable
    {
        public class MappedList
        {
            public DataEntities.tblBatchPaymentReportInfo ValueData = null;
            public DataEntities.tblPaymentReportFields MetaData = null;
        }

        // Flag: Has Dispose already been called? 
        bool disposed = false;
        private UnitOfWork myUnitOfWork = null;
        private NonGeneric myNonGeneric = null;
        private IList<IEnumerable<MappedList>> myMappedData;
        private int RowsBetweenHeaderAndData = 0;
        private string FileArchiveLocation;
        private string DMSWrkDirLocation;


        private ReportGenerator()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            this.myUnitOfWork = new UnitOfWork();
            this.myNonGeneric = new NonGeneric(myUnitOfWork);
            SolutionTraceClass.WriteLineVerbose("End");
        }

        public ReportGenerator(int passedRowsBetweenHeaderAndData, string passedFileArchiveLocation, string passedDMSWrkDirLocation)
            : this()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            // TODO: Complete member initialization
            this.RowsBetweenHeaderAndData = passedRowsBetweenHeaderAndData;
            this.FileArchiveLocation = passedFileArchiveLocation;
            this.DMSWrkDirLocation = passedDMSWrkDirLocation;
            SolutionTraceClass.WriteLineVerbose("End");
        }


        public void GenerateReports(int passedhowManyPaymentTransactionRecordsToGet, int passedmaxAllowedRetry)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            int? lastPaymentTransactionID = 0;
            bool stillMoreRecords = true;
            try
            {
                while (stillMoreRecords)
                {
                    stillMoreRecords = false;
                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                    {

                        //get report records from EPS database
                        IList<tblPaymentReport> ReportStatusRecords = GetEPSResportStatusRecords(lastPaymentTransactionID, passedhowManyPaymentTransactionRecordsToGet);
                        foreach (tblPaymentReport mytblPaymentReport in ReportStatusRecords)
                        {
                            LoggingHelper.LogAuditingActivity(string.Format("About to produce report for mytblPaymentReport with NotificationID={0}. ", mytblPaymentReport.NotificationID));

                            try
                            {
                                stillMoreRecords = true;
                                lastPaymentTransactionID = mytblPaymentReport.tblPaymentNotification.tblPaymentTransaction.PaymentTransactionID;

                                //get report METADATA from EPS database this include a ref to a DB record that enables us to get the 
                                //subject, email recipients, spread sheet fields.
                                tblPaymentReportInfo METADATARecords = GetEPSResportMETADATARecord((int)mytblPaymentReport.tblPaymentNotification.tblPaymentTransaction.tblPaymentRequest.First().PayeeInfoID);

                                IList<tblBatchPaymentReportInfo> ReportData = null;

                                //Get data for report
                                //Get all the report data as a key value pair based on the PaymentRequestID
                                ReportData = GetEPSReportData((int)mytblPaymentReport.tblPaymentNotification.tblPaymentTransaction.PaymentTransactionID);

                                myMappedData = PrepareData(ReportData, METADATARecords.tblPaymentReportFields.ToList());

                                //Generate Subject
                                string subjectLine = METADATARecords.PaymentReportEmailSubject;
                                subjectLine = Mapper.ReplaceStringValues(subjectLine, ReportData);


                                //Generate To
                                string toEmailAddress = METADATARecords.PaymentReportEmailAddresses;

                                //Generate MessageBody
                                string emailBody = METADATARecords.PaymentReportBody;
                                emailBody = Mapper.ReplaceStringValues(emailBody, ReportData);


                                //Generate the report
                                OpenXMLGenerator myOpenXMLGenerator = new OpenXMLGenerator(myMappedData, METADATARecords.tblReportFileFormat.ReportFileFormat, RowsBetweenHeaderAndData);
                                MemoryStream myMemoryStream = myOpenXMLGenerator.GenerateReport();

                                //Convert the report
                                License asposeLicense = new License();
                                asposeLicense.SetLicense("Aspose.Total.lic");
                                Document myDocument = new Document(myMemoryStream);
                                MemoryStream myComvertedDocumentMemoryStream = new MemoryStream();
                                myDocument.Save(myComvertedDocumentMemoryStream, (Aspose.Cells.SaveFormat)METADATARecords.ReportFileFormatID);

                                //Save File To archive
                                DateTime myDateTime = DateTime.Now;
                                string FileName = ReportData.First<tblBatchPaymentReportInfo>(c => c.FieldName == "FileName").FieldValue;
                                String archiveFileName = System.IO.Path.Combine(new string[] { FileArchiveLocation, myDateTime.Year.ToString("0000"), myDateTime.Month.ToString("00"), myDateTime.Day.ToString("00"), FileName });
                                new FileInfo(archiveFileName).Directory.Create();
                                File.WriteAllBytes(archiveFileName, myComvertedDocumentMemoryStream.ToArray());

                                //Save File to DMS wrkdir
                                String DMSFileName = System.IO.Path.Combine(new string[] { DMSWrkDirLocation, myDateTime.Year.ToString(), myDateTime.Month.ToString(), myDateTime.Day.ToString(), FileName });
                                new FileInfo(DMSFileName).Directory.Create();
                                File.WriteAllBytes(DMSFileName, myComvertedDocumentMemoryStream.ToArray());

                                //Send the report
                                SendEmail(toEmailAddress, subjectLine, emailBody, DMSFileName);

                                //Update record with the document
                                mytblPaymentReport.PaymentReportFile = myMemoryStream.ToArray();

                                //Update statis of tblPaymentReport record
                                UpdatetblPaymentReportRecord(mytblPaymentReport, Constants.DataBase.Tables.tblEPSStatus.SUBMITTED);

                                myMemoryStream.Dispose();
                                myComvertedDocumentMemoryStream.Dispose();
                                myMemoryStream = null;
                                myComvertedDocumentMemoryStream = null;


                            }
                            catch (Exception ex)
                            {
                                SolutionTraceClass.WriteLineWarning(String.Format("Exception while trying to send report for NotificationID = {1}.  Retry count was updated.  Message was->{0}", ex.Message, mytblPaymentReport.NotificationID));

                                if (passedmaxAllowedRetry <= mytblPaymentReport.NumberRetried)
                                {
                                    UpdatetblPaymentReportRecord(mytblPaymentReport, Constants.DataBase.Tables.tblEPSStatus.FAILED);
                                    SolutionTraceClass.WriteLineWarning(String.Format("Report was marked as failed for NotificationID={1}.  Message was->{0}", ex.Message, mytblPaymentReport.NotificationID));
                                    LoggingHelper.LogErrorActivity(String.Format("Report was marked as failed for NotificationID={0}.", mytblPaymentReport.NotificationID), ex);
                                }
                                else
                                {
                                    mytblPaymentReport.NumberRetried++;
                                    UpdatetblPaymentReportRecord(mytblPaymentReport, Constants.DataBase.Tables.tblEPSStatus.RECEIVED);
                                }

                                LoggingHelper.LogWarningActivity(string.Format("Exception while trying to send report for NotificationID = {0}. Retry count was updated.", mytblPaymentReport.NotificationID), ex);
                            }


                        }
                        transactionScope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineWarning(String.Format("Exception trying to retieve tblPaymentRequest Records.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity("Exception trying to retieve tblPaymentRequest Records.", ex);
                throw;
            }

            SolutionTraceClass.WriteLineVerbose("End");
        }

        private void SendEmail(string toEmailAddress, string subjectLine, string emailBody, string passedfilePath)
        {
            SystemServiceWrapper target = new SystemServiceWrapper(); // TODO: Initialize to an appropriate value
            string emailAddress = toEmailAddress;
            string dealId = "";
            string subject = subjectLine;
            string message = emailBody;
            string filePath = passedfilePath;
            string userId = ""; // TODO: Initialize to an appropriate value


            string jobID = target.SendEmail(emailAddress, dealId, subject, message, filePath, userId);
        }

        private void UpdatetblPaymentReportRecord(tblPaymentReport mytblPaymentReport, int passedStatus)
        {
            mytblPaymentReport.StatusID = passedStatus;
            myUnitOfWork.TblPaymentReportRepository.Update(mytblPaymentReport);
            myUnitOfWork.Save();
        }

        private tblPaymentReportInfo GetEPSResportMETADATARecord(int passedPayeeInfoID)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            try
            {
                return myUnitOfWork.TblPaymentReportInfoRepository.GetByID(passedPayeeInfoID);
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineWarning(String.Format("Exception trying to retieve TblPaymentReportinfo Records.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity("Exception trying to retieve TblPaymentReportinfo Records.", ex);
                SolutionTraceClass.WriteLineVerbose("End");
                throw;
            }
        }

        private IList<tblPaymentReport> GetEPSResportStatusRecords(int? lastPaymentTransactionID, int passedhowManyPaymentTransactionRecordsToGet)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            try
            {
                //Get the list of records to process
                return myNonGeneric.GetEPSReportStatusRecords(lastPaymentTransactionID, passedhowManyPaymentTransactionRecordsToGet);
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineWarning(String.Format("Exception trying to retieve GetEPSResportStatusRecords Records.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity("Exception trying to retieve GetEPSResportStatusRecords Records.", ex);
                SolutionTraceClass.WriteLineVerbose("End");
                throw;
            }
        }


        private IList<tblBatchPaymentReportInfo> GetEPSReportData(int passedPaymentRequestID)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            try
            {
                //Get the list of records to process
                return myNonGeneric.GetACollectionOfReportDictionaries(passedPaymentRequestID);
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineWarning(String.Format("Exception trying to retieve GetACollectionOfReportDictionaries Records.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity("Exception trying to retieve GetACollectionOfReportDictionaries Records.", ex);
                SolutionTraceClass.WriteLineVerbose("End");
                throw;
            }

        }

        public void Dispose()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            Dispose(true);
            GC.SuppressFinalize(this);
            SolutionTraceClass.WriteLineVerbose("End");

        }

        private void Dispose(bool disposing)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            if (disposed)
                return;

            if (disposing)
            {
                //handle.Dispose();
                // Free any other managed objects here. 
                //
            }

            // Free any unmanaged objects here. 
            //
            myUnitOfWork.Dispose();
            myUnitOfWork = null;
            disposed = true;
            SolutionTraceClass.WriteLineVerbose("End");
        }

        private IList<IEnumerable<MappedList>> PrepareData(IList<tblBatchPaymentReportInfo> ReportData, List<tblPaymentReportFields> ReportFields)
        {
            IEnumerable<IEnumerable<MappedList>> MergedList = (from KeyValue in ReportData
                                                               join MetaData in ReportFields on KeyValue.FieldName equals MetaData.TemplateFieldName
                                                               orderby MetaData.TemplateFieldIndex ascending
                                                               select new MappedList() { ValueData = KeyValue, MetaData = MetaData }).GroupBy(u => u.ValueData.PaymentRequestID);

            return MergedList.ToList();
        }
    }
}
