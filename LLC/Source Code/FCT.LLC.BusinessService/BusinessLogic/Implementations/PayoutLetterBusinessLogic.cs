using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.BusinessService.BusinessLogic.Interfaces;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.BusinessService.DataAccess.Mappers;
using FCT.LLC.DocumentService.Client;
using FCT.LLC.DocumentService.Common;
using FCT.LLC.DocumentService.Data;


namespace FCT.LLC.BusinessService.BusinessLogic.Implementations
{
    public class PayoutLetterBusinessLogic : IPayoutLetterBusinessLogic
    {
        private IDisbursementRepository _disbursementRepository;
        private IPayoutLetterWorklistRepository _payoutLetterRepository;
        private IFundingDealRepository _fundingDealRepository;
        private IDealHistoryRepository _dealHistoryRepository;
        
        public PayoutLetterBusinessLogic(IDisbursementRepository disbursementRepository, IPayoutLetterWorklistRepository payoutRepository, IFundingDealRepository fundingRepository, IDealHistoryRepository dealHistoryRepository)
        {
            _disbursementRepository = disbursementRepository;
            _payoutLetterRepository = payoutRepository;
            _fundingDealRepository = fundingRepository;
            _dealHistoryRepository = dealHistoryRepository;
        }
        public void CreatePayoutLetter(CreatePayoutLetterRequest request)
        {
            DocumentListManager dlm = new DocumentListManager();
            DocumentManagerClient dmc = new DocumentManagerClient();

            int dealDocTypeID = dlm.GetDealDocumentTypeIDByDisbursementId(request.DealDocumentTypeID, 
                                                                       request.DisbursementID, request.PayoutLetterDate);
            
            
            DocumentGenerationInfo dinfo = DocumentManagerClient.GetDocumentGenerationInfoAvail(dealDocTypeID, request.LanguageID);

            string formData = Util.BuildGenerateDocumentXmlString(dealDocTypeID, request.DealID, dinfo.DocumentTemplateId, request.LanguageID, null);

            DocumentService.Common.UserContext userctx = new DocumentService.Common.UserContext
            {
                IPAddress = null,
                UserName = request.UserContext.UserID
            };
            using (var scope=TransactionScopeBuilder.CreateReadCommitted())
            {
                int docID = dmc.GenerateDocumentReturnDocID(formData, Originator.FCT, userctx);
                dmc.PublishDocument(docID, Originator.FCT, userctx);
                CreatePayoutLetterDealHistory(ref request, ref dinfo, docID, ref userctx, ref dmc);
                scope.Complete();
            }
        }

        private void CreatePayoutLetterDealHistory(ref CreatePayoutLetterRequest request, ref DocumentGenerationInfo dinfo, int docID, 
            ref DocumentService.Common.UserContext userctx, ref DocumentManagerClient dmc)
        {
            DealHistoryEntry deCreate = new DealHistoryEntry();
            deCreate = _dealHistoryRepository.GetDealHistoryEntry(ResourceSet.LawyerPortalMessage, HistoryMessage.DocumentCreated);
            deCreate.EnglishVersion = deCreate.EnglishVersion.Replace("{0}", dinfo.DisplayName);
            deCreate.FrenchVersion = deCreate.FrenchVersion.Replace("{0}", dinfo.DisplayName);
            _dealHistoryRepository.CreateDealHistoryByDealHistoryEntry(dinfo.DealId, deCreate, request.UserContext, false);

            if (IsLawyerOrLenderUser(request.UserContext.UserType))
            {
                DealHistoryEntry dePublish = new DealHistoryEntry();
                dePublish = _dealHistoryRepository.GetDealHistoryEntry(ResourceSet.LawyerPortalMessage, HistoryMessage.DocumentPublished);
                dePublish.EnglishVersion = dePublish.EnglishVersion.Replace("{0}", dinfo.DisplayName);
                dePublish.FrenchVersion = dePublish.FrenchVersion.Replace("{0}", dinfo.DisplayName);
                _dealHistoryRepository.CreateDealHistoryByDealHistoryEntry(dinfo.DealId, dePublish, request.UserContext, false); 
            }            
            return;
        }
        private bool IsLawyerOrLenderUser(string userType)
        {
            bool IsLawyer = userType.Equals(UserType.Lawyer, StringComparison.CurrentCultureIgnoreCase)
                || userType.Equals(UserType.Clerk, StringComparison.CurrentCultureIgnoreCase)
                || userType.Equals(UserType.Assistant, StringComparison.CurrentCultureIgnoreCase)
                ||userType.Equals(UserType.LENDER, StringComparison.OrdinalIgnoreCase);
            return IsLawyer;
        }
        public GetPayoutLetterDateResponse GetPayoutLetterDate(GetPayoutLetterDateRequest request)
        {
            return _disbursementRepository.GetPayoutLetterDate(request);
        }

        public GetPayoutLettersWorklistResponse GetPayoutLettersWorklist(GetPayoutLettersWorklistRequest request)
        {
            

            int rows = 0;
            if (request.ChequeBatchNumber == null) { 
                request.ChequeBatchNumber = "";  
            }
            
            List<vw_PayoutLetterWorklist> _worklist = _payoutLetterRepository.GetPayoutLetterWorkList(request.ChequeBatchNumber, request.OrderBySpecifications, request.PageIndex, request.PageSize, request.UserContext, out rows);

            GetPayoutLettersWorklistResponse result = new GetPayoutLettersWorklistResponse();
            result.SearchResults = MapDealRelatedEntities.MapForPayoutLetterWorklist(_worklist, request.ChequeBatchNumber);
            result.TotalRowsCount = rows;
            result.PageIndex = request.PageIndex;

            return result;
        }
        public void AssignFundingDeal(AssignFundingDealRequest request) {
            

            _fundingDealRepository.UpdateFundingDealAssignedTo(request.DealID, request.UserContext.UserID);
        }


        public void SavePayoutSentDate(SavePayoutSentDateRequest request)
        {
            DocumentListManager docList = new DocumentListManager();

            List<DocumentItem> docItems = docList.GetDocumentList(request.DealID);
            var docs = docItems.Where(d => d.Type.ToUpper().Contains("PAYOUT LETTER"));
            docs = docs.Where(d => d.IsGenerated == true).ToList();
            int actualPayoutLetterCount = docs.Count();

            if ( !AllPayoutLettersAreGenerated(request.DealID, actualPayoutLetterCount) )
            {
                throw new Exception("A Payout letter must be generated on the deal before it can be removed from the worklist.");
            }

            _fundingDealRepository.UpdateFundingDealPayoutSent(request.DealID, DateTime.Now);
            return;

        }

        private bool AllPayoutLettersAreGenerated(int dealId, int actualPayoutLetterCount)
        {
             List<Disbursement> disbursements = _disbursementRepository.GetDisbursements(dealId);
             if (disbursements == null) return true;
            int requiredPayouts = disbursements.Where((d => d.PaymentMethod == "CHEQUE"
                || (d.PayeeType == "Loan/Unsecured Line of Credit" 
                && (d.AccountAction == "Reduce Account Limit" || d.AccountAction == "Close Account")) )).ToList().Count();
            return requiredPayouts <= actualPayoutLetterCount;

        }

    }
}
