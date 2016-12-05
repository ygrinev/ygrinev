using System.ServiceModel;
using System.Threading.Tasks;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.BusinessService.Contracts.FaultContracts;
using FCT.LLC.Common.DataContracts;
using System.Linq;
using FCT.LLC.BusinessService.Entities;

namespace FCT.LLC.BusinessService.Contracts
{
    [ServiceContract]
    public interface ILLCBusinessService
    {

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        GetFundingDealResponse GetFundingDeal(GetFundingDealRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        [FaultContract(typeof(ConcurrencyViolationFault))]
        SaveFundingDealResponse SaveFundingDeal(SaveFundingDealRequest request);
        
        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        void DeleteDraftDeal(DeleteDraftDealRequest request);


        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        SearchDealResponse SearchDeal(SearchDealRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        GetDealResponse GetDeal(GetDealRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        GetMilestonesResponse GetMilestones(GetMilestonesRequest request);


        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        GetNotesResponse GetNotes(GetNotesRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        GetDealHistoryResponse GetDealHistory(GetDealHistoryRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        void UpdateDealStatus(UpdateDealStatusRequest request);
        
        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        SearchFundsAllocationResponse SearchFundsAllocation(SearchFundsAllocationRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        [FaultContract(typeof(ConcurrencyViolationFault))]
        void UpdateFundsAllocation(UpdateFundsAllocationRequest request);
        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        GetDisbursementsResponse GetDisbursements(GetDisbursementsRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        [FaultContract(typeof(ConcurrencyViolationFault))]
        void AcceptDeal(AcceptDealRequest request);


        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        [FaultContract(typeof(ConcurrencyViolationFault))]
        [FaultContract(typeof(ValidationFault))]
        SaveDisbursementsResponse SaveDisbursements(SaveDisbursementsRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        CalculateFCTFeeResponse CalculateFCTFee(CalculateFCTFeeRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        [FaultContract(typeof(ConcurrencyViolationFault))]
        [FaultContract(typeof(ValidationFault))]
        Task SignDeal(SignDealRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        void UnsignDeal(UnsignDealRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        void SyncFundingDealData(SyncFundingDealDataRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        void CreatePayoutLetter(CreatePayoutLetterRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        GetPayoutLetterDateResponse GetPayoutLetterDate(GetPayoutLetterDateRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        GetPayoutLettersWorklistResponse GetPayoutLettersWorklist(GetPayoutLettersWorklistRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        void AssignFundingDeal(AssignFundingDealRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        void SavePayoutSentDate(SavePayoutSentDateRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        [FaultContract(typeof(ValidationFault))]
        DisburseFundsResponse DisburseFunds(DisburseFundsRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        [FaultContract(typeof(ValidationFault))]
        void CancelDeal(CancelDealRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        void DeclineDeal(DeclineDealRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        void SavePayoutCommentsRequest(SavePayoutCommentsRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        GetPendingFundsReturnResponse GetPendingFundsReturn(GetPendingFundsReturnRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        [FaultContract(typeof(ValidationFault))]
        void SavePendingFundsReturn(SavePendingFundsReturnRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        GetReconciliationWorklistResponse GetReconciliationWorklist(GetReconciliationWorklistRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        void SaveReconciliation(SaveReconciliationRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        void CreateConfirmationLetters(CreateConfirmationLettersRequest request);

        [OperationContract(IsOneWay = true)]
        void AsyncCreateConfirmationLetters(CreateConfirmationLettersRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        GetPifQuestionsResponse GetPifQuestions(GetPifQuestionsRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        SavePifAnswersResponse SavePifAnswers(SavePifAnswersRequest request);

        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
        int GetOtherDealId(GetDealRequest request);
    }
}
