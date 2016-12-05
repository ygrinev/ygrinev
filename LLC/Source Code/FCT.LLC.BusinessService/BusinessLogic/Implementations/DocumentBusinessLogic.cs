using FCT.LLC.DocumentService.Client;
using FCT.LLC.DocumentService.Common;
using System.Linq;
using FCT.LLC.Service;
using System;
using System.Collections.Generic;
using FCT.LLC.Portal.DTOs.Dto;
using DealDocument = FCT.LLC.Portal.DTOs.Dto.DealDocument;
using FCT.LLC.BusinessService.Entities;
//using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public struct DocumentTypeCode
    {
        public const int FinalReport = 1;
        public const int RequestForFund = 2;
        public const int SolicitorInstruction = 3;
    }
    public class DocumentBusinessLogic : IDocumentBusinessLogic
    {
        /// <summary>
        /// Populate the grid view with lender/lawyer document
        /// </summary>
        public DealDocuments GetDealDocs(tblDeal deal, int langID)
        {
            //DealService ds = new DealService();
            //LLC.Entities.Deal deal = ds.GetByDealId(dealID);

            List<Portal.DTOs.Dto.DealDocument> _lenderDocs = new List<DealDocument>(),
                                _lawyerDocs = new List<DealDocument>(),
                                _lawyerPublishedDocs = new List<DealDocument>(),
                                _lawyerSubmittedDocs = new List<DealDocument>(),
                                _fctDocs = new List<DealDocument>();
            try
            {
                int dealID = deal.DealID;
                DocumentListManager documentListManager = new DocumentListManager();
                List<DealDocument> documentList =
                    documentListManager.GetDealDocumentByDealId(deal.DealID, langID).Select(d => new DealDocument((object)d)).Cast<DealDocument>().ToList();

                //1. Get all documents that are related to the deal

                bool bIsMMSDeal = DealStateHelper.isMMSDeal(deal.BusinessModel);
                if (DealStateHelper.isLLCDeal(deal.BusinessModel) || bIsMMSDeal)
                {
                    _lenderDocs = GetLenderDocs(documentList, dealID, langID);
                    _lawyerSubmittedDocs = GetLawyerSubmittedDocs(documentList);
                }
                // belongs to both types of the deal
                _lawyerDocs = GetLawyerDocs(documentList);

                if (DealStateHelper.isEasyFundDeal(deal.BusinessModel))
                {
                    if (!string.IsNullOrWhiteSpace(deal.LawyerActingFor) && deal.LawyerActingFor.ToUpper() != "BOTH" && deal.LawyerActingFor.ToUpper() != "MORTGAGOR")
                        _lawyerPublishedDocs = GetLawyerPublishedDocs(documentList);
                    _fctDocs = GetFCTDocs(documentList);
                }
                return new DealDocuments
                {
                    lenderDocs = _lenderDocs,
                    lawyerDocs = _lawyerDocs,
                    lawyerPublishedDocs = _lawyerPublishedDocs,
                    lawyerSubmittedDocs = _lawyerSubmittedDocs,
                    fctDocs = _fctDocs
                };
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        private static List<DealDocument> GetLawyerPublishedDocs(List<DealDocument> documentList)
        {
            List<DealDocument> lawyerDocumentList = documentList.FindAll(delegate (DealDocument doc)
            {
                return
                    (!string.IsNullOrWhiteSpace(doc.DocumentOriginator) && doc.PublishedDate.HasValue && doc.DocumentOriginator == "OTHER");
            });

            return lawyerDocumentList;
        }
        private static List<DealDocument> GetFCTDocs(List<DealDocument> documentList)
        {
            List<DealDocument> docList = documentList.FindAll(delegate (DealDocument doc)
            {
                return
                    (!string.IsNullOrWhiteSpace(doc.DocumentOriginator) && doc.DocumentOriginator == "FCT");
            });

            return docList;
        }
        private static List<DealDocument> GetLawyerSubmittedDocs(List<DealDocument> documentList)
        {
            List<DealDocument> lawyerSubmittedDocumentList = documentList.FindAll(delegate (DealDocument doc)
            {
                return
                    (doc.DocumentOriginator.ToUpper() == "LAWYER" &&
                     doc.SubmittedDate != null);
            });

            return lawyerSubmittedDocumentList;
        }
        private static List<DealDocument> GetLawyerDocs(List<DealDocument> documentList)
        {
            List<DealDocument> lawyerNonSubmittedDocumentList = documentList.FindAll(delegate (DealDocument doc)
            {
                return
                    (doc.DocumentOriginator.ToUpper() == "LAWYER" &&
                     doc.SubmittedDate == null);
            });

            return lawyerNonSubmittedDocumentList;
        }
        private static List<DealDocument> GetLenderDocs(List<DealDocument> documentList, int dealID, int langID)
        {

            List<DealDocument> lenderDocumentList = documentList.FindAll(delegate (DealDocument doc)
            {
                return (doc.DocumentOriginator.ToUpper() == "LENDER");
            });
            // A.Nikiforov Sept 7, #24258
            // we have to get SIP doc on opposite UI lang.
            DealDocument oppDoc = GetSIPDocOnDifLang(dealID, langID);
            if (oppDoc != null)
            {
                lenderDocumentList.Add(oppDoc);
            }
            //else
            //{
            //    if (documentList.Count <= 0)
            //        return null;
            //}
            //end A.Nikiforov Sept 7, # 24258
            return lenderDocumentList;
        }
        private static DealDocument GetSIPDocOnDifLang(int dealID, int langID)
        {
            DocumentListManager documentListManager = new DocumentListManager();
            //1. get all docs on currentPageLanguage and all uploaded
            List<DealDocument> documentList = null;
            int oppLangID = GetOppositeLanguageID(langID);
            documentList = documentListManager.GetDealDocumentByDealId(dealID, oppLangID).Select(d => new DealDocument((object)d)).Cast<DealDocument>().ToList();

            DealDocument doc = null;
            doc = documentList.Find(delegate (DealDocument doc1)
            {
                return (doc1.DocumentOriginator == "LENDER"
                    && doc1.DocumentCategoryId == DocumentTypeCode.SolicitorInstruction
                    && doc1.LanguageId == oppLangID);

            });
            return doc;
        }
        private static int GetOppositeLanguageID(int langID)
        {
            if (langID == LLC.Entities.Language.LanguageIdConst.English)
                return LLC.Entities.Language.LanguageIdConst.French;
            else
                return LLC.Entities.Language.LanguageIdConst.English;
        }
    }
}
