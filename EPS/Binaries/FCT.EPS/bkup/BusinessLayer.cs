using ChinhDo.Transactions;
using FCT.EPS.DataEntities;
using FCT.EPS.FileSerializer.RBC;
using FCT.EPS.WSP.DataAccess;
using FCT.EPS.WSP.ExternalResources;
using FCT.EPS.WSP.GCLA.Resources;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.EFValidator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;

namespace FCT.EPS.WSP.GCLA.BusinessLogic
{
    public class BusinessLayer
    {
        public static int numOfRetried = 0;
        public static string archiveFileName = "";
        public static bool isFileMoved = false;
        public static bool isStagingSaved = false;
        public static bool isConfigFileRead = false;
        public static int Interval = 60;
        UnitOfWork myUnitOfWork = new UnitOfWork(Constants.Misc.DATABASE_LOG_CONNECTION_STRING);
        public bool isTestMode = false;
        private int updCounter = 0;
        List<tblRBCCreditorListStaging> newCreditorList = new List<tblRBCCreditorListStaging>();
        public BusinessLayer()
        {

        }
        public string GetFile(string fileInWCard)
        {
            List<string> fileList = new List<string>();
            string fileWCard = System.IO.Path.GetFileName(fileInWCard);
            string filePath = System.IO.Path.GetDirectoryName(fileInWCard);
            foreach (string file in System.IO.Directory.EnumerateFiles(filePath, fileWCard))
            {
                string ext = System.IO.Path.GetExtension(file);
                if (ext.Length != 4 || !Regex.IsMatch(ext, @"\.\d\d\d"))
                    continue;
                fileList.Add(file);
            }

            return fileList.OrderBy(el=>el.Substring(el.LastIndexOf('.')+1)).LastOrDefault();
        }
        public bool MoveFile(string fileInWCard, string fileOutPath)
        {
            if (isFileMoved == true)
                return true;
            archiveFileName = "";
            try
            {
                string filePath = GetFile(fileInWCard);
                if (!string.IsNullOrEmpty(filePath))
                {
                    TxFileManager fileMgr = new TxFileManager();
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                    {
                        String fileName = System.IO.Path.GetFileName(filePath);
                        DateTime myDateTime = DateTime.Now;
                        archiveFileName = System.IO.Path.Combine(new string[] { fileOutPath, myDateTime.Year.ToString("0000"), myDateTime.Month.ToString("00"), myDateTime.Day.ToString("00"), fileName });
                        try
                        {
                            //Create the dir
                            fileMgr.CreateDirectory(System.IO.Path.GetDirectoryName(archiveFileName));
                            //Move file to archive
                            fileMgr.Move(filePath, archiveFileName);
                        }
                        catch (System.IO.FileNotFoundException ex)
                        {
                            numOfRetried++;
                            SolutionTraceClass.WriteLineInfo(String.Format("'Credit' File ({1}) could not be moved.  Exception.  Message was->{0}", ex.Message, fileName));
                            LoggingHelper.LogErrorActivity(String.Format("'Credit' File ({0}) could not be moved.", fileName), ex);
                            archiveFileName = "";
                        }
                        catch (Exception ex)
                        {
                            numOfRetried++;
                            SolutionTraceClass.WriteLineWarning(String.Format("'Credit' File ({1}) could not be moved.  Exception.  Message was->{0}", ex.Message, fileName));
                            LoggingHelper.LogErrorActivity(String.Format("'Credit' File ({0}) could not be moved.", fileName), ex);
                            archiveFileName = "";
                        }

                        scope.Complete();
                        isFileMoved = true;
                    }
                }
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError(String.Format("'Credit' Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(ex);
                SolutionTraceClass.WriteLineVerbose("End");
                return false;
            }

            SolutionTraceClass.WriteLineVerbose("End");
            return !string.IsNullOrEmpty(archiveFileName);
        }
        private bool ValidateRow(object row)
        {
            try
            {
                //row.GetType().GetProperties().ToList().ForEach(pr => {
                //    pr.GetCustomAttributes().ToList().ForEach(att => { 
                //        switch(att.)
                //        {
                //            case: 
                //        }
                //    });
                //});
            }
            catch(Exception ex)
            {
                SolutionTraceClass.WriteLineError(String.Format("Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(ex);
                return false;
            }
            return true;
        }
        public bool ProcessCCListRBC()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            if (isStagingSaved == true)
                return true;
            if (string.IsNullOrEmpty(archiveFileName))
                return false;

            StreamReader fr = null;
            int counter = 0;
            try
            {
                DateTime now = DateTime.Now.Date;
                Translate trns = new Translate();
                UnitOfWork myUnitOfWork = new UnitOfWork();
                var oldStagingList = myUnitOfWork.TblRBCCreditorListStagingRepository;

                using (fr = new StreamReader(archiveFileName, Encoding.Default, true))
                {
                    string s;
                    // INSERT and UPDATE Staging
                    while (!string.IsNullOrEmpty(s = fr.ReadLine()))
                    {
                        counter++;
                        if (s.Length != 4349)  // discuss if this value should be configurable...
                        {
                            // exclude only corrupted creditors :)
                            string msg = "***** Line #" + counter + "[CompanyID=" + (s.Length > 6 ? s.Substring(0, 6) : "???") + "] has length=" + s.Length + ", must be " + 4349;
                            SolutionTraceClass.WriteLineError(String.Format("Invalid RBC " + System.IO.Path.GetFileName(archiveFileName) + " file data.  Message was->{0}", msg));
                            LoggingHelper.LogAuditingActivity(msg);
                            //throw new InvalidDataException();
                            continue;
                        }
                        RBCPayeeListBodyDataValues datavalue = new RBCPayeeListBodyDataValues();
                        RBCPayeeListFileBodyProcessor<RBCPayeeListBodyDataValues> process = new RBCPayeeListFileBodyProcessor<RBCPayeeListBodyDataValues>(datavalue);
                        process.DeserializeFromString(s, ref datavalue);
                        updCounter++;
                        tblRBCCreditorListStaging row = trns[datavalue]; // ACTUALLY HERE SHOULD HAPPEN VALIDATION!!
                        row.NumberRetried = -1; // Mark as existing - NO UPDATE!!
                        tblRBCCreditorListStaging rowOld = oldStagingList.GetByID(row.CompanyID);
                        if (rowOld != null)
                        {
                            if (row.EffectiveDate.Date <= now && rowOld.EffectiveDate.Date < row.EffectiveDate.Date) // candidates for UPDATE
                            {
                                row.NumberRetried = -2; // Mark as existing to overwrite later
                                //1. Y->N
                                if (row.CurrentRecord.Contains('Y') && rowOld.CurrentRecord.Contains('N') && !row.AccountNumberEditRules.Contains('D'))
                                {
                                    row.CurrentRecord = "E"; // El. Delivery + CurRec->'Y'
                                }
                                 //2. N->Y
                                else if(row.CurrentRecord.Contains('N') && rowOld.CurrentRecord.Contains('Y'))
                                {
                                    row.CurrentRecord = "C"; // cheque + CurRec->'N'
                                }
                                else if(row.AccountNumberEditRules.Contains('D'))
                                {
                                    row.CurrentRecord = row.CurrentRecord.Contains('N') ? "P" : "Q"; // Cheque
                                }
                                else if(!row.AccountNumberEditRules.Contains('D') && rowOld.AccountNumberEditRules.Contains('D'))
                                {
                                    row.CurrentRecord = row.CurrentRecord = row.CurrentRecord.Contains('N') ? "D" : "L"; // El. Delivery
                                }
                                Translate.UpdateRow(row, rowOld);
                                //rowOld.Assign(row);
                                // oldStagingList.Update(row);
                            }
                        }
                        else
                        {
                            row.NumberRetried = -3; // Mark as new to overwrite later
                            oldStagingList.Insert(row);
                        }
                    }
                    // DELETE from Staging records not in CC List
                    myUnitOfWork.Save();  // The 2-nd transactionwe ALMOST done with the Staging!!
                    archiveFileName = "";
                    isStagingSaved = true;
                }
                SolutionTraceClass.WriteLineVerbose("End");
                return updCounter > 0;
            }
            catch (Exception ex)
            {
                numOfRetried++;
                SolutionTraceClass.WriteLineError(String.Format("Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(ex);
                return false;
            }
            finally
            {
                if (fr != null)
                    fr.Dispose();
            }
        }
        public bool SaveCCUpdateList()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            try
            {
                var myUnitOfWork = new UnitOfWork();
                var stagingList = myUnitOfWork.TblRBCCreditorListStagingRepository;

                // updating list of existing payees
                var ccList = myUnitOfWork.TblCreditorListRepository;
                var payList = myUnitOfWork.TblPayeeInfoRepository;
                var adrList = myUnitOfWork.TblPaymentAddressRepository;
                var rulList = myUnitOfWork.TblCreditorRulesRepository;
                var exList = myUnitOfWork.TblCreditorListExcludedRepository.Get();

                // update DB from given CC list with BUSINESS RULES:
                stagingList.Get().ToList().ForEach(stagingItem =>
                {
                    bool isExcluded = exList.Any(el=>el.CompanyID==stagingItem.CompanyID);
                    if(!isExcluded)
                    {
                        switch (stagingItem.NumberRetried)
                        {
                            case -1: // Existing in staging - no update
                                break;
                            case -2: // Existing in staging - UPDATE
                                tblPayeeInfo payeeItem = payList.Get(el => el.tblCreditorList.CompanyID == stagingItem.CompanyID && el.IsActive).FirstOrDefault();
                                if (payeeItem != null)
                                {

                                    // Update Address
                                    adrList.Get(adr => adr.PaymentAddressID == payeeItem.PayeeAddressID).ToList().ForEach(a =>
                                    {
                                        a.StreetNumber = stagingItem.Address;
                                        a.City = stagingItem.City;
                                        a.ProvinceCode = stagingItem.Province;
                                        a.PostalCode = stagingItem.PostalCode;
                                        try
                                        {
                                            adrList.Update(a);
                                        }
                                        catch (Exception pex)
                                        {
                                            SolutionTraceClass.WriteLineError(String.Format("Exception when updating address.  Message was->{0}", pex.Message));
                                            LoggingHelper.LogErrorActivity(pex);
                                        }
                                    });

                                    // update creditor rules
                                    rulList.Get(el => el.PayeeInfoID == payeeItem.PayeeInfoID).ToList().ForEach(rule =>
                                    {
                                        rule.AccountNumberEditRules = stagingItem.AccountNumberEditRules;
                                        rule.AccountNumberExactLength = stagingItem.AccountNumberExactLength;
                                        rule.AccountNumberMinLength = stagingItem.AccountNumberMinLength;
                                        rule.AccountNumberMaxLength = stagingItem.AccountNumberMaxLength;
                                        rule.AccountNumberDataTypeFormat = stagingItem.AccountNumberDataTypeFormat;
                                        rule.AccountNumberInvalidEnd = stagingItem.AccountNumberInvalidEnd;
                                        rule.AccountNumberInValidStart = stagingItem.AccountNumberInValidStart;
                                        rule.AccountNumberValidEnd = stagingItem.AccountNumberValidEnd;
                                        rule.AccountNumberValidStart = stagingItem.AccountNumberValidStart;
                                    });

                                    // Creditor List Update
                                    var ccItem = payeeItem.tblCreditorList;
                                    // If VSA type or Company ID = 2 (American Express-Reg, Gold, Plat, Corp.'): - Do NOT update the CCIN
                                    if (!stagingItem.CreditorType.Contains("VSA") && int.Parse(stagingItem.CompanyID) != 2)
                                    {
                                        ccItem.CCIN = stagingItem.CCIN; // the old staging value should stay
                                    }
                                    ccItem.CompanyName = stagingItem.CompanyName;
                                    ccItem.Department = stagingItem.Department;
                                    ccItem.CompanyNameFr = stagingItem.CompanyNameFr;
                                    ccItem.CompanyShortNameEn = stagingItem.CompanyShortNameEn;
                                    ccItem.CompanyShortNameFr = stagingItem.CompanyShortNameFr;
                                    ccItem.LanguageIndicator = stagingItem.LanguageIndicator;
                                    ccItem.EffectiveDate = stagingItem.EffectiveDate;
                                    ccItem.CreditorType = stagingItem.CreditorType;

                                    // update payee info itself
                                    payeeItem.PayeeName = stagingItem.CompanyName;
                                    // Change payment method: 
                                    //1. Y->N/+D
                                    if (stagingItem.CurrentRecord == "C" || stagingItem.CurrentRecord == "P" || stagingItem.CurrentRecord == "Q") // Cheque [ + CurRec->'N']
                                    {
                                        stagingItem.CurrentRecord = stagingItem.CurrentRecord == "Q" ? "Y" : "N";

                                        payeeItem.PaymentMethodID = 1;
                                        payeeItem.PayeeComments = "Cheque will be mailed within 24 hours of disbursement.";
                                    }
                                    //2. N->Y/-D
                                    else if (stagingItem.CurrentRecord == "E" || stagingItem.CurrentRecord == "D" || stagingItem.CurrentRecord == "L") // El. Delivery [ + CurRec->'Y']
                                    {
                                        stagingItem.CurrentRecord = stagingItem.CurrentRecord == "D" ? "N" : "Y";

                                        payeeItem.PaymentMethodID = 6;
                                        payeeItem.PayeeComments = "Same day electronic delivery if deal disbursed before 1:00pm. Next day electronic delivery after 1:00pm disbursement.";
                                    }

                                    payeeItem.PayeeContactPhoneNumber = stagingItem.ContactPhone;
                                    payeeItem.PayeeContact = stagingItem.ContactName;
                                }
                                else
                                {
                                    newCreditorList.Add(stagingItem);
                                }
                                break;
                            case -3: // NEW for staging => (as per Benny) NEW for existing CC LIST
                                newCreditorList.Add(stagingItem);
                                break;
                            default:
                                break;
                        }
                    } // end of exclusion list check

                    // restore NumberRetried to the correct value
                    if (stagingItem.NumberRetried > -1)
                    {
                        try
                        {
                            stagingList.Delete(stagingItem); // NOT EXIST IN THE FILE!!
                        }
                        catch (Exception pex)
                        {
                            SolutionTraceClass.WriteLineError(String.Format("Exception when deleting Staging row [CompanyID=" + (stagingItem.CompanyID??"??") + "].  Message was->{0}", pex.Message));
                            LoggingHelper.LogErrorActivity(pex);
                        }
                    }
                    else
                    { 
                        stagingItem.NumberRetried = numOfRetried;
                    }
                });
                myUnitOfWork.Save(); // The 3-d transaction

                // after the moved file is successfully read - be ready for the next one!
                isFileMoved = false;
                isStagingSaved = false;
                numOfRetried = 0;
            }
            catch (Exception ex)
            {
                numOfRetried++;
                SolutionTraceClass.WriteLineError(String.Format("Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(ex);
                return false;
            }
            SolutionTraceClass.WriteLineVerbose("End");
            return true;
        }
        public bool ProcessCCNewList(string mailTo, string mailBodyNew, string mailBodyDel, string mailSubjectNew, string mailSubjectDel)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            if (newCreditorList == null || newCreditorList.Count() < 1)
                return true;
            StreamWriter fw = null, fwDel = null;
            try
            {
                List<RBCPayeeListBodyDataValues> fullCCList = new List<RBCPayeeListBodyDataValues>();
                string dateToday = DateTime.Now.Date.ToString("d");
                string heads = FormatObjPropsToCsvHead(typeof(RBCPayeeListBodyDataValues));
                newCreditorList.ForEach(item =>
                {
                    bool isToDel = item.AccountNumberEditRules.Contains('D');
                    //if (isToDel)
                    //{
                    //    SendEmail(mailTo, String.Format(mailSubjectDel, dateToday), String.Format(mailBodyDel, item.CompanyName, item.CompanyID, dateToday), "");
                    //}
                    //else
                    {
                        SendEmail(mailTo, String.Format(mailSubjectNew, dateToday), String.Format(mailBodyNew, item.CompanyName, item.CompanyID, dateToday), "");
                    }
                });
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError(String.Format("Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(ex);
            }
            finally
            {
                if (fw != null)
                    fw.Dispose();
                if (fwDel != null)
                    fwDel.Dispose();
            }
            SolutionTraceClass.WriteLineVerbose("End");
            return true;
        }
        public void SendEmail(string toEmailAddress, string subjectLine, string emailBody, string filePath)
        {
            new SystemServiceWrapper().SendEmail(toEmailAddress, "", subjectLine, emailBody, filePath, "");
        }
        private string FormatObjPropsToCsvHead(Type type)
        {
            return string.Join("", type.GetProperties().Select((info, idx) => (idx > 0 ? "," : "") + info.Name).Cast<string>().ToArray());
        }
        private string FormatObjPropsToCsvHead(object o)
        {
            return o == null ? null : FormatObjPropsToCsvHead(o.GetType());
        }
        private string FormatObjToCsv(object o)
        {
            if (o == null)
                return null;
            int totalLen = 4400;
            StringBuilder sb = new StringBuilder(totalLen);
            foreach (PropertyInfo info in o.GetType().GetProperties())
            {
                object v = info.GetValue(o);
                sb.Append((sb.Length > 0 ? "," : "") + "\"" + ConvertToStrIfStartsWith0((info.GetValue(o) ?? "").ToString().Trim().Replace("\0", "")) + "\""); // quotate segments
            }
            return sb.ToString();
        }
        private string ConvertToStrIfStartsWith0(string s)
        {
            string sret = s;
            if (!string.IsNullOrEmpty(sret) && sret[0] == '0')
                sret = "=\"\"" + s + "\"\"";
            return sret;
        }
    }
}
