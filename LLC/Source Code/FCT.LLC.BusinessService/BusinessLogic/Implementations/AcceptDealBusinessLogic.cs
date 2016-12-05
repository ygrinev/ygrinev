using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.BusinessService.DataAccess.Mappers;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.DocumentService.Common;
using FCT.LLC.DocumentService.Client;
using FCT.LLC.Common.NotificationEmailDispatching.Entities;

namespace FCT.LLC.BusinessService.BusinessLogic.Implementations
{
    public class AcceptDealBusinessLogic : IAcceptDealBusinessLogic
    {
        private readonly IDealRepository _dealRepository;
        private readonly IDealHistoryRepository _dealHistoryRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMortgageRepository _mortgageRepository;
        private readonly IPINRepository _pinRepository;
        private readonly IFundedRepository _fundedRepository;
        private readonly IDealContactRepository _dealContactRepository;
        private readonly IGlobalizationRepository _globalizationRepository;
        private readonly IFundingDealRepository _fundingDealRepository;
        private readonly ILawyerRepository _lawyerRepository;
        private readonly IEmailHelper _emailHelper;
        private readonly IMortgagorRepository _mortgagorRepository;
        private readonly IDealScopeRepository _dealScopeRepository;
        private readonly IDealEventsPublisher _dealEventsPublisher;

        const string ConnectionKeyName = "LLCDBconnection";
        public AcceptDealBusinessLogic(IDealRepository dealRepository, IDealHistoryRepository dealHistoryRepository
                                           , IPropertyRepository propertyRepository, IMortgageRepository mortgageRepository
                                           , IPINRepository pinRepository, IFundedRepository fundedRepository
                                           , IDealContactRepository dealContactRepository, IGlobalizationRepository globalizationRepository
                                           , IFundingDealRepository fundingDealRepository
                                           , ILawyerRepository lawyerRepository, IEmailHelper emailHelper
                                           , IMortgagorRepository mortgagorRepository
                                           , IDealScopeRepository dealScopeRepository
                                           , IDealEventsPublisher dealEventsPublisher)
        {
            _dealRepository = dealRepository;           
            _dealHistoryRepository = dealHistoryRepository;
            _propertyRepository = propertyRepository;
            _mortgageRepository = mortgageRepository;
            _pinRepository = pinRepository;
            _fundedRepository = fundedRepository;
            _dealContactRepository = dealContactRepository;
            _globalizationRepository = globalizationRepository;
            _fundingDealRepository = fundingDealRepository;
            _lawyerRepository = lawyerRepository;
            _emailHelper = emailHelper;
            _mortgagorRepository = mortgagorRepository;
            _dealScopeRepository = dealScopeRepository;
            _dealEventsPublisher = dealEventsPublisher;
        }

