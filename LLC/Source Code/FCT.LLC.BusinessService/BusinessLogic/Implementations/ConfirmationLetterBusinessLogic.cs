using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.BusinessLogic.Interfaces;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.DocumentService.Client;
using FCT.LLC.DocumentService.Common;
using FCT.LLC.Logging;
using DealDocumentType = FCT.LLC.BusinessService.DataAccess.DealDocumentType;
using UserContext = FCT.LLC.Common.DataContracts.UserContext;

namespace FCT.LLC.BusinessService.BusinessLogic.Implementations
{
    public class ConfirmationLetterBusinessLogic : IConfirmationLetterBusinessLogic
    {
        #region Private Members

        private int _purchaserDealId;
        private int _otherDealId;

        private readonly IEntityMapper<tblDealDocumentType, DealDocumentType> _dealDocumentTypeMapper;
        private readonly IEntityMapper<tblDisbursementDealDocumentType, DisbursementDealDocumentType> _disbursementDealDocumentTypeMapper;

        private readonly IDocumentTypeRepository _documentTypeRepository; 
        private readonly IDealRepository _dealRepository;
        private readonly IDealDocumentTypeRepository _dealDocumentTypeRepository;
        private readonly IDisbursementDealDocumentTypeRepository _disbursementDealDocumentTypeRepository;
        private readonly IDisbursementRepository _disbursementRepository;
        private readonly IFundedRepository _fundedRepository;
        private readonly ILogger _logger;
        
        #endregion

        #region Constructors

        public ConfirmationLetterBusinessLogic(IDocumentTypeRepository documentTypeRepository,
            IDealRepository dealRepository, IDealDocumentTypeRepository dealDocumentTypeRepository,
            IDisbursementDealDocumentTypeRepository disbursementDealDocumentTypeRepository,
            IDisbursementRepository disbursementRepository, IFundedRepository fundedRepository, IEntityMapper<tblDealDocumentType, DealDocumentType> dealDocumentTypeMapper,
            IEntityMapper<tblDisbursementDealDocumentType, DisbursementDealDocumentType> disbursementDealDocumentTypeMapper, ILogger logger)
        {
            _documentTypeRepository = documentTypeRepository;
            _dealRepository = dealRepository;
            _dealDocumentTypeRepository = dealDocumentTypeRepository;
            _disbursementDealDocumentTypeRepository = disbursementDealDocumentTypeRepository;
            _disbursementRepository = disbursementRepository;
            _fundedRepository = fundedRepository;

            _dealDocumentTypeMapper = dealDocumentTypeMapper;
            _disbursementDealDocumentTypeMapper = disbursementDealDocumentTypeMapper;

            _logger = logger;
        }
        #endregion

        #region Implementation of IConfirmationLetterBusinessLogic
        public async Task CreateConfirmationLetters(CreateConfirmationLettersRequest request)
        {
            var activeConfirmationLetterDocumentTypeId = 0;
            // Determine the DealId's involved
            SetDealIds(request.DealID, request.UserContext);

            // Defect Fixes
            // It looks like the assumption made previously was that both Purchaser and Vendor Deals would have
            // same DealDocumentId. This assumption is not true as there will be 2 sets one for each type
            var confirmationLetterDocumentTypeId = GetConfirmationLetterDocumentTypeId();

            List<int> dealIds = new List<int>();
            if (_purchaserDealId > 0) 
                dealIds.Add(_purchaserDealId);

            if (_otherDealId > 0)
                dealIds.Add(_otherDealId);

            foreach (int dealId in dealIds)
            {
                if (confirmationLetterDocumentTypeId > 0)
                {
                    // Try to get the 'Active' confirmation letter, which is displayed in the drop-down on the front-end
                    activeConfirmationLetterDocumentTypeId = GetConfirmationLetterDealDocumentTypeId(dealId,
                        confirmationLetterDocumentTypeId, request.LanguageID);

                    // If we didn't get it, add it because this is the first time creating confirmation letters for this Deal
                    if (activeConfirmationLetterDocumentTypeId == 0)
                    {
                        // Each Deal requires an active Confirmation Letter for use in the front-end
                        activeConfirmationLetterDocumentTypeId = AddConfirmationLetterToDeal(dealId, confirmationLetterDocumentTypeId);
                    }
                }

                if (activeConfirmationLetterDocumentTypeId > 0)
                {
                    var confirmationLettersRequiringPdfGeneration =
                        GetConfirmationLettersRequiringPdfGeneration(dealId, confirmationLetterDocumentTypeId);

                    foreach (var dealDocumentTypeId in confirmationLettersRequiringPdfGeneration)
                    {
                        // Create the PDF representation of the Confirmation Letter
                        // NOTE: The resulting PDF will belong to the Vendor lawyer
                        CreateConfirmationLetter(dealId, dealDocumentTypeId,
                            request.LanguageID, request.UserContext);
                    }
                }
                else
                {
                    _logger.LogError(string.Format("Failed to look up the active Confirmation Letter for deal with id: {0}.  Confirmation Letters will not be generated.", dealId));
                }
            }

        }
       
