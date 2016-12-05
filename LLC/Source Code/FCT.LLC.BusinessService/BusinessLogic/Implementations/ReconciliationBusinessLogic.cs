using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.BusinessService.BusinessLogic.Interfaces;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.BusinessService.DataAccess.Mappers;
using FCT.LLC.DocumentService.Client;
using FCT.LLC.DocumentService.Common;
using FCT.LLC.DocumentService.Data;
using FCT.Services.AuditLog.DataContracts;
using FCT.Services.AuditLog.ServiceContracts;

namespace FCT.LLC.BusinessService.BusinessLogic.Implementations
{
    public class ReconciliationBusinessLogic : IReconciliationBusinessLogic
    {

        private IDisbursementRepository _disbursementRepository;
        private IDealFundsAllocRepository _dealFundsAllocRepository;
        private IReconciliationItemsRepository _reconciliationItemsRepository;
        private IGlobalizationRepository _globalizationRepository;
        private readonly IAuditLogService _auditLogService;
        public ReconciliationBusinessLogic(IDisbursementRepository disbursementRepository, IDealFundsAllocRepository dealFundsAllocRepository, IReconciliationItemsRepository reconciliationItemsRepository,
            IGlobalizationRepository globalizationRepository, IAuditLogService auditLogService)
        {
            _disbursementRepository = disbursementRepository;
            _dealFundsAllocRepository = dealFundsAllocRepository;
            _reconciliationItemsRepository = reconciliationItemsRepository;
            _globalizationRepository = globalizationRepository;
            _auditLogService = auditLogService;
        }
        public GetReconciliationWorklistResponse GetReconciliationWorklist(GetReconciliationWorklistRequest request)
        {
            return  _reconciliationItemsRepository.GetReconciliationItems(request);
        }
        public void SaveReconciliation(SaveReconciliationRequest request)
        {
            List<ReconciliationItem> newReconItemList = new List<ReconciliationItem>();
            using (var scope=TransactionScopeBuilder.CreateReadCommitted())
            {
                foreach (ReconciliationItem item in request.Reconciliations)
                {
                    if (item.ItemType == "Disbursement")
                    {
                        _disbursementRepository.SetReconciliationData(item.ItemID, request.UserContext.UserID, item.Reconciled);
                        ReconciliationItem newItem = CloneReconItem(item);
                        if (string.IsNullOrEmpty(newItem.FCTURN))
                        {
                            newItem.FCTURN = _disbursementRepository.GetFCTURNByItemID(newItem.ItemID);
                        }
                        newReconItemList.Add(newItem);

                    }
                    if (item.ItemType == "FundsAllocation")
                    {
                        _dealFundsAllocRepository.SetReconciledDate(item.ItemID, item.Reconciled, request.UserContext.UserID);
                        ReconciliationItem newItem = CloneReconItem(item);
                        if (string.IsNullOrEmpty(newItem.FCTURN))
                        {
                            newItem.FCTURN = _dealFundsAllocRepository.GetFCTURNByItemID(newItem.ItemID);
                        }
                        newReconItemList.Add(newItem);
                    }
                }
 
                scope.Complete();
            }
           

            UpdateAuditLog(ref request, newReconItemList);
        }

        private ReconciliationItem CloneReconItem(ReconciliationItem item)
        {
            ReconciliationItem newItem = new ReconciliationItem()
            {
                AmountIn = item.AmountIn,
                AmountOut = item.AmountOut,
                BatchNumber = item.BatchNumber,
                ExtensionData = item.ExtensionData,
                FCTURN = item.FCTURN,
                ItemID = item.ItemID,
                ItemType = item.ItemType,
                Reconciled = item.Reconciled,
                ReferenceNumber = item.ReferenceNumber,
                TransactionDate = item.TransactionDate,
                TransactionType = item.TransactionType
            };

            return newItem;
        }

        private void UpdateAuditLog(ref SaveReconciliationRequest request, List<ReconciliationItem> reconItems)
        {
            string strStatusUpdateEng = string.Empty;

            string userId = (request.UserContext == null || request.UserContext.UserID == null)
                ? ""
                : request.UserContext.UserID;

                //Check if userType is not Lawyer then log the Audit Log message
                DealHistoryEntry dealHistoryReconciled = _globalizationRepository.GetEntry(ResourceSet.FCTLLCBusinessService, DealActivity.FundsReconciled);
                DealHistoryEntry dealHistoryUnreconciled = _globalizationRepository.GetEntry(ResourceSet.FCTLLCBusinessService, DealActivity.FundsUnreconciled);

                foreach (ReconciliationItem recon in reconItems) {

                    string message = string.Empty;
                    string activity = string.Empty;

                    if (recon.Reconciled == true)
                    {
                        message = string.Format(dealHistoryReconciled.EnglishVersion, recon.FCTURN, recon.ItemID);
                        activity = "Funds Reconciled";
                    }
                    else
                    {
                        message = string.Format(dealHistoryUnreconciled.EnglishVersion, recon.FCTURN, recon.ItemID);
                        activity = "Funds Unreconciled";
                    }

                    var auditLogRequest = new FCT.Services.AuditLog.MessageContracts.WriteLogRequest
                    {
                        LogEntry = new LogEntry()
                        {
                            Activity = activity,
                            ActivityDate = DateTime.Now,
                            Message = message,
                            UserName = userId,
                            SourceSystem = "LLC Business Service",
                            IPAddress = FundsAllocationHelper.GetIPAddress()
                        }
                    };
                    _auditLogService.WriteLog(auditLogRequest);
                }
            }

    }
}