        public void AcceptDeal(AcceptDealRequest request)
        {            
            // retrieve the 3 deals
            // 1.  The Invited Lawyer LLC deal - this is the LLC deal the invited lawyer would like to use
            // 2.  The Invited Lawyer EasyFund deal - this deal was created by the originating lawyer.  When the LLC deal is linked to - it will be physically deleted
            // 3.  The Originating Lawyer deal - this deal is the deal of the originating lawyer.
            int DealID = request.DealID;
            int dealscopeTobeDeleted = 0;
            tblDeal invitedLawyerEasyFundDeal = _dealRepository.GetDealDetails(request.DealID, request.UserContext);

            if (!FundingBusinessLogicHelper.IsValidDealForUpdate(invitedLawyerEasyFundDeal.Status))
            {
                throw new InValidDealException()
                {
                    ViolationCode = ErrorCode.DealCancelledOrDeclined,
                    ExceptionMessage = ErrorCode.DealCancelledOrDeclined.ToString()
                };
            }

            List<Tuple<string, string, string>> _dealAmendmentsList = null;
            tblDeal invitedLawyerLLCDeal = null;

            int? DealScopeID = invitedLawyerEasyFundDeal.DealScopeID;

            int originatingLawyerDealId = _dealRepository.GetOtherDealInScope(invitedLawyerEasyFundDeal.DealID, invitedLawyerEasyFundDeal.DealScopeID.Value);

            string UserName = "";
            if (request.UserContext != null)
            {
                UserName = request.UserContext.UserID;
            }

            if (request.AssociatedLLCDealID.HasValue)
            {
                invitedLawyerLLCDeal = _dealRepository.GetDealDetails(request.AssociatedLLCDealID.Value, request.UserContext, true);
                //Assign The DealId value to Associated LLC DealID
                DealID = request.AssociatedLLCDealID.Value;

                tblDeal originatingLawyerDeal = _dealRepository.GetDealDetails(originatingLawyerDealId, request.UserContext);

                //Get Documents related UserContext
                DocumentService.Common.UserContext _documentServiceUserContext = GetDocumentServiceUserContext(UserName);

                using (var scope=TransactionScopeBuilder.CreateReadCommitted())
                {
                    //Check if the DealScope already exists for LLC Deal (MMS deals have deal scope associated)
                    //If exists do not update
                    if (invitedLawyerLLCDeal.DealScopeID == null || invitedLawyerLLCDeal.DealScopeID <= 0)
                    {
                        invitedLawyerLLCDeal.DealScopeID = DealScopeID;
                    }
                    else
                    {
                        dealscopeTobeDeleted = (int)invitedLawyerLLCDeal.DealScopeID;

                        // Update LLC/MMS Deal DealScopeId to EasyFund DealScopeId, 
                        invitedLawyerLLCDeal.DealScopeID = DealScopeID;

                        // Copy DealScope Details From LLC/MMS DealScope to EasyFund DealScope
                        DealScope llcDealScope = new DealScope()
                        {
                            DealScopeId = (int)invitedLawyerEasyFundDeal.DealScopeID,
                            FCTRefNumber = invitedLawyerLLCDeal.tblDealScope.FCTRefNumber,
                            FormattedFCTRefNumber = FCTURNHelper.FormatShortFCTURN(invitedLawyerLLCDeal.tblDealScope.ShortFCTRefNumber),
                            ShortFCTRefNumber = invitedLawyerLLCDeal.tblDealScope.ShortFCTRefNumber,
                            WireDepositVerificationCode = invitedLawyerEasyFundDeal.tblDealScope.WireDepositVerificationCode
                            //WireDepositVerificationCode = FCTURNHelper.GenerateWireDepositCode()
                        };

                        _dealScopeRepository.OverwriteDealScopeDetails(llcDealScope);

                        //Clear DealScope Entity from Invited Lawyer LLC deal after successful update to avoid conflict issues 
                        invitedLawyerLLCDeal.tblDealScope = null;
                    }

                    //invitedLawyerLLCDeal.BusinessModel =  BusinessModel.COMBO;
                    invitedLawyerLLCDeal.BusinessModel = (invitedLawyerLLCDeal.BusinessModel == BusinessModel.MMS) ? BusinessModel.MMSCOMBO : BusinessModel.LLCCOMBO;
                    invitedLawyerLLCDeal.LawyerActingFor = LawyerActingFor.Purchaser; // as per Benny
                    invitedLawyerLLCDeal.DealType = DealType.PurchaseSale;

                    _dealRepository.UpdateDeal(invitedLawyerLLCDeal);

                    // STEP 1.  Check if the Invited Lawyer EasyFund (2) deal has any documents before deleting it.  If it does - these need to be migrated to the LLC deal (1).
                    MigrateDocuments(request.DealID, invitedLawyerLLCDeal.DealID, _documentServiceUserContext);//, request.UserContext);
                    //MigrateDealHistory(invitedLawyerEasyFundDeal, invitedLawyerLLCDeal);

                    // STEP 2.  Delete the Invited Lawyer EasyFund (2) deal
                    DeleteInvitedLawyerDocuments(invitedLawyerEasyFundDeal.DealID, _documentServiceUserContext);

                    DeletedInvitedLawyerDeal(invitedLawyerEasyFundDeal.DealID);

                    invitedLawyerLLCDeal = _dealRepository.GetDealDetails(request.AssociatedLLCDealID.Value, request.UserContext);   

                    // STEP 3.  Deal Sync from LLC deal to originating lawyer deal
                    //originatingLawyerDeal.LawyerMatterNumber = request.LawyerFileNumber;
                    _dealAmendmentsList = DealSync(invitedLawyerLLCDeal, originatingLawyerDeal);

                    // STEP 4. Update DealScopeID to LLCDeal
                    //Get the DealInfo only again to avoid issues with reupdating the same entity(E.g.) same LawyerId
                    //Entity Validation is being thrown.

                    _dealHistoryRepository.CreateDealHistory(request.AssociatedLLCDealID.Value, "HistoryMessage.EasyFundDealAccepted", request.UserContext);

                    _dealHistoryRepository.CreateDealHistory(originatingLawyerDealId, "HistoryMessage.EasyFundDealAccepted", request.UserContext);

                    //Update Deal Contacts for Deal
                    UpdateDealContacts(request.DealContacts, DealID);

                    //UpdateAcceptedDate Milestone in tblFundingDeal table
                    UpdateInvitationAcceptedMilestone(DealID);

                    //Delete MMS DealScope created at the time of deal submission from FCT Portal.
                    //This has to be done avoid Orphan DealScope records
                    if (dealscopeTobeDeleted > 0)
                        _dealScopeRepository.DeleteDealScope(dealscopeTobeDeleted);
                    
                    scope.Complete();
                }

                                                            
            }
            else
            {
                using (var scope=TransactionScopeBuilder.CreateReadCommitted())
                {
                    ProcessOnlyEasyFundDeal(request);

                    _dealHistoryRepository.CreateDealHistory(DealID, "HistoryMessage.EasyFundDealAccepted", request.UserContext);

                    _dealHistoryRepository.CreateDealHistory(originatingLawyerDealId, "HistoryMessage.EasyFundDealAccepted", request.UserContext);

                    //Update Deal Contacts for Deal
                    UpdateDealContacts(request.DealContacts, DealID);

                    //UpdateAcceptedDate Milestone in tblFundingDeal table
                    UpdateInvitationAcceptedMilestone(DealID);

                    scope.Complete();
                }

            }

            //Send Emails
            //This dealid will be regular LLCDealId or EasyFund DealId depending on the option selected
            FundingDeal _fundingDeal = GetFundingDeal(DealID);

            //Send business model to MMS
            if (!string.IsNullOrEmpty(_fundingDeal.BusinessModel) && _fundingDeal.BusinessModel.ToUpper().Contains(BusinessModel.MMS))
            {
                _dealEventsPublisher.SendDealBusinessModel((int)_fundingDeal.DealID, _fundingDeal.Lawyer.LawyerID);
            }
            
            //Get email Notification Recipients to avoid calling it more than once
            string sOtherNotificationRecipients = GetNotificationRecipientsForOtherLawyer(_fundingDeal, DealScopeID);

            SendemailToOriginatingLawyer(_fundingDeal, sOtherNotificationRecipients);

            if (invitedLawyerLLCDeal != null)
            {                
                if (null != _dealAmendmentsList && _dealAmendmentsList.Count > 0)
                {
                    SendEmailForDealAmendments(_dealAmendmentsList, _fundingDeal, sOtherNotificationRecipients, request.UserContext, originatingLawyerDealId);//invitedLawyerLLCDeal.DealID);
                }
            }

           
        }