        /// <summary>
        /// Gets a list of DisbursementDealDocumentTypes that do not yet have a PDF
        /// </summary>
        /// <param name="dealId">The ID of the Deal</param>
        /// <param name="documentTypeId"></param>
        /// <returns>Returns a list of DisbursementDealDocumentTypeID's</returns>
        private IEnumerable<int> GetConfirmationLettersRequiringPdfGeneration(int dealId, int documentTypeId)
        {
            var documentTypeIdsRequiringGeneration = new List<int>();
            var dealDocumentTypeIds = new List<int>();

            if (documentTypeId > 0)
            {
                dealDocumentTypeIds = AddConfirmationLetterToQualifyingDisbursements(dealId, documentTypeId);
            }

            if (dealDocumentTypeIds.Any())
            {
                foreach (var dealDocumentTypeId in dealDocumentTypeIds)
                {
                    if (!IsPdfGenerated(dealDocumentTypeId))
                    {
                        documentTypeIdsRequiringGeneration.Add(dealDocumentTypeId);
                    }
                }
            }

            return documentTypeIdsRequiringGeneration;
        }

        #endregion

        #region Private Methods
        private void CreateConfirmationLetter(int dealId, int dealDocumentTypeId, int languageId, UserContext userContext)
        {
            var client = new DocumentManagerClient();

            DocumentGenerationInfo documentGenerationInfo = DocumentManagerClient.GetDocumentGenerationInfoAvail(dealDocumentTypeId, languageId);

            if (documentGenerationInfo != null)
            {
                var documentTemplateId = documentGenerationInfo.DocumentTemplateId;
                string formData = Util.BuildGenerateDocumentXmlString(dealDocumentTypeId, dealId,
                    documentTemplateId, languageId, null);

                DocumentService.Common.UserContext userctx = new DocumentService.Common.UserContext
                {
                    IPAddress = null,
                    UserName = userContext.UserID
                };
                using (var scope = TransactionScopeBuilder.CreateReadCommitted())
                {
                    int documentId = client.GenerateDocumentReturnDocID(formData, Originator.FCT, userctx);

                    if (documentId <= 0)
                    {
                        _logger.LogError(string.Format("Document Service failed to create the Confirmation Letter for deal: {0}, deal document type id: {1}", dealId, dealDocumentTypeId));
                    }
                    //client.PublishDocument(documentId, dealId, Originator.FCT, userctx);
                    // FIX FOR DEFECT# 40862
                    //Update confirmation letter document published date
                    client.PublishDocumentDate(documentId);

                    scope.Complete();
                }
            }
            else
            {
                _logger.LogError(string.Format("Failed to retrieve Document Generation Info from the Document Service for deal: {0}, deal document type Id: {1}.  Confirmation Letter cannot be created.", dealId, dealDocumentTypeId));
            }
            
        }

        private int GetConfirmationLetterDocumentTypeId()
        {
            const string documentTypeName = "Payment Confirmation Letter";
            const string categoryName = "EasyFund";
            const string businessModel = "EASYFUND";

            var documentTypeId = -1;
            var documentType = _documentTypeRepository.GetByName(documentTypeName, categoryName, businessModel);

            if (documentType != null)
            {
                documentTypeId = documentType.DocumentTypeId;
            }
            else
            {
                _logger.LogError("Failed look up with Confirmation Letter Document Type Id");
            }
            
            return documentTypeId;
        }

