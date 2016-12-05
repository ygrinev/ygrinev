using System;
using System.Data;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.Common.DataContracts;
using FCT.LLC.BusinessService.BusinessLogic;
using FCT.LLC.BusinessService.BusinessLogic.Interfaces;
using FCT.LLC.BusinessService.Contracts;
using FCT.LLC.BusinessService.Contracts.FaultContracts;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.Logging;
using System.Linq;
using FCT.LLC.BusinessService.Entities;

namespace FCT.LLC.BusinessService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class LLCBusinessService : ILLCBusinessService
    {
        private readonly IDealManagementBusinessLogic _llcBusinessLogic;
        private readonly IDealSearchBusinessLogic _DealSearchBusinessLogic;
        private readonly IDealBusinessLogic _DealBusinessLogic;
        private readonly IFundsAllocBusinessLogic _fundsAllocBusinessLogic;
        private readonly IDisbursementBusinessLogic _disbursementBusinessLogic;
        private readonly IAcceptDealBusinessLogic _acceptDealBusinessLogic;
        private readonly ISignDealBusinessLogic _signDealBusinessLogic;
        private readonly ILogger _logger;
        private readonly IPayoutLetterBusinessLogic _payoutLetterBusinessLogic;
        private readonly IDeclineDealBusinessLogic _declineDealBusinessLogic;
        private readonly ICancelDealBusinessLogic _cancelDealBusinessLogic;
        private readonly IReturnFundsBusinessLogic _returnFundsBusinessLogic;
        private readonly IReconciliationBusinessLogic _reconciliationBusinessLogic;
        private readonly ReadOnlyDataHelper _helper;
        private readonly IConfirmationLetterBusinessLogic _confirmationLetterBusinessLogic;
        private readonly IPifQuestionsBusinessLogic _pifQuestionsBusinessLogic;


        public LLCBusinessService(IDealManagementBusinessLogic businessLogic,
            IDealSearchBusinessLogic dealSearchBusinessLogic
            , IDealBusinessLogic dealBusinessLogic, IFundsAllocBusinessLogic fundsAllocBusinessLogic,
            IDisbursementBusinessLogic disbursementBusinessLogic, IAcceptDealBusinessLogic acceptDealBusinessLogic,
            ISignDealBusinessLogic signDealBusinessLogic, ILogger logger,
            IPayoutLetterBusinessLogic payoutLetterBusinessLogic, IDeclineDealBusinessLogic declineDealBusinessLogic,
            ICancelDealBusinessLogic cancelDealBusinessLogic, IReturnFundsBusinessLogic returnFundsBusinessLogic,
            IReconciliationBusinessLogic reconciliationBusinessLogic, ReadOnlyDataHelper helper, IConfirmationLetterBusinessLogic confirmationLetterBusinessLogic,
            IPifQuestionsBusinessLogic pifQuestionsBusinessLogic)
        {
            _llcBusinessLogic = businessLogic;
            _DealSearchBusinessLogic = dealSearchBusinessLogic;
            _DealBusinessLogic = dealBusinessLogic;
            _fundsAllocBusinessLogic = fundsAllocBusinessLogic;
            _disbursementBusinessLogic = disbursementBusinessLogic;
            _acceptDealBusinessLogic = acceptDealBusinessLogic;
            _signDealBusinessLogic = signDealBusinessLogic;
            _logger = logger;
            _payoutLetterBusinessLogic = payoutLetterBusinessLogic;
            _declineDealBusinessLogic = declineDealBusinessLogic;
            _cancelDealBusinessLogic = cancelDealBusinessLogic;
            _returnFundsBusinessLogic = returnFundsBusinessLogic;
            _reconciliationBusinessLogic = reconciliationBusinessLogic;
            _helper = helper;
            _confirmationLetterBusinessLogic = confirmationLetterBusinessLogic;
            _pifQuestionsBusinessLogic = pifQuestionsBusinessLogic;

        }

        public GetFundingDealResponse GetFundingDeal(GetFundingDealRequest request)
        {
            try
            {               
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                return _llcBusinessLogic.GetFundingDeal(request);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
            
        }

        public SaveFundingDealResponse SaveFundingDeal(SaveFundingDealRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                var result = _llcBusinessLogic.SaveFundingDeal(request);
                return result;
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                if (ex is InValidDealException)
                {
                    var invEx = ex as InValidDealException;
                    _logger.LogError(invEx);
                    var concurrencyFault = new ConcurrencyViolationFault() { Description = invEx.ExceptionMessage,
                        ViolationCode = invEx.ViolationCode};
                    throw new FaultException<ConcurrencyViolationFault>(concurrencyFault, concurrencyFault.Description);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
            
        }

        public void DeleteDraftDeal(DeleteDraftDealRequest request)
        {
            _helper.StartGuardium();
            _llcBusinessLogic.DeleteDraftDeal(request);
        }

        //Search Deal functionality for Operations Portal
        public SearchDealResponse SearchDeal(SearchDealRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                return _DealSearchBusinessLogic.SearchDeal(request);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }

                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
        }

        public GetDealResponse GetDeal(GetDealRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                return _DealBusinessLogic.GetDeal(request);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }

                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
        }


        public void UpdateDealStatus(UpdateDealStatusRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                _DealBusinessLogic.UpdateDealStatus(request);   
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }

                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
        }

        public SearchFundsAllocationResponse SearchFundsAllocation(SearchFundsAllocationRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                return _fundsAllocBusinessLogic.SearchFundsAllocation(request);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }

                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
        }

        public void UpdateFundsAllocation(UpdateFundsAllocationRequest request)
        {

            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                _fundsAllocBusinessLogic.UpdateFundsAllocation(request);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }

                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
 
        }

        public void AcceptDeal(AcceptDealRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                _acceptDealBusinessLogic.AcceptDeal(request);
                
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                if (ex is InValidDealException)
                {
                    var invEx = ex as InValidDealException;
                    _logger.LogError(invEx);
                    var concurrencyFault = new ConcurrencyViolationFault()
                    {
                        Description = invEx.ExceptionMessage,
                        ViolationCode = invEx.ViolationCode
                    };
                    throw new FaultException<ConcurrencyViolationFault>(concurrencyFault, concurrencyFault.Description);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);

            }
        }
        public void DeclineDeal(DeclineDealRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                _declineDealBusinessLogic.DeclineDeal(request);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);

            }
        }
        public void SavePayoutCommentsRequest(SavePayoutCommentsRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                _disbursementBusinessLogic.SavePayoutComments(request);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }

                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
        }

        public GetPendingFundsReturnResponse GetPendingFundsReturn(GetPendingFundsReturnRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                return _returnFundsBusinessLogic.GetPendingFundsReturn(request);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
        }

        public void SavePendingFundsReturn(SavePendingFundsReturnRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                _returnFundsBusinessLogic.SavePendingFundsReturn(request);
            }

            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                if (ex is ValidationException)
                {
                    var invalidEx = ex as ValidationException;
                    var sb = new StringBuilder();
                    foreach (var serviceError in invalidEx.ErrorCodes)
                    {
                        sb.AppendLine(serviceError.ToString());
                    }
                    _logger.LogError(invalidEx, sb.ToString());

                    var validationFault = new ValidationFault() { ErrorCodes = invalidEx.ErrorCodes };
                    throw new FaultException<ValidationFault>(validationFault);
                }
                if (ex is DBConcurrencyException)
                {
                    var dbEx = ex as DBConcurrencyException;
                    _logger.LogError(dbEx);
                    var concurrencyFault = new ConcurrencyViolationFault() { Description = dbEx.Message };
                    throw new FaultException<ConcurrencyViolationFault>(concurrencyFault, concurrencyFault.Description);
                }
                if (ex is FaultException<ServiceFault>)
                {
                    var sevx = ex as FaultException<ServiceFault>;                   
                    var sb = new StringBuilder();
                    foreach (var serviceError in sevx.Detail.ServiceErrorList)
                    {
                        sb.AppendLine(serviceError.ErrorMessage);
                    }
                    _logger.LogError(sevx, sb.ToString());
                    serviceNotAvailableFault = new ServiceNotAvailableFault { ErrorCode = ErrorCode.SubmitPaymentFaulted, Message = sb.ToString() };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
        }

        public GetDisbursementsResponse GetDisbursements(GetDisbursementsRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                return _disbursementBusinessLogic.GetDisbursements(request);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
 
        }

        public SaveDisbursementsResponse SaveDisbursements(SaveDisbursementsRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                return _disbursementBusinessLogic.SaveDisbursements(request);
            }

            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is ValidationException)
                {
                    var invalidEx = ex as ValidationException;
                    var sb = new StringBuilder();
                    foreach (var serviceError in invalidEx.ErrorCodes)
                    {
                        sb.AppendLine(serviceError.ToString());
                    }
                    _logger.LogError(invalidEx, sb.ToString());

                    var validationFault = new ValidationFault() { ErrorCodes = invalidEx.ErrorCodes };
                    throw new FaultException<ValidationFault>(validationFault);
                }

                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                     _logger.LogError(daEx);
                   serviceNotAvailableFault = new ServiceNotAvailableFault {Message = daEx.BaseException.Message};
                   throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                if (ex is DBConcurrencyException)
                {
                    var dbEx = ex as DBConcurrencyException;
                    _logger.LogError(dbEx);
                    var concurrencyFault=new ConcurrencyViolationFault() {Description = dbEx.Message};
                    throw new FaultException<ConcurrencyViolationFault>(concurrencyFault, concurrencyFault.Description);
                }
                if (ex is InValidDealException)
                {
                    var invEx = ex as InValidDealException;
                    _logger.LogError(invEx);
                    var concurrencyFault = new ConcurrencyViolationFault()
                    {
                        Description = invEx.ExceptionMessage,
                        ViolationCode = invEx.ViolationCode
                    };
                    throw new FaultException<ConcurrencyViolationFault>(concurrencyFault, concurrencyFault.Description);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
 
        }

        public CalculateFCTFeeResponse CalculateFCTFee(CalculateFCTFeeRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext();
                return _disbursementBusinessLogic.CalculateFctFee(request);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
 
        }

        public async Task SignDeal(SignDealRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                await _signDealBusinessLogic.SignDeal(request);

            }

            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                if (ex is DBConcurrencyException)
                {
                    var dbEx = ex as DBConcurrencyException;
                    _logger.LogError(dbEx);
                    var concurrencyFault = new ConcurrencyViolationFault() { Description = dbEx.Message };
                    throw new FaultException<ConcurrencyViolationFault>(concurrencyFault, concurrencyFault.Description);
                }
                if (ex is ValidationException)
                {
                    var invalidEx = ex as ValidationException;
                    var sb = new StringBuilder();
                    foreach (var serviceError in invalidEx.ErrorCodes)
                    {
                        sb.AppendLine(serviceError.ToString());
                    }
                    _logger.LogError(invalidEx, sb.ToString());

                    var validationFault = new ValidationFault() { ErrorCodes = invalidEx.ErrorCodes };
                    throw new FaultException<ValidationFault>(validationFault);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
        }

        public void UnsignDeal(UnsignDealRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                _signDealBusinessLogic.UnsignDeal(request);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
        }

        public void SyncFundingDealData(SyncFundingDealDataRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                _llcBusinessLogic.SyncFundingDealData(request);

            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);

            }
        }

        public void CreatePayoutLetter(CreatePayoutLetterRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                _payoutLetterBusinessLogic.CreatePayoutLetter(request);

            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);

            }
        }

        public GetPayoutLetterDateResponse GetPayoutLetterDate(GetPayoutLetterDateRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                return _payoutLetterBusinessLogic.GetPayoutLetterDate(request);

            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);

            }
        }

        public GetPayoutLettersWorklistResponse GetPayoutLettersWorklist(GetPayoutLettersWorklistRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                return _payoutLetterBusinessLogic.GetPayoutLettersWorklist(request);

            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);

            }
        }

        public void AssignFundingDeal(AssignFundingDealRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                _payoutLetterBusinessLogic.AssignFundingDeal(request);
                return;
            }

            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                if (ex is ValidationException)
                {
                    var invalidEx = ex as ValidationException;
                    _logger.LogError(invalidEx);

                    var validationFault = new ValidationFault() {ErrorCodes = invalidEx.ErrorCodes};
                    throw new FaultException<ValidationFault>(validationFault);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
        }

        public void SavePayoutSentDate(SavePayoutSentDateRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                _payoutLetterBusinessLogic.SavePayoutSentDate(request);
                return;
            }

            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                if (ex is ValidationException)
                {
                    var invalidEx = ex as ValidationException;
                    _logger.LogError(invalidEx);

                    var validationFault = new ValidationFault() { ErrorCodes = invalidEx.ErrorCodes };
                    throw new FaultException<ValidationFault>(validationFault);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
            
        }

        public DisburseFundsResponse DisburseFunds(DisburseFundsRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                return _disbursementBusinessLogic.DisburseFunds(request).Result;
            }

            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex.InnerException is DataAccessException)
                {
                    var daEx = ex.InnerException as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                if (ex.InnerException is ValidationException)
                {
                    var invalidEx = ex.InnerException as ValidationException;
                    var sb = new StringBuilder();
                    foreach (var serviceError in invalidEx.ErrorCodes)
                    {
                        sb.AppendLine(serviceError.ToString());
                    }
                    _logger.LogError(invalidEx, sb.ToString());

                    var validationFault = new ValidationFault() { ErrorCodes = invalidEx.ErrorCodes };
                    throw new FaultException<ValidationFault>(validationFault);
                }
                if (ex.InnerException is DBConcurrencyException)
                {
                    var dbEx = ex.InnerException as DBConcurrencyException;
                    _logger.LogError(dbEx);
                    var concurrencyFault = new ConcurrencyViolationFault() { Description = dbEx.Message };
                    throw new FaultException<ConcurrencyViolationFault>(concurrencyFault, concurrencyFault.Description);
                }
                if (ex.InnerException is FaultException<ServiceFault>)
                {
                    var sevx = ex.InnerException as FaultException<ServiceFault>;
                    var sb = new StringBuilder();
                    foreach (var serviceError in sevx.Detail.ServiceErrorList)
                    {
                        sb.AppendLine(serviceError.ErrorMessage);
                    }
                    _logger.LogError(sevx, sb.ToString());
                    serviceNotAvailableFault = new ServiceNotAvailableFault { ErrorCode = ErrorCode.SubmitPaymentFaulted, Message = sb.ToString()};
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault);
                }
                _logger.LogUnhandledError(ex.InnerException);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.InnerException.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
        }

        public void CancelDeal(CancelDealRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                _cancelDealBusinessLogic.CancelDeal(request);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault {Message = daEx.BaseException.Message};
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault,
                        serviceNotAvailableFault.Message);
                }
                if (ex is ValidationException)
                {
                    var invalidEx = ex as ValidationException;
                    var sb = new StringBuilder();
                    foreach (var serviceError in invalidEx.ErrorCodes)
                    {
                        sb.AppendLine(serviceError.ToString());
                    }
                    _logger.LogError(invalidEx, sb.ToString());

                    var validationFault = new ValidationFault() { ErrorCodes = invalidEx.ErrorCodes };
                    throw new FaultException<ValidationFault>(validationFault);
                }

                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault {Message = ex.Message};
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault,
                    serviceNotAvailableFault.Message);
            }
        }


        public GetNotesResponse GetNotes(GetNotesRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                return _DealBusinessLogic.GetNotes(request);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
              
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
        }

        
        public GetDealHistoryResponse GetDealHistory(GetDealHistoryRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                return _DealBusinessLogic.GetDealHistory(request);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
             
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
        }


        public GetMilestonesResponse GetMilestones(GetMilestonesRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                return _DealBusinessLogic.GetDealMilestones(request);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
        }
        public GetReconciliationWorklistResponse GetReconciliationWorklist(GetReconciliationWorklistRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                return _reconciliationBusinessLogic.GetReconciliationWorklist(request);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);

            }
        }

        public void SaveReconciliation(SaveReconciliationRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                _reconciliationBusinessLogic.SaveReconciliation(request);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);

            }

        }

        public void CreateConfirmationLetters(CreateConfirmationLettersRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
              _confirmationLetterBusinessLogic.CreateConfirmationLetters(request);

            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);

            }
        }

        public async void AsyncCreateConfirmationLetters(CreateConfirmationLettersRequest request)
        {
            UserContextHelper.SetUserContext(request.UserContext.UserID);
            _helper.StartGuardium();
            await _confirmationLetterBusinessLogic.CreateConfirmationLetters(request);
           // new Task(() => _confirmationLetterBusinessLogic.CreateConfirmationLetters(request)).Start();
        }

        public GetPifQuestionsResponse GetPifQuestions(GetPifQuestionsRequest request)
        {
            var response = new GetPifQuestionsResponse();
            response.PifQuestions = new PifQuestionCollection();

            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                response.PifQuestions = _pifQuestionsBusinessLogic.GetPifQuestions(request.DealID, request.RecalculateQuestions);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }

            return response;
        }

        public SavePifAnswersResponse SavePifAnswers(SavePifAnswersRequest request)
        {
            var response = new SavePifAnswersResponse();
            response.Success = false;

            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                response.Success = _pifQuestionsBusinessLogic.SavePifAnswers(request.DealId, request.Answers);

            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }

            return response;
        }

        public int GetOtherDealId(GetDealRequest request)
        {
            try
            {
                UserContextHelper.SetUserContext(request.UserContext.UserID);
                _helper.StartGuardium();
                return _DealBusinessLogic.GetOtherDealid(request);
            }
            catch (Exception ex)
            {
                ServiceNotAvailableFault serviceNotAvailableFault;
                if (ex is DataAccessException)
                {
                    var daEx = ex as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }

                _logger.LogUnhandledError(ex);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = ex.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
        }
    }
}
