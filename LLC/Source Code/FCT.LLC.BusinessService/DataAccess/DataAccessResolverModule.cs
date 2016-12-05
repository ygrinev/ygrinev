using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FCT.LLC.BusinessService.DataAccess.Implementations;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.GenericRepository.Contracts;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class DataAccessResolverModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EFBusinessContext>();
            builder.RegisterType<DealHistoryHelper>();
            builder.RegisterType<ReadOnlyDataHelper>();
            builder.RegisterType<FundingDealRepository>().As<IFundingDealRepository>();
            builder.RegisterType<DealScopeRepository>().As<IDealScopeRepository>();
            builder.RegisterType<PropertyRepository>().As<IPropertyRepository>();
            builder.RegisterType<VendorRepository>().As<IVendorRepository>();
            builder.RegisterType<MortgagorRepository>().As<IMortgagorRepository>();
            builder.RegisterType<DealContactRepository>().As<IDealContactRepository>();
            builder.RegisterType<MortgageRepository>().As<IMortgageRepository>();
            builder.RegisterType<PINRepository>().As<IPINRepository>();
            builder.RegisterType<LawyerRepository>().As<ILawyerRepository>();
            builder.RegisterType<DealSearchRepository>().As<IDealSearchRepository>();
            builder.RegisterType<DealRepository>().As<IDealRepository>();
            builder.RegisterType<DealHistoryRepository>().As<IDealHistoryRepository>();
            builder.RegisterType<DealFundsAllocRepository>().As<IDealFundsAllocRepository>();
            builder.RegisterType<FundedRepository>().As<IFundedRepository>();
            builder.RegisterType<DisbursementSummaryRepository>().As<IDisbursementSummaryRepository>();
            builder.RegisterType<GlobalizationRepository>().As<IGlobalizationRepository>();
            builder.RegisterType<DisbursementRepository>().As<IDisbursementRepository>();
            builder.RegisterType<FeeRepository>().As<IFeeRepository>();
            builder.RegisterType<PayoutLetterWorklistRepository>().As<IPayoutLetterWorklistRepository>();
            builder.RegisterType<PaymentRequestRepository>().As<IPaymentRequestRepository>();
            builder.RegisterType<PaymentNotificationRepository>().As<IPaymentNotificationRepository>();
            builder.RegisterType<ReconciliationItemsRepository>().As<IReconciliationItemsRepository>();
            builder.RegisterType<ApplicationLocker>().As<IApplicationLocker>();
            builder.RegisterType<DocumentTypeRepository>().As<IDocumentTypeRepository>();
            builder.RegisterType<DisbursementDealDocumentTypeRepository>().As<IDisbursementDealDocumentTypeRepository>();
            builder.RegisterType<DealDocumentTypeRepository>().As<IDealDocumentTypeRepository>();
            builder.RegisterType<BuilderLegalDescriptionRepository>().As<IBuilderLegalDescriptionRepository>();
            builder.RegisterType<BuilderUnitLevelRepository>().As<IBuilderUnitLevelRepository>();
            builder.RegisterType<PifQuestionRepository>().As<IPifQuestionRepository>();
            builder.RegisterType<PifAnswerTypeRepository>().As<IPifAnswerTypeRepository>();
            builder.RegisterType<PifAnswerRepository>().As<IPifAnswerRepository>();
        }
    }
}