        private List<Tuple<string, string, string>> DealSync(tblDeal invitedLawyerLLCDeal, tblDeal originatingLawyerDeal)
        {

            List<Tuple<string, string, string>> _dealAmendmentsList = MapOriginatingInvitedLawyerDealAmendmentsDiff(ref originatingLawyerDeal, invitedLawyerLLCDeal);

            return _dealAmendmentsList;
        }
                
        private void DeletedInvitedLawyerDeal(int invitedLawyerEasyFundDealID)//tblDeal invitedLawyerEasyFundDeal)
        {
            //Update the DealScope to null
            //This is to avoid any impact of deleting deal which doesnt delete the existing dealScope and its related records
            _dealRepository.RemoveDealScopeFromDeal(invitedLawyerEasyFundDealID);

            //Delete the deal object now
            _dealRepository.DeleteDeal(invitedLawyerEasyFundDealID);
        }

        private void MigrateDocuments(int invitedLawyerEasyFundDealID, int invitedLawyerLLCDealID, DocumentService.Common.UserContext _documentServiceUserContext)
        {            
            DocumentManagerClient dmc = new DocumentManagerClient();
            dmc.MigrateDocuments(invitedLawyerEasyFundDealID, invitedLawyerLLCDealID, Originator.NONE, _documentServiceUserContext);
        }

        private void DeleteInvitedLawyerDocuments(int invitedLawyerEasyFundDealID, DocumentService.Common.UserContext _documentServiceUserContext)
        {
            DocumentManagerClient dmc = new DocumentManagerClient();
            dmc.DeleteDocumentsByDeal(invitedLawyerEasyFundDealID, _documentServiceUserContext);
        }

        private string GetIP()
        {
            System.ServiceModel.OperationContext context = System.ServiceModel.OperationContext.Current;
            System.ServiceModel.Channels.MessageProperties prop = context.IncomingMessageProperties;
            System.ServiceModel.Channels.RemoteEndpointMessageProperty endpoint =
               prop[System.ServiceModel.Channels.RemoteEndpointMessageProperty.Name] as System.ServiceModel.Channels.RemoteEndpointMessageProperty;
            string ip = endpoint.Address;

            return ip;
        }
                
