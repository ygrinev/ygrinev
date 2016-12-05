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
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using DocumentFormat.OpenXml.Spreadsheet;
using AgentParams = FCT.EPS.WSP.GCLA.Resources.AgentConstants.Misc;
using System.Data.Objects;
using System.Data;

namespace FCT.EPS.WSP.GCLA.BusinessLogic
{
    public class BusinessLayer
    {
        public int numOfRetried = 0;
        public string archiveFileName = "";
        public bool isFileMoved = false;
        public bool isStagingSaved = false;
        public bool isStagingInfoMerged = false;
        public bool isErrorsProcessed = false;
        public int errCounter = 0;
        public int Interval = 60;
        private int updCounter = 0;
        //
        public int lastErrorNum = 0;
        public bool isTestMode = false;
        public List<tblRBCCreditorListStaging> newCreditorList = new List<tblRBCCreditorListStaging>();
        public List<string> errCreditorList = new List<string>();
        public BusinessLayer()
        {
            //UnitOfWork myUnitOfWork = new UnitOfWork(Constants.Misc.DATABASE_LOG_CONNECTION_STRING);
            UnitOfWork myUnitOfWork = new UnitOfWork();
            isTestMode = myUnitOfWork.TblCreditorListExcludedRepository.GetByID("-10000") != null;
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

            return fileList.OrderBy(el => el.Substring(el.LastIndexOf('.') + 1)).LastOrDefault();
        }
        private static bool UpdNumRetried(tblRBCCreditorListStaging item, int value)
        { item.NumberRetried = value; return true; }
        public static bool LockProcess(TimeSpan passedPollStart)
        {
            
            try
            {
                SolutionTraceClass.WriteLineVerbose("Start");

                UnitOfWork myUnitOfWork = new UnitOfWork();
                DateTime myTempDate = DateTime.Now;
                DateTime myDateTime = new DateTime(myTempDate.Year, myTempDate.Month, myTempDate.Day, passedPollStart.Hours, passedPollStart.Minutes, passedPollStart.Seconds);
                IList<tblAgentNames> mytblAgentNames = null;
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                {
                    mytblAgentNames = new NonGeneric(myUnitOfWork).GetAgentRecord("CCListAgent");
                    scope.Complete();
                }

                
                tblAgentNames mytblAgentName = null;
                if (mytblAgentNames != null && mytblAgentNames.Count > 0)
                {
                    mytblAgentName = mytblAgentNames.First();
                }
                else 
                {
                    throw new Exception("Can't get or create agent record.");
                }
                if (myTempDate > myDateTime)
                {
                    
                    tblPaymentScheduleRunLog mytblPaymentScheduleRunLog = myUnitOfWork.TblPaymentScheduleRunLogRepository.GetByID(new object[] { mytblAgentName.AgentID, myDateTime });

                    if (mytblPaymentScheduleRunLog != null)
                    {
                        SolutionTraceClass.WriteLineVerbose("Already Run today.");
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                myUnitOfWork.TblPaymentScheduleRunLogRepository.Insert(new tblPaymentScheduleRunLog() { AgentID = mytblAgentName.AgentID, RunTime = myDateTime, Created = DateTime.Now });
                myUnitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineWarning(String.Format("'CCLISTA' cannot lock the process.  Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(String.Format("'CCLISTA' cannot lock the process."), ex);
                return false;
            }
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
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
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
                            SolutionTraceClass.WriteLineInfo(String.Format("'CCLISTA' File ({1}) could not be moved.  Exception.  Message was->{0}", ex.Message, fileName));
                            LoggingHelper.LogErrorActivity(String.Format("'CCLISTA' File ({0}) could not be moved.", fileName), ex);
                            archiveFileName = "";
                        }
                        catch (Exception ex)
                        {
                            numOfRetried++;
                            SolutionTraceClass.WriteLineWarning(String.Format("'CCLISTA' File ({1}) could not be moved.  Exception.  Message was->{0}", ex.Message, fileName));
                            LoggingHelper.LogErrorActivity(String.Format("'CCLISTA' File ({0}) could not be moved.", fileName), ex);
                            archiveFileName = "";
                        }

                        scope.Complete();
                        isFileMoved = true;
                    }
                }
            }
            catch (Exception ex)
            {
                numOfRetried++;
                SolutionTraceClass.WriteLineError(String.Format("'CCLISTA' Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(ex);
                SolutionTraceClass.WriteLineVerbose("End");
                return false;
            }

            SolutionTraceClass.WriteLineVerbose("End");
            return true; // !string.IsNullOrEmpty(archiveFileName);
        }
        public bool ProcessCCListRBC()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            if (isStagingSaved == true || string.IsNullOrEmpty(archiveFileName))
            {
                isStagingSaved = true;
                archiveFileName = "";
                return true;
            }

            StreamReader fr = null;
            int counter = 0;
            Dictionary<string, tblRBCCreditorListStaging> duplRecs = new Dictionary<string, tblRBCCreditorListStaging>();
            try
            {
                DateTime now = DateTime.Now.Date;
                Translate trns = new Translate();
                UnitOfWork myUnitOfWork = new UnitOfWork();
                var db = myUnitOfWork.Context.Database;
                var oldStagingRep = myUnitOfWork.TblRBCCreditorListStagingRepository;
                var oldStagingList = oldStagingRep.Get().ToList();

                    using (fr = new StreamReader(archiveFileName, Encoding.Default, true))
                    {
                        string s;
                        // INSERT and UPDATE Staging
                        while (!string.IsNullOrEmpty(s = fr.ReadLine()))
                        {
                            counter++;
                            if (s.Length != 4349)  // discuss if this value should be configurable...
                            {
                                lastErrorNum = counter;
                                // exclude only corrupted creditors :)
                                string msg = "***** Line #" + counter + "[CompanyID=" + (s.Length > 6 ? s.Substring(0, 6) : "???") + "] has length=" + s.Length + ", must be " + 4349;
                                SolutionTraceClass.WriteLineError(String.Format("Invalid RBC " + System.IO.Path.GetFileName(archiveFileName) + " file data.  Message was->{0}", msg));
                                LoggingHelper.LogAuditingActivity(msg);
                                //add to error file
                                errCreditorList.Add(s);
                                errCounter++;
                                continue;
                            }

                            RBCPayeeListBodyDataValues datavalue = new RBCPayeeListBodyDataValues();
                            RBCPayeeListFileBodyProcessor<RBCPayeeListBodyDataValues> process = new RBCPayeeListFileBodyProcessor<RBCPayeeListBodyDataValues>(datavalue);
                            process.DeserializeFromString(s, ref datavalue);
                            IList<string> myErrors = Validate.ValidateData(datavalue);
                            if (myErrors.Count() > 0)
                            {
                                StringBuilder RecordErrors = new StringBuilder();
                                foreach (string error in myErrors)
                                {
                                    RecordErrors.AppendLine(error);
                                }
                                lastErrorNum = counter;
                                // exclude only corrupted records :)
                                string msg = String.Format("Invalid RBC " + System.IO.Path.GetFileName(archiveFileName) + " file data.  Message was->{0}", "***** Line #" + counter + "[CompanyID=" + (s.Length > 6 ? s.Substring(0, 6) : "???") + "] has invalid values. The errors are..." + RecordErrors.ToString());
                                SolutionTraceClass.WriteLineError(msg);
                                LoggingHelper.LogAuditingActivity(msg);
                                //add to error file
                                errCreditorList.Add(s);
                                errCounter++;
                                continue;
                            }
                            updCounter++;
                            tblRBCCreditorListStaging row = trns[datavalue]; // ACTUALLY HERE SHOULD HAPPEN VALIDATION!!
                            row.NumberRetried = -1; // Mark as existing - NO UPDATE!!
                            tblRBCCreditorListStaging rowOld = oldStagingList.Where(el => el.CompanyID == row.CompanyID).FirstOrDefault();
                            if (rowOld != null)
                            {
                                if (row.EffectiveDate.Date <= now && rowOld.EffectiveDate.Date < row.EffectiveDate.Date) // candidates for UPDATE
                                {
                                    row.NumberRetried = -2; // Mark as existing for UPDATE
                                    //1. Y->N
                                    if (row.CurrentRecord.Contains('Y') && rowOld.CurrentRecord.Contains('N') && !row.AccountNumberEditRules.Contains('D'))
                                    {
                                        row.CurrentRecord = "E"; // El. Delivery + CurRec->'Y'
                                    }
                                    //2. N->Y
                                    else if (row.CurrentRecord.Contains('N') && rowOld.CurrentRecord.Contains('Y'))
                                    {
                                        row.CurrentRecord = "C"; // cheque + CurRec->'N'
                                    }
                                    else if (row.AccountNumberEditRules.Contains('D'))
                                    {
                                        row.CurrentRecord = row.CurrentRecord.Contains('N') ? "P" : "Q"; // Cheque
                                    }
                                    else if (!row.AccountNumberEditRules.Contains('D') && rowOld.AccountNumberEditRules.Contains('D'))
                                    {
                                        row.CurrentRecord = row.CurrentRecord = row.CurrentRecord.Contains('N') ? "D" : "L"; // El. Delivery
                                    }
                                    Translate.UpdateRow(row, rowOld);
                                }
                                else if(rowOld.NumberRetried != -2) // already processed with same CompanyId and newer EffDate
                                {
                                    rowOld.NumberRetried = -1;
                                }
                            }
                            else
                            {
                                row.NumberRetried = -3; // Mark as NEW
                                //take care of multiple IDs
                                if (row.CurrentRecord == "N")
                                {
                                    if(duplRecs.Keys.Contains(row.CompanyID))
                                    {
                                        if(duplRecs[row.CompanyID].CurrentRecord == "N")
                                        {
                                            SolutionTraceClass.WriteLineError(String.Format("Error.  Duplicated not current record ID={0}"), row.CompanyID);
                                            LoggingHelper.LogAuditingActivity(String.Format("Error.  Duplicated not current record ID={0}"), row.CompanyID);
                                        }
                                    }
                                    else
                                        duplRecs.Add(row.CompanyID, row);
                                }
                                else
                                {
                                    if(duplRecs.Keys.Contains(row.CompanyID)) // existing must have currentRecord="N"
                                    {
                                        // replace record with "N" with one with "Y"
                                        duplRecs[row.CompanyID] = row;
                                    }
                                    oldStagingRep.Insert(row);
                                }
                            }
                        }
                        // take care of last RBC row of 1 freaky symbol
                        if (lastErrorNum == counter)
                            errCounter--;
                        // Insert lonely records with "N", having no counterparts:
                        foreach(string key in duplRecs.Keys)
                        {
                            if(duplRecs[key].CurrentRecord == "N")
                                oldStagingRep.Insert(duplRecs[key]);
                        }
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
                errCounter = 0;
                return false;
            }
            finally
            {
                if (fr != null)
                    fr.Dispose();
            }
        }
        public bool ParseAndStageCCList()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            if (!isFileMoved || isStagingSaved == true || string.IsNullOrEmpty(archiveFileName))
            {
                archiveFileName = "";
                return true;
            }

            StreamReader fr = null;
            try
            {
                DateTime now = DateTime.Now.Date;
                Translate trns = new Translate();
                UnitOfWork myUnitOfWork = new UnitOfWork();
                var oldStagingRep = myUnitOfWork.TblRBCCreditorListStagingRepository;
                var newStageDict = new Dictionary<string, tblRBCCreditorListStaging>();
                int counter = 0;

                using (fr = new StreamReader(archiveFileName, Encoding.Default, true))
                {
                    string s;
                    tblRBCCreditorListStaging row;
                    // INSERT and UPDATE Staging
                    while (!string.IsNullOrEmpty(s = fr.ReadLine()))
                    {
                        counter++;
                        try
                        {
                            row = new tblRBCCreditorListStaging(s); // HERE HAPPENS VALIDATION!!
                        }
                        catch(Exception ex)
                        {
                            lastErrorNum = counter;
                            // exclude only corrupted creditors :)
                            string msg = "***** Line #" + counter + "[CompanyID=" + (s.Length > 6 ? s.Substring(0, 6) : "???") + "]: " + ex.Message;
                            SolutionTraceClass.WriteLineError(String.Format("Invalid RBC " + System.IO.Path.GetFileName(archiveFileName) + " file data.  Message was->{0}", msg));
                            LoggingHelper.LogAuditingActivity(msg);
                            //add to error file
                            errCreditorList.Add(s);
                            errCounter++;
                            continue;
                        }
                        if (!newStageDict.Keys.Contains(row.CompanyID) || newStageDict[row.CompanyID].EffectiveDate < row.EffectiveDate && row.EffectiveDate <= now)
                            newStageDict[row.CompanyID] = row;
                    }

                }
                DataTable dtNewCCKeys = new DataTable();
                dtNewCCKeys.Columns.Add("CompanyID", typeof(string));
                dtNewCCKeys.Columns.Add("EffectiveDate", typeof(DateTime));
                foreach (tblRBCCreditorListStaging r in newStageDict.Values)
                {
                    dtNewCCKeys.Rows.Add(new Object[] { r.CompanyID, r.EffectiveDate });
                }

                // get 3 lists for UPDATE/INSERT/DELETE
                Dictionary<string, List<string>> dictCCListKeys = new NonGeneric(myUnitOfWork).GetQListCreditorListStaging(dtNewCCKeys);
                updCounter = dictCCListKeys["UPDATE"].Count() + dictCCListKeys["INSERT"].Count() + dictCCListKeys["DELETE"].Count();

                // UPDATE
                List<string> list = dictCCListKeys["UPDATE"];
                oldStagingRep.Get(r => list.Any(el=>el == r.CompanyID)).ToList().ForEach(rowOld=>
                {
                    var rowNew = newStageDict.Values.FirstOrDefault(v=>v.CompanyID == rowOld.CompanyID); // may not be null!!
                    rowNew.NumberRetried = numOfRetried;
                    Translate.UpdateRow(rowNew, rowOld);
                    //newStageDict[rowOld.CompanyID].NumberRetried = -2; // existing updated record
                });
                // INSERT
                list = dictCCListKeys["INSERT"];
                oldStagingRep.InsertRange(newCreditorList = newStageDict.Values.Where(r => list.Any(el => el == r.CompanyID)).ToList());
                // DELETE
                list = dictCCListKeys["DELETE"];
                oldStagingRep.DeleteRange(oldStagingRep.Get(r => list.Any(el => el == r.CompanyID)));
                // take care of last RBC row of 1 freaky symbol
                if (lastErrorNum == counter)
                    errCounter--;
                if(updCounter > 0) 
                    myUnitOfWork.Save();  // The 2-nd transactionwe ALMOST done with the Staging!!
                archiveFileName = "";
                isStagingInfoMerged = true;
                SolutionTraceClass.WriteLineVerbose("End");
                return true;
            }
            catch (Exception ex)
            {
                numOfRetried++;
                SolutionTraceClass.WriteLineError(String.Format("Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(ex);
                errCounter = 0;
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
            if (!isFileMoved || isStagingInfoMerged == true || updCounter == 0)
            {
                updCounter = 0;
                return true;
            }
            try
            {
                var myUnitOfWork = new UnitOfWork();
                var stagingList = myUnitOfWork.TblRBCCreditorListStagingRepository;

                // updating list of existing payees
                var payList = myUnitOfWork.TblPayeeInfoRepository.Get().ToList();
                var ccList = myUnitOfWork.TblCreditorListRepository.Get().ToList();
                var adrList = myUnitOfWork.TblAddressRepository.Get().ToList();
                var rulList = myUnitOfWork.TblCreditorRulesRepository.Get().ToList();
                var exList = myUnitOfWork.TblCreditorListExcludedRepository.Get();


                // delete obsoleted Creditors NumberRetried > -1
                try
                {
                    stagingList.DeleteRange(stagingList.Get(item=>item.NumberRetried > -1)); // NOT EXIST IN THE FILE!!
                }
                catch (Exception pex)
                {
                    SolutionTraceClass.WriteLineError(String.Format("Exception when deleting obsoleted Staging rows.  Message was->{0}", pex.Message));
                    LoggingHelper.LogErrorActivity(pex);
                }
                // update DB from given CC list with BUSINESS RULES:
                stagingList.Get(item=>item.NumberRetried == -2 || item.NumberRetried == -1).ToList().ForEach(stagingItem =>
                {
                    bool isExcluded = exList.Any(el => el.CompanyID == stagingItem.CompanyID);
                    if (stagingItem.NumberRetried == -2 && !isExcluded)
                    {
                        var ccItem = ccList.FirstOrDefault(el=>el.CompanyID == stagingItem.CompanyID);
                        tblPayeeInfo payeeItem = ccItem == null ? null : payList.FirstOrDefault(el=>el.PayeeInfoID == ccItem.PayeeInfoID && el.IsActive);
                        if (payeeItem != null)
                        {

                            // Update Address
                            adrList.Where(adr => adr.AddressID == payeeItem.PayeeAddressID).ToList().ForEach(a =>
                            {
                                a.StreetAddress1 = stagingItem.Address;
                                a.City = stagingItem.City;
                                a.ProvinceCode = stagingItem.Province;
                                a.PostalCode = stagingItem.PostalCode;
                            });

                            // update creditor rules
                            rulList.Where(el => el.PayeeInfoID == payeeItem.PayeeInfoID).ToList().ForEach(rule =>
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
                            payeeItem.PayeeContactPhoneNumber = stagingItem.ContactPhone;
                            payeeItem.PayeeContact = stagingItem.ContactName;
                            payeeItem.StatusChangeDate = DateTime.Now.Date;
                            payeeItem.PayeeName = stagingItem.CompanyName;
                            // Change payment method: 
                            //1. Y->N/+D
                            if ("CPQ".Contains(stagingItem.CurrentRecord)) // Cheque [ + CurRec->'N']
                            {
                                stagingItem.CurrentRecord = stagingItem.CurrentRecord == "Q" ? "Y" : "N";

                                payeeItem.PaymentMethodID = 1;
                                payeeItem.PayeeComments = "Cheque will be mailed within 24 hours of disbursement.";
                            }
                            //2. N->Y/-D
                            else if ("EDL".Contains(stagingItem.CurrentRecord)) // El. Delivery [ + CurRec->'Y']
                            {
                                stagingItem.CurrentRecord = stagingItem.CurrentRecord == "D" ? "N" : "Y";

                                payeeItem.PaymentMethodID = 6;
                                payeeItem.PayeeComments = "Same day electronic delivery if deal disbursed before 1:00pm. Next day electronic delivery after 1:00pm disbursement.";
                            }
                        }
                        else if(ccItem == null)
                        {
                            newCreditorList.Add(stagingItem);
                        }
                    } // end of exclusion list check
                    else if (stagingItem.NumberRetried == -2) // Excluded!!
                    {
                        if ("CPQ".Contains(stagingItem.CurrentRecord))
                        {
                            stagingItem.CurrentRecord = stagingItem.CurrentRecord == "Q" ? "Y" : "N";
                        }
                        //2. N->Y/-D
                        else if ("EDL".Contains(stagingItem.CurrentRecord))
                        {
                            stagingItem.CurrentRecord = stagingItem.CurrentRecord == "D" ? "N" : "Y";
                        }
                    }

                    // restore NumberRetried to the correct value
                    stagingItem.NumberRetried = numOfRetried;
                });
                // save new Creditor list NumberRetried == -3
                stagingList.Get(item => item.NumberRetried == -3).ToList().ForEach(stagingItem => { stagingItem.NumberRetried = numOfRetried; newCreditorList.Add(stagingItem); });
                myUnitOfWork.Save(); // The 3-d transaction

                // after the moved file is successfully read - be ready for the next one!
                updCounter = 0;
                isStagingInfoMerged = true;
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
        public bool ProcessCCErrList(string pathToReport, string mailTo, string mailBodyErr, string mailSubjectErr)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            if (!isFileMoved || errCounter < 1 || errCreditorList == null || errCreditorList.Count() < 1)
            {
                errCounter = 0;
                errCreditorList = new List<string>();
                return true;
            }
            StreamWriter fw = null;
            try
            {
                DateTime now = DateTime.Now;
                // if we moved file to ARC folder then the chance that we cannot create a new file is slim
                string errFile = String.Format("RBC CC List Update – Erroneous Corporate Creditors on {0:yyyy-MM-dd}.txt", now);
                String errFilePath = System.IO.Path.Combine(new string[] { pathToReport, now.Year.ToString("0000"), now.Month.ToString("00"), now.Day.ToString("00"), errFile });
                new FileInfo(errFilePath).Directory.Create();
                using (fw = new StreamWriter(errFilePath, true, Encoding.Default))
                {
                    foreach(string s in errCreditorList)
                    {
                        fw.WriteLine(s);
                        fw.WriteLine();
                    }
                }
                //Send Email with file link to Errors file
                SendEmail(mailTo, String.Format(mailSubjectErr, now), String.Format(mailBodyErr, now, errFilePath), "");
                errCounter = 0;
                errCreditorList = new List<string>();
                isErrorsProcessed = true;
                return true;
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
                if (fw != null)
                    fw.Dispose();
                SolutionTraceClass.WriteLineVerbose("End");
            }
        }
        public bool ProcessCCNewList(string mailTo, string mailBodyNew, string mailSubjectNew, string FileArchiveLocation, string passedCellDateFormat)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            if (newCreditorList == null || newCreditorList.Count() < 1)
            {
                return true;
            }
            StreamWriter fw = null, fwDel = null;
            try
            {
                //Save records to the file
                //Generate the report
                OpenXMLGenerator myOpenXMLGenerator = new OpenXMLGenerator();

                myOpenXMLGenerator.AddRowToSheet
                (
                    new FCT.EPS.WSP.GCLA.BusinessLogic.OpenXMLGenerator.ReportCell()
                    {
                        CellValue = new CellValue("Company ID"),
                        DataType = CellValues.String
                    },
                    new FCT.EPS.WSP.GCLA.BusinessLogic.OpenXMLGenerator.ReportCell()
                    {
                        CellValue = new CellValue("Company Name"),
                        DataType = CellValues.String
                    },
                    new FCT.EPS.WSP.GCLA.BusinessLogic.OpenXMLGenerator.ReportCell()
                    {
                        CellValue = new CellValue("Effective Date"),
                        DataType = CellValues.String
                    }
                );

                newCreditorList.ForEach(item =>
                {
                    myOpenXMLGenerator.AddRowToSheet
                    (
                        new FCT.EPS.WSP.GCLA.BusinessLogic.OpenXMLGenerator.ReportCell()
                        {
                            CellValue = new CellValue(item.CompanyID),
                            DataType = CellValues.String
                        },
                        new FCT.EPS.WSP.GCLA.BusinessLogic.OpenXMLGenerator.ReportCell()
                        {
                            CellValue = new CellValue(item.CompanyName),
                            DataType = CellValues.String
                        },
                        new FCT.EPS.WSP.GCLA.BusinessLogic.OpenXMLGenerator.ReportCell()
                        {
                            CellValue = new CellValue(string.Format(passedCellDateFormat, item.EffectiveDate)),
                            DataType = CellValues.String
                        }
                    );
                });

                myOpenXMLGenerator.Close();

                //Create the file
                DateTime myDateTime = DateTime.Now;
                string FileName = String.Format("RBC CC List Update – New Corporate Creditors on {0:yyyy-MM-dd}.xlsx", myDateTime);
                String archiveFileName = System.IO.Path.Combine(new string[] { FileArchiveLocation, myDateTime.Year.ToString("0000"), myDateTime.Month.ToString("00"), myDateTime.Day.ToString("00"), FileName });
                new FileInfo(archiveFileName).Directory.Create();
                File.WriteAllBytes(archiveFileName, myOpenXMLGenerator.Document.ToArray());

                SolutionTraceClass.WriteLineInfo("About to email New CCList File.");
                LoggingHelper.LogAuditingActivity("About to email New CCList File.");
                //Send Email with file link
                SendEmail(mailTo, String.Format(mailSubjectNew, myDateTime), String.Format(mailBodyNew, myDateTime, archiveFileName), "");

                // refresh values
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError(String.Format("Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity("Exception in ProcessCCNewList->",ex);
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
