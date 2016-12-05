using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.BusinessService.DataAccess.Mappers;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class MappingResolverModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PINMapper>().As<IEntityMapper<tblPIN, Pin>>();
            builder.RegisterType<LenderMapper>().As<IEntityMapper<tblLender, Lender>>();
            builder.RegisterType<ContactMapper>().As<IEntityMapper<tblDealContact, Contact>>();
            builder.RegisterType<LawyerMapper>().As<IEntityMapper<tblLawyer, Lawyer>>();
            builder.RegisterType<PropertyMapper>().As<IEntityMapper<tblProperty, Property>>();
            builder.RegisterType<MortgagorMapper>().As<IEntityMapper<tblMortgagor, Mortgagor>>();
            builder.RegisterType<VendorMapper>().As<IEntityMapper<tblVendor, Vendor>>();                        
            builder.RegisterType<FundingDealMapper>().As<IEntityMapper<tblDeal, FundingDeal>>();
            builder.RegisterType<DealMapper>().As<IEntityMapper<tblDeal, Deal>>();
            builder.RegisterType<FundsAllocationMapper>()
                .As<IEntityMapper<tblDealFundsAllocation, DealFundsAllocation>>();
            builder.RegisterType<PaymentNotificationMapper>()
                .As<IEntityMapper<tblDealFundsAllocation, PaymentNotification>>();
            builder.RegisterType<FundedDealMapper>().As<IEntityMapper<tblFundingDeal, FundedDeal>>();
            builder.RegisterType<DisbursementMapper>().As<IEntityMapper<tblDisbursement, Disbursement>>();
            builder.RegisterType<DisbursementSummaryMapper>()
                .As<IEntityMapper<tblDisbursementSummary, DisbursementSummary>>();
            builder.RegisterType<FeeMapper>().As<IEntityMapper<tblFee, Fee>>();
            builder.RegisterType<MortgageMapper>().As<IEntityMapper<tblMortgage, Mortgage>>();
            builder.RegisterType<PaymentRequestMapper>().As<IEntityMapper<tblPaymentRequest, LLCPaymentRequest>>();
            builder.RegisterType<EPSToLLCPaymentMapper>()
                .As<IEntityMapper<tblPaymentNotification, PaymentNotification>>();
            builder.RegisterType<DealDocumentTypeMapper>().As<IEntityMapper<tblDealDocumentType, DealDocumentType>>();
            builder.RegisterType<DisbursementDealDocumentTypeMapper>()
                .As<IEntityMapper<tblDisbursementDealDocumentType, DisbursementDealDocumentType>>();
            builder.RegisterType<DocumentTypeMapper>().As<IEntityMapper<tblDocumentType, DocumentType>>();
            builder.RegisterType<BuilderLegalDescriptionMapper>().As<IEntityMapper<tblBuilderLegalDescription, BuilderLegalDescription>>();
            builder.RegisterType<BuilderUnitLevelMapper>().As<IEntityMapper<tblBuilderUnitLevel, BuilderUnitLevel>>();
            builder.RegisterType<QuestionMapper>().As<IEntityMapper<tblQuestion, PifQuestion>>();
            builder.RegisterType<AnswerTypeMapper>().As<IEntityMapper<tblAnswerType, PifAnswerType>>();
            builder.RegisterType<AnswerMapper>().As<IEntityMapper<tblAnswer, PifAnswer>>();
        }
    }
}