        private DocumentService.Common.UserContext GetDocumentServiceUserContext(string UserName)
        {
            //Get IPaddress and username
            string strHostIPAddress = GetIP();

            DocumentService.Common.UserContext _userContext = new DocumentService.Common.UserContext();                      
                       
            _userContext.IPAddress = strHostIPAddress;
            _userContext.UserName = UserName;

            return _userContext;
        }

        private void ProcessOnlyEasyFundDeal(AcceptDealRequest request)
        {           

            tblDeal invitedLawyerEasyFundDeal = _dealRepository.GetDealDetails(request.DealID, request.UserContext,true);
            if (null != invitedLawyerEasyFundDeal)
            {
                invitedLawyerEasyFundDeal.Status = DealStatus.Active;
                invitedLawyerEasyFundDeal.StatusDate = DateTime.Now;
                invitedLawyerEasyFundDeal.LawyerDeclinedFlag = false;
                invitedLawyerEasyFundDeal.LawyerApplication = LawyerApplication.Portal;

                if (!string.IsNullOrEmpty(request.LawyerApplication))
                {
                    invitedLawyerEasyFundDeal.LawyerApplication = request.LawyerApplication;
                }

                invitedLawyerEasyFundDeal.StatusUserType = Common.DataContracts.UserType.Lawyer;
                //Need to add status userid
                invitedLawyerEasyFundDeal.LawyerMatterNumber = request.LawyerFileNumber; 

                _dealRepository.UpdateDeal(invitedLawyerEasyFundDeal);       

            }
        }

        private void UpdateInvitationAcceptedMilestone(int DealID)
        {           
            int FundedDealID = _fundedRepository.GetFundingDealIdByDeal(DealID);
            if(FundedDealID > 0)
            {
                FundingMilestone _fundingMilestone = new FundingMilestone();
                _fundingMilestone.InvitationAccepted = DateTime.Now;
                _fundedRepository.UpdateMilestones(FundedDealID, _fundingMilestone);
            }
        }

        private void UpdateDealContacts(ContactCollection DealContacts, int DealID)
        {
            if(DealContacts != null)
            {
                _dealContactRepository.UpdateDealContactRange(DealContacts, DealID);
            }
        }

        private void SendemailToOriginatingLawyer(FundingDeal _fundingDeal, string sOtherNotificationRecipients)//int DealID, int LawyerID)
        {
            var recipientProfile = _lawyerRepository.GetNotificationDetails(_fundingDeal.OtherLawyer.LawyerID);

            //update with notification recipients received before
            if (!string.IsNullOrEmpty(sOtherNotificationRecipients))
            {
                recipientProfile.Email = sOtherNotificationRecipients; //recipientProfile.Email + ";" +
            }
            string recipientActingFor = _fundingDeal.ActingFor == LawyerActingFor.Purchaser ? LawyerActingFor.Vendor : LawyerActingFor.Purchaser;
            IDictionary<string, object> tokens = _emailHelper.CreateTokens(_fundingDeal, recipientActingFor);

            EmailHelper.SendStandardNotificationwithTokens(_fundingDeal, recipientProfile, _fundingDeal.ActingFor,
                StandardNotificationKey.EFDealInviteAccepted, tokens);
        }

