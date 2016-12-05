using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class DisbursementMapper:IEntityMapper<tblDisbursement, Disbursement>
    {
        private readonly IEntityMapper<tblFee, Fee> _feeMapper; 
        public DisbursementMapper(IEntityMapper<tblFee, Fee> feeMapper)
        {
            _feeMapper = feeMapper;
        }
        public Disbursement MapToData(tblDisbursement tEntity, object parameters = null)
        {
            if (tEntity != null)
            {
                var data = new Disbursement()
                {
                    AccountAction = tEntity.AccountAction,
                    AccountNumber = tEntity.AccountNumber,
                    TrustAccountID = tEntity.TrustAccountID,
                    AgentFirstName = tEntity.AgentFirstName,
                    AgentLastName = tEntity.AgentLastName,
                    Amount = Math.Round(tEntity.Amount, 2, MidpointRounding.AwayFromZero),
                    AssessmentRollNumber = tEntity.AssessmentRollNumber,
                    BankNumber = tEntity.BankNumber,
                    BranchNumber = tEntity.BranchNumber,
                    City = tEntity.City,
                    Country = tEntity.Country,
                    DisbursementComment = tEntity.DisbursementComment,
                    DisbursementID = tEntity.DisbursementID,
                    FundingDealID = tEntity.FundingDealID,
                    Instructions = tEntity.Instructions,
                    NameOnCheque = tEntity.NameOnCheque,
                    PayeeComments = tEntity.PayeeComments,
                    PayeeID = tEntity.PayeeID,
                    PaymentMethod = tEntity.PaymentMethod,
                    PayeeName = tEntity.PayeeName,
                    PayeeType = tEntity.PayeeType,
                    PostalCode = tEntity.PostalCode,
                    Province = tEntity.Province,
                    ReferenceNumber = tEntity.ReferenceNumber,
                    StreetNumber = tEntity.StreetNumber,
                    StreetAddress1 = tEntity.StreetAddress1,
                    StreetAddress2 = tEntity.StreetAddress2,
                    UnitNumber = tEntity.UnitNumber,
                    ChainDealID = tEntity.ChainDealID,
                    DisbursementStatus = tEntity.DisbursementStatus,
                    FCTFeeSplit = tEntity.FCTFeeSplit,
                    VendorFee = _feeMapper.MapToData(tEntity.VendorFee),
                    PurchaserFee = _feeMapper.MapToData(tEntity.PurchaserFee),
                    PaymentReferenceNumber = tEntity.PaymentReferenceNumber,
                    Reconciled = tEntity.Reconciled,
                    DisbursedAmount = tEntity.DisbursedAmount,
                    AccountHolderName = tEntity.AccountHolderName,
                    Token = tEntity.Token
                    
                };
                return data;
            }
            return null;
        }

        public tblDisbursement MapToEntity(Disbursement tData)
        {
            if (tData != null)
            {
                var entity = new tblDisbursement()
                {
                    AccountAction = tData.AccountAction,
                    AccountNumber = tData.AccountNumber,
                    TrustAccountID = tData.TrustAccountID,
                    AgentFirstName = tData.AgentFirstName,
                    AgentLastName = tData.AgentLastName,
                    Amount = Math.Round(tData.Amount,2,MidpointRounding.AwayFromZero),
                    AssessmentRollNumber = tData.AssessmentRollNumber,
                    BranchNumber = tData.BranchNumber,
                    BankNumber = tData.BankNumber,
                    City = tData.City,
                    Country = tData.Country,
                    DisbursementComment = tData.DisbursementComment,
                    DisbursementID = tData.DisbursementID??default(int),
                    FundingDealID = tData.FundingDealID,
                    PayeeID = tData.PayeeID,
                    Instructions = tData.Instructions,
                    PaymentMethod = tData.PaymentMethod,
                    NameOnCheque = tData.NameOnCheque,
                    PayeeComments = tData.PayeeComments,
                    PayeeName = tData.PayeeName,
                    PayeeType = tData.PayeeType,
                    PostalCode = tData.PostalCode,
                    Province = tData.Province,
                    ReferenceNumber = tData.ReferenceNumber,
                    StreetNumber = tData.StreetNumber,
                    UnitNumber = tData.UnitNumber,
                    StreetAddress1 = tData.StreetAddress1,
                    StreetAddress2 = tData.StreetAddress2,
                    ChainDealID = tData.ChainDealID,
                    DisbursementStatus = tData.DisbursementStatus,
                    FCTFeeSplit = tData.FCTFeeSplit,
                    PaymentReferenceNumber = tData.PaymentReferenceNumber,
                    DisbursedAmount = tData.DisbursedAmount,
                    AccountHolderName = tData.AccountHolderName,
                    Token = tData.Token
                };
                if (tData.VendorFee != null)
                {
                    entity.VendorFeeID = tData.VendorFee.FeeID;
                    
                }
                if (tData.PurchaserFee != null)
                {
                    entity.PurchaserFeeID = tData.PurchaserFee.FeeID;
                }
                return entity;
            }
            return null;
        }
    }
}