        private List<int> AddConfirmationLetterToQualifyingDisbursements(int dealId, int documentTypeId)
        {
            var dealDocumentTypeIds = new List<int>();

            if ((dealId > 0) && (documentTypeId > 0))
            {
                // Get the deal disbursements
                var disbursements = _disbursementRepository.GetDisbursements(dealId);

                if (disbursements.Any())
                {
                    var disbursementDate = GetDisbursementDate(dealId);
                    foreach (var disbursement in disbursements)
                    {
                        if (DisbursementRequiresConfirmationLetter(disbursement))
                        {
                            var dealDocumentTypeIdForDisbursement = 0;

                            if (disbursement.DisbursementID != null)
                            {                                
                                // Check if the Disbursement has a Confirmation Letter
                                dealDocumentTypeIdForDisbursement = GetDealDocumentTypeIdForDisbursement(dealId, (int)disbursement.DisbursementID, documentTypeId);

                                // If the Disbursement does not yet have a Confirmation Letter, add one
                                if (dealDocumentTypeIdForDisbursement == 0)
                                {
                                    dealDocumentTypeIdForDisbursement = AddConfirmationLetterForDisbursement(disbursement.DisbursementID, dealId, disbursementDate, documentTypeId);
                                }
                            }

                            if (dealDocumentTypeIdForDisbursement != 0)
                            {
                                dealDocumentTypeIds.Add(dealDocumentTypeIdForDisbursement);
                            }
                        }
                    }
                }
                else
                {
                    _logger.LogError(string.Format("No disbursements were found for deal with id: {0}", dealId));
                }
            }

            return dealDocumentTypeIds;
        }

        private DateTime GetDisbursementDate(int dealId)
        {
            var disbursementDate = DateTime.MinValue;

            if (dealId > 0)
            {
                var fundedDeal = _fundedRepository.GetMilestonesByDeal(dealId);

                if ((fundedDeal != null) && (fundedDeal.Milestone != null) && (fundedDeal.Milestone.Disbursed != null))
                {
                    disbursementDate = (DateTime)fundedDeal.Milestone.Disbursed;
                }
            }

            return disbursementDate;
        }

        /// <summary>
        /// Gets the Confirmation Letter dealDocumentTypeId or adds one if not yet present
        /// </summary>
        /// <param name="dealId">The ID of the Deal</param>
        /// <param name="documentTypeId">The DocumentTypeId of the Confirmation Letter</param>
        /// <param name="languageId">The ID of the language</param>
        /// <returns>Returns the dealDocumentTypeId if successful, 0 otherwise</returns>
        private int GetConfirmationLetterDealDocumentTypeId(int dealId, int documentTypeId, int languageId, bool isActive=true)
        {
            var dealDocumentTypeId = 0;

            var dealDocumentType = _dealDocumentTypeRepository.GetByDealDocumentTypeId(dealId, documentTypeId, languageId, isActive);

            if (dealDocumentType != null)
            {
                dealDocumentTypeId = dealDocumentType.DealDocumentTypeId;
            }

            return dealDocumentTypeId;
        }

        /// <summary>
        /// Adds a Confirmation Letter to the specified Deal
        /// </summary>
        /// <param name="dealId">The ID of the Deal for which the Confirmation Letter will be added </param>
        /// <param name="documentTypeId">The ID of the document to be added for the Deal</param>
        /// <returns>Returns ID of the DealDocumentType</returns>
        private int AddConfirmationLetterToDeal(int dealId, int documentTypeId, bool isActive=true)
        {
            var dealDocumentTypeId = -1;
            
            if ((dealId > 0) && (documentTypeId > 0))
            {
                var data = new DealDocumentType
                {
                    DealId = dealId,
                    DocumentTypeId = documentTypeId,
                    IsActive = isActive,
                    DisplayNameSuffix = null
                };

                var entity = _dealDocumentTypeMapper.MapToEntity(data);

                if (entity != null)
                {
                    dealDocumentTypeId = _dealDocumentTypeRepository.InsertDealDocumentType(entity);
                }
            }
            return dealDocumentTypeId;
        }