        private List<Tuple<string, string, string>> MapOriginatingInvitedLawyerDealAmendmentsDiff(ref tblDeal originatingLawyerDeal, tblDeal invitedLawyerLLCDeal )
        {
            var _dealAmendmentslist = new List<Tuple<string, string, string>>();            

            if (originatingLawyerDeal != null && invitedLawyerLLCDeal != null)
            {
                //Copy Purchaser/Mortgagor info from LLC to vendor
                _mortgagorRepository.UpdateMortgagorRangeForOtherDeal(originatingLawyerDeal.DealID, invitedLawyerLLCDeal.DealID);

                if (originatingLawyerDeal.tblMortgages.Count > 0 && invitedLawyerLLCDeal.tblMortgages.Count > 0)
                {
                    if (invitedLawyerLLCDeal.tblMortgages.FirstOrDefault().ClosingDate != originatingLawyerDeal.tblMortgages.FirstOrDefault().ClosingDate)
                    {
                        DateTime? invitedClosingDate = invitedLawyerLLCDeal.tblMortgages.FirstOrDefault().ClosingDate;
                        DateTime? originatingClosingDate = originatingLawyerDeal.tblMortgages.FirstOrDefault().ClosingDate;

                        string sInvitedClosingDate = string.Empty;
                        string sOriginalLawyerDate = string.Empty;

                        if(invitedClosingDate.HasValue)
                        {
                            sInvitedClosingDate = invitedClosingDate.Value.ToShortDateString();
                        }
                        if (originatingClosingDate.HasValue)
                        {
                            sOriginalLawyerDate = originatingClosingDate.Value.ToShortDateString();
                        }

                        _dealAmendmentslist.Add(Tuple.Create("DealAmendments.EmailNotification.ClosingDate", sOriginalLawyerDate, sInvitedClosingDate));
                        //Update Mortgage Entity for the deal
                        _mortgageRepository.UpdateMortgage(originatingLawyerDeal.DealID, invitedLawyerLLCDeal.tblMortgages.FirstOrDefault().ClosingDate);
                    }
                }

                //Map Properties
                if (originatingLawyerDeal.tblProperties.Count > 0 && invitedLawyerLLCDeal.tblProperties.Count > 0)
                {
                    tblProperty originaltblProperty = originatingLawyerDeal.tblProperties.FirstOrDefault();
                    tblProperty invitedtblProperty = invitedLawyerLLCDeal.tblProperties.FirstOrDefault();

                    bool bIsAnyUpdateReq = false;

                    //checked if both are not null 
                    //Address                    
                    if (!(string.IsNullOrEmpty(originaltblProperty.Address) && string.IsNullOrEmpty(invitedtblProperty.Address)))
                    {
                        if (!string.Equals(originaltblProperty.Address, invitedtblProperty.Address, StringComparison.InvariantCultureIgnoreCase))
                        {
                            bIsAnyUpdateReq = true;
                            _dealAmendmentslist.Add(Tuple.Create("DealAmendments.EmailNotification.PropertyStreetAddress1", originaltblProperty.Address, invitedtblProperty.Address));
                            originaltblProperty.Address = invitedtblProperty.Address;                            
                        }
                    }
                    //Address2
                    if (!(string.IsNullOrEmpty(originaltblProperty.Address2) && string.IsNullOrEmpty(invitedtblProperty.Address2)))
                    {
                        if (!string.Equals(originaltblProperty.Address2, invitedtblProperty.Address2, StringComparison.InvariantCultureIgnoreCase))
                        {
                            bIsAnyUpdateReq = true;
                            _dealAmendmentslist.Add(Tuple.Create("DealAmendments.EmailNotification.PropertyStreetAddress2", originaltblProperty.Address2, originaltblProperty.Address2));
                            originaltblProperty.Address2 = invitedtblProperty.Address2;
                        }
                    }
                    //UnitNumber
                    if (!(string.IsNullOrEmpty(originaltblProperty.UnitNumber) && string.IsNullOrEmpty(invitedtblProperty.UnitNumber)))
                    {
                        if (!string.Equals(originaltblProperty.UnitNumber, invitedtblProperty.UnitNumber, StringComparison.InvariantCultureIgnoreCase))
                        {
                            bIsAnyUpdateReq = true;
                            _dealAmendmentslist.Add(Tuple.Create("DealAmendments.EmailNotification.PropertyUnitNumber", originaltblProperty.UnitNumber, invitedtblProperty.UnitNumber));
                            originaltblProperty.UnitNumber = invitedtblProperty.UnitNumber;
                        }
                    }
                    //StreetNumber
                    if (!(string.IsNullOrEmpty(originaltblProperty.StreetNumber) && string.IsNullOrEmpty(invitedtblProperty.StreetNumber)))
                    {
                        if (!string.Equals(originaltblProperty.StreetNumber, invitedtblProperty.StreetNumber, StringComparison.InvariantCultureIgnoreCase))
                        {
                            bIsAnyUpdateReq = true;
                            _dealAmendmentslist.Add(Tuple.Create("DealAmendments.EmailNotification.PropertyStreetNumber", originaltblProperty.StreetNumber, invitedtblProperty.StreetNumber));
                            originaltblProperty.StreetNumber = invitedtblProperty.StreetNumber;
                        }
                    }
                    //City
                    if (!(string.IsNullOrEmpty(originaltblProperty.City) && string.IsNullOrEmpty(invitedtblProperty.City)))
                    {
                        if (!string.Equals(originaltblProperty.City, invitedtblProperty.City, StringComparison.InvariantCultureIgnoreCase))
                        {
                            bIsAnyUpdateReq = true;
                            _dealAmendmentslist.Add(Tuple.Create("DealAmendments.EmailNotification.PropertyCity", originaltblProperty.City, invitedtblProperty.City));
                            originaltblProperty.City = invitedtblProperty.City;
                        }
                    }
                    //Province
                    if (!(string.IsNullOrEmpty(originaltblProperty.Province) && string.IsNullOrEmpty(invitedtblProperty.Province)))
                    {
                        if (!string.Equals(originaltblProperty.Province, invitedtblProperty.Province, StringComparison.InvariantCultureIgnoreCase))
                        {
                            bIsAnyUpdateReq = true;
                            _dealAmendmentslist.Add(Tuple.Create("DealAmendments.EmailNotification.PropertyProvince", originaltblProperty.Province, invitedtblProperty.Province));
                            originaltblProperty.Province = invitedtblProperty.Province;
                        }
                    }
                    //PostalCode
                    if (!(string.IsNullOrEmpty(originaltblProperty.PostalCode) && string.IsNullOrEmpty(invitedtblProperty.PostalCode)))
                    {
                        if (!string.Equals(originaltblProperty.PostalCode, invitedtblProperty.PostalCode, StringComparison.InvariantCultureIgnoreCase))
                        {
                            bIsAnyUpdateReq = true;
                            _dealAmendmentslist.Add(Tuple.Create("DealAmendments.EmailNotification.PropertyPostalCode", originaltblProperty.PostalCode, invitedtblProperty.PostalCode));
                            originaltblProperty.PostalCode = invitedtblProperty.PostalCode;
                        }
                    }
                
                    //Update Property info only if any changes
                    if (bIsAnyUpdateReq)
                    {
                        _propertyRepository.UpdateProperty(originaltblProperty);
                    }

                    //Assign all the pins
                    //Get All the PINs details for each PropertyID
                    MapPINs(ref _dealAmendmentslist, originaltblProperty.PropertyID, invitedtblProperty.PropertyID);

                    _pinRepository.DeleteAndInsertPins(originaltblProperty.PropertyID, invitedtblProperty.PropertyID);
                }
            }

            return _dealAmendmentslist;
        }



