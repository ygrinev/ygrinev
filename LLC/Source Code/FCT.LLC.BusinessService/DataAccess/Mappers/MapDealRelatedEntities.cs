using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.BusinessService.Entities;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public static class MapDealRelatedEntities
    {
        #region Public Methods
        public static DealInfoCollection MapForSearchDeal(List<vw_Deal> dealList, SearchDealCriteria searchDealCriteria)//List<DealInfo>
        {
            DealInfoCollection dealInfoList = new DealInfoCollection();

            if (dealList != null)
            {                
                foreach (vw_Deal deal in dealList)
                {
                    dealInfoList.Add(MapForSearchDeal(deal, searchDealCriteria));
                }
            }

            return dealInfoList;
        }

        public static DealInfo MapForSearchDeal(vw_Deal deal, SearchDealCriteria searchDealCriteria)
        {
            DealInfo dealInfo = new DealInfo();

            if (dealInfo != null)
            {
                dealInfo.DealID = deal.DealID;
                dealInfo.FCTURN = deal.FCTRefNum;
                dealInfo.LawyerName = deal.LawyerName;//ComposeLawyerName(deal.tblLawyer);                
                dealInfo.LenderReferenceNumber = deal.LenderRefNum;
                dealInfo.ClientName = deal.ClientName;
                dealInfo.ClosingDate = ComposeClosingDate(deal.ClosingDate);
                dealInfo.PropertyAddress = deal.Address;
                dealInfo.DealStatus = deal.Status;
                dealInfo.ActingFor = deal.LawyerActingFor;
                dealInfo.BusinessModel = deal.BusinessModel;
                
            }
            return dealInfo;
        }



        public static NoteCollection MapToNotesCollection(tblDeal deal)
        {
            NoteCollection noteCollection = new NoteCollection();
            string LenderName = string.Empty;
            string LawyerName = string.Empty;

            if(deal != null && deal.tblNotes != null)
            {
                IEnumerable<tblNote> _tblNotesList = deal.tblNotes.Where(p=>p.NoteType.ToUpper() == "DEALNOTE"
                                            && p.Usertype.ToUpper() != "FCT").OrderByDescending(p=>p.NotesID);

                if(_tblNotesList != null)
                {
                    //the below name functionality is similar to "[_usp_tblNotes__GetByDealIdUserType]" stored proc
                    if(null != deal.tblLender)             
                    {
                        LenderName = deal.tblLender.ShortName + " User";
                    }
                    if(null != deal.tblLawyer)
                    {
                        if (!string.IsNullOrEmpty(deal.tblLawyer.FirstName))
                        {
                            LawyerName = deal.tblLawyer.FirstName;
                            if (!string.IsNullOrEmpty(deal.tblLawyer.LastName))
                            {
                                LawyerName += " ";
                            } 
                        }

                        if(!string.IsNullOrEmpty(deal.tblLawyer.LastName))
                        {
                            LawyerName += deal.tblLawyer.LastName;
                        }
                    }
                    foreach(tblNote _tblNote in _tblNotesList.ToList())
                    {
                        Note _note = new Note();

                        _note.NoteDate = _tblNote.NotesDate;
                        _note.Actionable = (_tblNote.ActionableNoteStatus > 0);//_tblNote.IsNewNote;

                        _note.UserName = "System";
                        if(!string.IsNullOrEmpty(_tblNote.Usertype))
                        {
                            if(string.Equals(_tblNote.Usertype,"Lender",StringComparison.InvariantCultureIgnoreCase))
                            {
                                _note.UserName = LenderName;
                            }
                            else if (string.Equals(_tblNote.Usertype, "Lawyer", StringComparison.InvariantCultureIgnoreCase)
                                    || string.Equals(_tblNote.Usertype, "Clerk", StringComparison.InvariantCultureIgnoreCase))
                            {
                                _note.UserName = LawyerName;
                            }                             
                        }
                                                
                        _note.Title = _tblNote.Title;
                        _note.NoteMember = _tblNote.Notes;
                        _note.Status = GetActionableNoteStatus(_tblNote.ActionableNoteStatus, _tblNote.IsNewNote);
                        noteCollection.Add(_note);
                    }
                }
            }
            return noteCollection;
        }

        public static ActivityCollection MapToActivityCollection(tblDeal deal)
        {
            ActivityCollection _activityCollection = new ActivityCollection();
            string LenderCode = string.Empty;
            if(deal != null && deal.tblDealHistory != null)
            {
                IEnumerable<tblDealHistory> _tblDealHistoryList = deal.tblDealHistory.OrderByDescending(p => p.DealHistoryID);
                if(null != _tblDealHistoryList)
                {
                    if(null != deal.tblLender)
                    {
                        LenderCode = deal.tblLender.LenderCode;
                    }
                    //stored Proc - [_usp_tblDealHistory__GetByDealIdSorted]
                    foreach(tblDealHistory _dealHistory in _tblDealHistoryList.ToList())
                    {
                        Activity _activity = new Activity();

                        _activity.ActivityDate = _dealHistory.LogDate;
                        _activity.ActivityMember = _dealHistory.Activity;

                        _activity.UserName = _dealHistory.UserID;
                        if(!(string.IsNullOrEmpty(_dealHistory.UserID)) && (!string.IsNullOrEmpty(_dealHistory.UserType)))                            
                        {
                            if (string.Equals(_dealHistory.UserID, "System", StringComparison.InvariantCultureIgnoreCase)
                                && string.Equals(_dealHistory.UserType, "Lender", StringComparison.InvariantCultureIgnoreCase))
                            _activity.UserName = LenderCode + " User";
                        }
                        _activityCollection.Add(_activity);
                    }
                }
            }

            return _activityCollection;
        }

        public static MilestoneCollection MapToMilestonesCollection(tblDeal deal)
        {
            MilestoneCollection _milestoneCollection = new MilestoneCollection();
            if(deal != null && deal.tblMilestones != null)
            {
                IEnumerable<tblMilestone> _milestoneList = deal.tblMilestones;
                if(_milestoneList != null)
                {
                    foreach(tblMilestone _tblmilestone in _milestoneList)
                    {
                        Milestone milestone = new Milestone();
                        if (_tblmilestone.CompletedDateTime.HasValue)
                        {
                            milestone.CompletedDate = _tblmilestone.CompletedDateTime.Value;
                        }
                        milestone.MilestoneName = _tblmilestone.tblMilestoneCode.Description;
                        _milestoneCollection.Add(milestone);
                    }
                }
            }
            return _milestoneCollection;
        }

        public static FundingMilestone MapToFundedDealMilestones(FundedDeal _fundingDeal)
        {
            return new FundingMilestone
            {
                Disbursed = _fundingDeal.Milestone.Disbursed,
                Funded = _fundingDeal.Milestone.Funded,
                InvitationAccepted = _fundingDeal.Milestone.InvitationAccepted,
                InvitationSent= _fundingDeal.Milestone.InvitationSent,
                PayoutSent = _fundingDeal.Milestone.PayoutSent,
                SignedByPurchaser = _fundingDeal.Milestone.SignedByPurchaser,
                SignedByVendor= _fundingDeal.Milestone.SignedByVendor
            };
        }

        //public static Deal MapToDealEntity(tblDeal _tbldeal)
        //{
        //    Deal _deal = new Deal();

        //    _deal.DealID = _tbldeal.DealID;
        //    _deal.DealFCTURN = _tbldeal.FCTRefNum;
        //    if(_tbldeal.tblDealScope != null)
        //    {
        //        _deal.DealScopeFCTURN = _tbldeal.tblDealScope.ShortFCTRefNumber;
        //    }
        //    else
        //    {
        //        _deal.DealScopeFCTURN = string.Empty;
        //    }
        //    _deal.DealType = _tbldeal.DealType;
        //    _deal.DealStatus = _tbldeal.Status;
        //    _deal.Lender = GetLenderEntity(_tbldeal);
        //    _deal.Lawyer = GetLawyerEntity(_tbldeal.tblLawyer);
        //    _deal.LawyerApplication = _tbldeal.LawyerApplication;
        //    _deal.LawyerFileNumber = _tbldeal.LawyerMatterNumber;
        //    _deal.LenderRefNumber = _tbldeal.LenderRefNum;
        //    //_deal.ClosingDate = _tbldeal.tblMortgages
        //    _deal.BusinessModel = _tbldeal.BusinessModel;
        //    //Need Property,Mortgagor,Mortgage Info
        //    return _deal;
        //}

        #endregion

        #region Private Methods
        private static string GetFCTRefNumberByBusinessModel(vw_Deal deal, SearchDealCriteria searchDealCriteria)
        {
            //Assign FCTURN by default to avoid empty field
            string FCTRefNum = deal.FCTRefNum;

            if (deal != null && deal.BusinessModel != null)
            {                
                if (searchDealCriteria != null)
                {
                    //If search by DealScope then it is ShortFCTURN
                    if (!string.IsNullOrEmpty(searchDealCriteria.DealScopeFCTURN))
                    {
                        if (deal.tblDealScope != null)
                        {
                            //FCTRefNum = deal.tblDealScope.ShortFCTRefNumber;
                            FCTRefNum = deal.tblDealScope.FCTRefNumber;
                        }
                        else
                        {
                            FCTRefNum = "";
                        }
                    }
                    else if (!string.IsNullOrEmpty(searchDealCriteria.LLCFCTURN))
                    {
                        //Search by FCTRefNumber - then display accordingly
                        FCTRefNum = deal.FCTRefNum;
                    }
                    else
                    {
                        if(deal.BusinessModel.Contains("LLC"))
                        {
                            FCTRefNum = deal.FCTRefNum;
                        }
                        else if (deal.BusinessModel.Contains("EASYFUND") || deal.BusinessModel.Contains("MMS"))
                        {
                            if (deal.tblDealScope != null)
                            {
                                //FCTRefNum = deal.tblDealScope.ShortFCTRefNumber;//deal.FCTRefNum;                
                                FCTRefNum = deal.tblDealScope.FCTRefNumber;
                            }
                        }
                    }
                }
            }

            return FCTRefNum;
        }



        //return Closing Date in string
        private static string ComposeClosingDate(DateTime? ClosingDate)
        {
            if (ClosingDate.HasValue)
            {
                return ClosingDate.Value.ToString();
            }

            return String.Empty;//DateTime.MinValue;
        }

        static string ComposeLawyerName(tblLawyer lawyer)
        {
            if (lawyer != null)
            {
                return String.Format("{0}, {1}", lawyer.LastName, lawyer.FirstName);
            }

            return String.Empty;
        }

        private static string ComposeMortgagorName(ICollection<tblMortgagor> mortgagors)
        
        {
            const string TYPE_BUSINESS = "BUSINESS";
            const string TYPE_COMPANY = "COMPANY";
            const string TYPE_CORPORATION = "CORPORATION";
            const string TYPE_PERSON = "PERSON";
            const string TYPE_INDIVIDUAL = "INDIVIDUAL";

            if (mortgagors == null || mortgagors.Count == 0)
            {
                return String.Empty;
            }

            tblMortgagor mortgagor = mortgagors.FirstOrDefault();

            if (mortgagor == null)
            {
                mortgagor = mortgagors.First();
            }

            if (!string.IsNullOrEmpty(mortgagor.MortgagorType))
            {
                if (string.Equals(mortgagor.MortgagorType,TYPE_BUSINESS , StringComparison.InvariantCultureIgnoreCase) 
                    || string.Equals(mortgagor.MortgagorType, TYPE_COMPANY , StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(mortgagor.MortgagorType, TYPE_CORPORATION, StringComparison.InvariantCultureIgnoreCase))
                {
                    return String.IsNullOrEmpty(mortgagor.CompanyName) ? String.Empty : mortgagor.CompanyName;
                }

                else if (string.Equals(mortgagor.MortgagorType, TYPE_PERSON, StringComparison.InvariantCultureIgnoreCase)
                     || string.Equals(mortgagor.MortgagorType,TYPE_INDIVIDUAL, StringComparison.InvariantCultureIgnoreCase))
                {
                    return ComposeIndividualName(mortgagor);
                }
            }
            return ComposeIndividualName(mortgagor) +
                (String.IsNullOrEmpty(mortgagor.CompanyName) ? String.Empty : " " + mortgagor.CompanyName);

        }

        private static string ComposeIndividualName(tblMortgagor mortgagor)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool isFirst = true;

            if (!String.IsNullOrEmpty(mortgagor.LastName))
            {
                stringBuilder.Append(mortgagor.LastName);
                isFirst = false;
            }

            if (!String.IsNullOrEmpty(mortgagor.FirstName))
            {
                if (!isFirst)
                {
                    stringBuilder.Append(", ");
                }

                stringBuilder.Append(mortgagor.FirstName);
            }

            if (!String.IsNullOrEmpty(mortgagor.MiddleName))
            {
                if (!isFirst)
                {
                    stringBuilder.Append(" ");
                }

                stringBuilder.Append(mortgagor.MiddleName);
            }

            return stringBuilder.ToString();
        }

        private static string GetActionableNoteStatus(int actionableNoteStatus, bool isNewNote)
        {
            switch (actionableNoteStatus)
            {
                case ActionableNoteStatusConst.NotUsed:
                    if (isNewNote)
                        return "Unacknowledged";
                    return "Acknowledged";

                case ActionableNoteStatusConst.LenderCreated:
                    return "Sent";

                case ActionableNoteStatusConst.LawyerCompleted:
                case ActionableNoteStatusConst.LenderViewed:
                    return "Completed";

                case ActionableNoteStatusConst.CompletedBySystem:
                    return "Completed";

                default:
                    throw new ApplicationException("Unknown Actionable Note Status " + actionableNoteStatus);
            }
        }

        //private static Lender GetLenderEntity(tblDeal _deal)
        //{
        //    //Using full deal object because Lendercontactname is from BranchContact table
        //    Lender _lender = new Lender();
        //    if(_deal.tblLender != null)
        //    {
        //        _lender.LenderName = _deal.tblLender.Name;
        //        _lender.LenderID = _deal.tblLender.LenderID;
        //        _lender.Phone = _deal.tblLender.Phone;
        //        _lender.Fax = _deal.tblLender.Fax;
        //        //Get BranchContact data
        //        //_lender.ContactName
        //    }
        //    return _lender;
        //}

        //private static Lawyer GetLawyerEntity(tblLawyer _tbllawyer)
        //{
        //    Lawyer _lawyer = new Lawyer();
        //    if(_tbllawyer != null)
        //    {
        //        _lawyer.LawyerID = _tbllawyer.LawyerID;
        //        _lawyer.FirstName = _tbllawyer.FirstName;
        //        _lawyer.LastName = _tbllawyer.LastName;
        //        _lawyer.LawFirm = _tbllawyer.LawFirm;
        //        _lawyer.Phone = _tbllawyer.Phone;
        //    }
        //    return _lawyer;
        //}

        #endregion


        public static PayoutLetterWorklistItemCollection MapForPayoutLetterWorklist(List<vw_PayoutLetterWorklist> _worklist, string batch)
        {
            PayoutLetterWorklistItemCollection payoutList = new PayoutLetterWorklistItemCollection();

            if (payoutList != null)
            {
                foreach (vw_PayoutLetterWorklist payout in _worklist)
                {
                    payoutList.Add(MapForSearchDeal(payout, batch));
                }
            }

            return payoutList;
        }

        private static PayoutLetterWorklistItem MapForSearchDeal(vw_PayoutLetterWorklist payout, string batch)
        {
            PayoutLetterWorklistItem item = new PayoutLetterWorklistItem();

            if (payout != null)
            {
                item.AssignedTo = payout.AssignedTo;
                item.ChequeBatchDescription = payout.ChequeBatchDescription;
                item.ChequeBatchNumber = payout.ChequeBatchNumber;
                item.DealID = payout.DealID;
                item.DisbursementDate = payout.DisbursementDate.HasValue ? payout.DisbursementDate.Value : DateTime.MinValue;
                item.FCTURN = payout.FCTURN;
                item.NumberOfCheques = payout.NumberOfCheques;
            }
            return item;
            
        }
    }
}