        /// <summary>
        /// Adds a Confirmation Letter for the specified Disbursement
        /// </summary>
        /// <param name="disbursementId">The ID of the disbursement for which the Confirmation Letter will be added</param>
        /// <param name="dealId">The ID of the Deal</param>
        /// <param name="disbursementDate">The date on which the Disbursement took place</param>
        /// <param name="documentTypeId">The Document Type ID of the Confirmation Letter</param>
        /// <returns>Returns DealDocumentTypeId of the Confirmation Letter belonging to the Disbursement</returns>
        private int AddConfirmationLetterForDisbursement(int? disbursementId, int dealId, DateTime disbursementDate, int documentTypeId)
        {
            var dealDocumentTypeId = 0;
            var disbursementDealDocumentTypeId = 0;

            if (disbursementId != null)
            {
                const bool inactiveDocumentType = false;

                // Confirmation Letters for Disbursements are inactive because the user is not allowed to interact with them (Ie. Generate, re-generate, publish, etc.)
                dealDocumentTypeId = AddConfirmationLetterToDeal(dealId, documentTypeId, inactiveDocumentType);

                if (dealDocumentTypeId > 0)
                {
                    var data = new DisbursementDealDocumentType
                    {
                        CreationDate = disbursementDate,
                        DisbursementId = (int) disbursementId,
                        DealDocumentTypeId = dealDocumentTypeId
                    };

                    var entity = _disbursementDealDocumentTypeMapper.MapToEntity(data);

                    if (entity != null)
                    {
                        disbursementDealDocumentTypeId =
                            _disbursementDealDocumentTypeRepository.InsertDisbursementDealDocumentType(entity);
                    }

                    if (disbursementDealDocumentTypeId == 0)
                    {
                        _logger.LogError(string.Format("Failed to create the DisbursementDealDocumentTypeId for deal: {0}, disbursement: {1}", dealId, disbursementId));
                    }
                }
                else
                {
                    _logger.LogError(string.Format("Failed to create the Confirmation Letter Deal Document Type Id for deal with id: {0}", dealId));
                }
            }

            return dealDocumentTypeId;
        }

        private void SetDealIds(int dealId, UserContext userContext)
        {
            const bool dealInfoOnly = true;
            var deal = _dealRepository.GetDealDetails(dealId, userContext, dealInfoOnly);

            if (deal != null)
            {
                switch (deal.LawyerActingFor)
                {
                    case LawyerActingFor.Both:
                    case LawyerActingFor.Mortgagor:
                        _otherDealId = dealId;
                        _purchaserDealId = dealId;
                        break;

                    case LawyerActingFor.Purchaser:
                        _purchaserDealId = dealId;

                        if (deal.DealScopeID != null)
                        {
                            _otherDealId = _dealRepository.GetOtherDealInScope(dealId, (int) deal.DealScopeID);
                        }
                        break;

                    case LawyerActingFor.Vendor:
                        _otherDealId = dealId;
                        if (deal.DealScopeID != null)
                        {
                            _purchaserDealId = _dealRepository.GetOtherDealInScope(dealId, (int)deal.DealScopeID);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the Deal Document Type Id of the type specified by Document Type Id, for the specified Disbursement
        /// </summary>
        /// <param name="disbursementId">The Disbursement</param>
        /// <param name="documentTypeId">The Document Type Id</param>
        /// <returns>Returns the Deal Document Type Id if found, zero otherwise</returns>
        private int GetDealDocumentTypeIdForDisbursement(int dealId, int disbursementId, int documentTypeId)
        {
            var dealDocumentTypeId = 0;

            var disbursementDealDocumentType = _disbursementDealDocumentTypeRepository.GetByDisbursementIdDocumentTypeId(dealId, disbursementId, documentTypeId);

            if (disbursementDealDocumentType != null)
            {
                dealDocumentTypeId = disbursementDealDocumentType.DealDocumentTypeId;
            }

            return dealDocumentTypeId;
        }

        /// <summary>
        /// Determines whether or not a given disbursement requires a Confirmation Letter to be generated
        /// </summary>
        /// <param name="disbursement">The Disbursement to examine</param>
        /// <returns>Returns true if a Confirmation Letter is required, false otherwise</returns>
        private bool DisbursementRequiresConfirmationLetter(Disbursement disbursement)
        {
            var requiresConfirmationLetter = true;

            if (disbursement != null)
            {
                if (disbursement.PaymentMethod == LLCPaymentMethod.Cheque ||
                    disbursement.PayeeType == EasyFundFee.FeeName || disbursement.PaymentMethod==RecordType.FCTFee)
                {
                    requiresConfirmationLetter = false;
                }
            }

            return requiresConfirmationLetter;
        }

        private bool IsPdfGenerated(int dealDocumentTypeId)
        {
            var isPdfGenerated = false;

            if (dealDocumentTypeId > 0)
            {
                isPdfGenerated = _disbursementRepository.IsDisbursementDocumentGenerated(dealDocumentTypeId);
            }

            return isPdfGenerated;
        }
        #endregion
    }
}