        //To send Email to originating lawyer with list of fields updated
        private void SendEmailForDealAmendments(List<Tuple<string, string, string>> _dealAmendmentsList,
                                                                  FundingDeal _fundingDeal, string sOtherNotificationRecipients, FCT.LLC.Common.DataContracts.UserContext userContext
                                                                   , int originatingLawyerDealId)
        {
            string strEnglishDescription = string.Empty;
            string strFrenchDescription = string.Empty;


            GetModifiedFields(_dealAmendmentsList, ref strEnglishDescription, ref strFrenchDescription, _fundingDeal, userContext, originatingLawyerDealId);

            if (!string.IsNullOrEmpty(strEnglishDescription) || !string.IsNullOrEmpty(strFrenchDescription))
            {
                string recipientActingFor = _fundingDeal.ActingFor == LawyerActingFor.Purchaser ? LawyerActingFor.Vendor : LawyerActingFor.Purchaser;
                IDictionary<string, object> tokens = _emailHelper.CreateTokens(_fundingDeal, recipientActingFor);

                ////To verify the below value
                //tokens.Add(EmailTemplateTokenList.LawyerFileNumber, _fundingDeal.LawyerFileNumber);

                tokens.Add(EmailTemplateTokenList.ENAmendments_Lawyer, strEnglishDescription);
                tokens.Add(EmailTemplateTokenList.FRAmendments_Lawyer, strFrenchDescription);


                var recipientProfile = _lawyerRepository.GetNotificationDetails(_fundingDeal.OtherLawyer.LawyerID);//_fundingDeal.OtherLawyer.LawyerID);

                //update with notification recipients received before
                if (!string.IsNullOrEmpty(sOtherNotificationRecipients))
                {
                    recipientProfile.Email = sOtherNotificationRecipients; //recipientProfile.Email + ";" +
                }

                EmailHelper.SendStandardNotificationwithTokens(_fundingDeal, recipientProfile, _fundingDeal.ActingFor,
                    StandardNotificationKey.EFAmendmentsToDeal, tokens);
            }
        }


        private FundingDeal GetFundingDeal(int DealID)
        {
            return _fundingDealRepository.GetFundingDeal(DealID);
        }

        private void MapPINs(ref List<Tuple<string, string, string>> _dealAmendmentslist, int originalPropertyID, int invitedPropertyID)
        {
            IEnumerable<tblPIN> _originalPINs = _pinRepository.GetPINs(originalPropertyID);
            IEnumerable<tblPIN> _invitedPINs = _pinRepository.GetPINs(invitedPropertyID);

            string sPinResourceKey="DealAmendments.EmailNotification.PropertyIdentificationNumber";

            if(_originalPINs != null && _originalPINs.Count() > 0
                    && _invitedPINs != null && _invitedPINs.Count() > 0)
            {
                List<tblPIN> originalPinsList = _originalPINs.ToList();
                List<tblPIN> invitedPinsList = _invitedPINs.ToList();

                foreach(tblPIN _tempPIN in originalPinsList)
                {
                    if (null == invitedPinsList.Find(p => p.PINNumber == _tempPIN.PINNumber))
                    {
                        _dealAmendmentslist.Add(Tuple.Create(sPinResourceKey, _tempPIN.PINNumber, string.Empty));  //null specifies removed
                    }
                }

                foreach (tblPIN _tempPIN in invitedPinsList)
                {
                    if (null == invitedPinsList.Find(p => p.PINNumber == _tempPIN.PINNumber))
                    {
                        _dealAmendmentslist.Add(Tuple.Create(sPinResourceKey, string.Empty, _tempPIN.PINNumber));  //null specifies removed
                    }
                }
            }
            else if(_originalPINs != null && _originalPINs.Count() > 0)
            {
                foreach (tblPIN _tempPIN in _originalPINs)
                {
                    _dealAmendmentslist.Add(Tuple.Create(sPinResourceKey, _tempPIN.PINNumber, string.Empty));
                }
            }
            else if (_invitedPINs != null && _invitedPINs.Count() > 0)
            {
                foreach (tblPIN _tempPIN in _invitedPINs)
                {
                    _dealAmendmentslist.Add(Tuple.Create(sPinResourceKey, string.Empty, _tempPIN.PINNumber));
                }
            }
        }
                       

        private void GetModifiedFields(List<Tuple<string, string, string>> _dealAmendmentsList, ref string strEnglishDescription, ref string strFrenchDescription
                                , FundingDeal _fundingDeal, FCT.LLC.Common.DataContracts.UserContext userContext
                                , int originatingLawyerDealId)
        {
            string strModifiedEngTemplate = string.Empty;
            string strModifiedFrTemplate = string.Empty;

            string strAddedEngTemplate = string.Empty;
            string strAddedFrTemplate = string.Empty;

            string strRemEngTemplate = string.Empty;
            string strRemFrTemplate = string.Empty;
                        
            DealHistoryEntry _dealHistoryEntry = _globalizationRepository.GetEntry(ResourceSet.LawyerPortalMessage, "DealAmendments.EmailNotification.FieldModified");
            if (_dealHistoryEntry != null)
            {
                strModifiedEngTemplate = _dealHistoryEntry.EnglishVersion;
                strModifiedFrTemplate = _dealHistoryEntry.FrenchVersion;
            }

            _dealHistoryEntry = _globalizationRepository.GetEntry(ResourceSet.LawyerPortalMessage, "DealAmendments.EmailNotification.FieldAdded");
            if (_dealHistoryEntry != null)
            {
                strAddedEngTemplate = _dealHistoryEntry.EnglishVersion;
                strAddedFrTemplate = _dealHistoryEntry.FrenchVersion;
            }

            _dealHistoryEntry = _globalizationRepository.GetEntry(ResourceSet.LawyerPortalMessage, "DealAmendments.EmailNotification.FieldRemoved");
            if (_dealHistoryEntry != null)
            {
                strRemEngTemplate = _dealHistoryEntry.EnglishVersion;
                strRemFrTemplate = _dealHistoryEntry.FrenchVersion;
            }

            for (int iIndex = 0; iIndex < _dealAmendmentsList.Count; iIndex++)
            {
                string strEngTemplateString = string.Empty;
                string strFrTemplateString = string.Empty;

                DealHistoryEntry _tempDealHistoryEntry = _globalizationRepository.GetEntry(ResourceSet.LawyerPortalMessage, _dealAmendmentsList[iIndex].Item1);

                if (_tempDealHistoryEntry != null)
                {
                    if (string.Equals(_dealAmendmentsList[iIndex].Item1,"DealAmendments.EmailNotification.PropertyIdentificationNumber",StringComparison.InvariantCultureIgnoreCase))
                    {
                        //Added Pins
                        if(!string.IsNullOrEmpty(_dealAmendmentsList[iIndex].Item2))
                        {
                            strEngTemplateString = string.Format(strAddedEngTemplate, _tempDealHistoryEntry.EnglishVersion, _dealAmendmentsList[iIndex].Item2);
                            strFrTemplateString = string.Format(strAddedFrTemplate, _tempDealHistoryEntry.FrenchVersion, _dealAmendmentsList[iIndex].Item2);
                        }
                        else if (!string.IsNullOrEmpty(_dealAmendmentsList[iIndex].Item3))//Removed Pins
                        {
                            strEngTemplateString = string.Format(strRemEngTemplate, _tempDealHistoryEntry.EnglishVersion, _dealAmendmentsList[iIndex].Item3);
                            strFrTemplateString = string.Format(strRemFrTemplate, _tempDealHistoryEntry.FrenchVersion, _dealAmendmentsList[iIndex].Item3);
                        }
                    }
                    else if (string.Equals(_dealAmendmentsList[iIndex].Item1, "DealAmendments.EmailNotification.ClosingDate", StringComparison.InvariantCultureIgnoreCase))
                    {
                        DealHistoryEntry _tempClosingDateDealHistory = _globalizationRepository.GetEntry(ResourceSet.LawyerPortalMessage, "DealAmendments.EmailNotification.ClosingDateFieldModified");
                        if (_tempClosingDateDealHistory != null)
                        {
                            strEngTemplateString = string.Format(_tempClosingDateDealHistory.EnglishVersion, _tempDealHistoryEntry.EnglishVersion, _dealAmendmentsList[iIndex].Item2, _dealAmendmentsList[iIndex].Item3);
                            strFrTemplateString = string.Format(_tempClosingDateDealHistory.FrenchVersion, _tempDealHistoryEntry.FrenchVersion, _dealAmendmentsList[iIndex].Item2, _dealAmendmentsList[iIndex].Item3);
                        }
                        else
                        {
                            strEngTemplateString = string.Format(strModifiedEngTemplate, _tempDealHistoryEntry.EnglishVersion, _dealAmendmentsList[iIndex].Item2, _dealAmendmentsList[iIndex].Item3);
                            strFrTemplateString = string.Format(strModifiedFrTemplate, _tempDealHistoryEntry.FrenchVersion, _dealAmendmentsList[iIndex].Item2, _dealAmendmentsList[iIndex].Item3);
                        }
                    }
                    else
                    {
                        strEngTemplateString = string.Format(strModifiedEngTemplate, _tempDealHistoryEntry.EnglishVersion, _dealAmendmentsList[iIndex].Item2, _dealAmendmentsList[iIndex].Item3);
                        strFrTemplateString = string.Format(strModifiedFrTemplate, _tempDealHistoryEntry.FrenchVersion, _dealAmendmentsList[iIndex].Item2, _dealAmendmentsList[iIndex].Item3);
                                                
                    }

                    //Insert Deal History records
                    AddDealHistory(originatingLawyerDealId, strEngTemplateString, strFrTemplateString, userContext);
                }

                if (!string.IsNullOrEmpty(strModifiedEngTemplate))
                {
                    strEnglishDescription = string.IsNullOrEmpty(strEnglishDescription)
                                                ? strEngTemplateString
                                                : strEnglishDescription + "<br/>" + strEngTemplateString;
                }

                if (!string.IsNullOrEmpty(strModifiedFrTemplate))
                {
                    strFrenchDescription = string.IsNullOrEmpty(strEnglishDescription)
                                                ? strFrTemplateString
                                                : strFrenchDescription + "<br/>" + strFrTemplateString;
                }
            }
        }

        private string GetNotificationRecipientsForOtherLawyer(FundingDeal _fundingDeal, int? DealScopeID)
        {           
            int OtherDealID = 0;
            if (DealScopeID.HasValue && _fundingDeal.DealID.HasValue)
            { 
               OtherDealID = _dealRepository.GetOtherDealInScope(_fundingDeal.DealID.Value, DealScopeID.Value);
            }
                        
            var mailerDetails = _emailHelper.GetEmailRecipientList(_fundingDeal, OtherDealID);

            string sEmailRecipients = mailerDetails.GetValueOrDefault().Value;                      

            return sEmailRecipients;

        }

        private void AddDealHistory(int DealID,string EnglishDesc, string FrenchDesc, FCT.LLC.Common.DataContracts.UserContext _userContext)
        {
            DealHistoryEntry _dealHistoryEntry = new DealHistoryEntry();
            _dealHistoryEntry.DealId = DealID;
            _dealHistoryEntry.EnglishVersion = EnglishDesc;
            _dealHistoryEntry.FrenchVersion = FrenchDesc;

            _dealHistoryRepository.CreateDealHistoryByDealHistoryEntry(DealID, _dealHistoryEntry, _userContext, false);
            
        }
    }
}
