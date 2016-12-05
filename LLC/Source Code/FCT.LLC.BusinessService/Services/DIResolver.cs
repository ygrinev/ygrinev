using System.Configuration;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Web.Compilation;
using Autofac;
using Autofac.Integration.Wcf;
using Autofac.Integration.WebApi;
using FCT.EPS.PayeeService.Contracts;
using FCT.EPS.PaymentService.DataContracts;
using FCT.LLC.BusinessService.BusinessLogic;
using FCT.LLC.BusinessService.BusinessLogic.Implementations;
using FCT.LLC.BusinessService.BusinessLogic.Interfaces;
using FCT.LLC.BusinessService.Contracts;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Logging;
using FCT.Services.AuditLog.ServiceContracts;
using FCT.Services.LIM.ServiceContracts;

namespace FCT.LLC.BusinessService
{
    public static class DIResolver
    {
        private const string SectionName = "system.serviceModel/client";

        public static IContainer RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EnterpriseLibraryLogger>().As<ILogger>();
            builder.RegisterModule(new DataAccessResolverModule());
            builder.RegisterModule(new MappingResolverModule());
            builder.RegisterType<ValidationHelper>().As<IValidationHelper>();
            builder.RegisterType<PaymentRequestConverter>().As<IPaymentRequestConverter>();
            builder.RegisterType<PaymentRequestProcessor>().As<IPaymentRequestProcessor>();
            builder.RegisterType<DealAmendmentsChecker>().As<IDealAmendmentsChecker>();
            builder.RegisterType<FeeCalculator>().As<IFeeCalculator>();
            builder.RegisterType<EmailHelper>().As<IEmailHelper>();
            builder.RegisterType<ReturnFundsHelper>().As<IReturnFundsHelper>();
            builder.RegisterType<VendorLawyerHelper>().As<IVendorLawyerHelper>();
            builder.RegisterType<CancelDealHelper>().As<ICancelDealHelper>();
            builder.RegisterType<FundsAllocationHelper>();
            builder.RegisterType<MilestoneUpdater>().As<IMilestoneUpdater>();
            builder.RegisterType<DealManagementBusinessLogic>().As<IDealManagementBusinessLogic>();
            builder.RegisterType<LLCBusinessService>().As<ILLCBusinessService>();
            builder.RegisterType<PaymentTrackingService>().As<IPaymentTrackingService>();
            builder.RegisterType<DealSearchBusinessLogic>().As<IDealSearchBusinessLogic>();
            builder.RegisterType<DealBusinessLogic>().As<IDealBusinessLogic>();
            builder.RegisterType<FundsAllocBusinessLogic>().As<IFundsAllocBusinessLogic>();
            builder.RegisterType<PaymentBusinessLogic>().As<IPaymentBusinessLogic>();
            builder.RegisterType<DisbursementBusinessLogic>().As<IDisbursementBusinessLogic>();
            builder.RegisterType<AcceptDealBusinessLogic>().As<IAcceptDealBusinessLogic>();
            builder.RegisterType<SignDealBusinessLogic>().As<ISignDealBusinessLogic>();
            builder.RegisterType<PayoutLetterBusinessLogic>().As<IPayoutLetterBusinessLogic>();
            builder.RegisterType<DeclineDealBusinessLogic>().As<IDeclineDealBusinessLogic>();
            builder.RegisterType<CancelDealBusinessLogic>().As<ICancelDealBusinessLogic>();
            builder.RegisterType<ReturnFundsBusinessLogic>().As<IReturnFundsBusinessLogic>();
            builder.RegisterType<ReconciliationBusinessLogic>().As<IReconciliationBusinessLogic>();
            builder.RegisterType<PaymentReportDetailsCollector>().As<IPaymentReportDetailsCollector>();
            builder.RegisterType<ConfirmationLetterBusinessLogic>().As<IConfirmationLetterBusinessLogic>();
            builder.RegisterType<PifQuestionsBusinessLogic>().As<IPifQuestionsBusinessLogic>();
            builder.RegisterType<DealEventsPublisher>().As<IDealEventsPublisher>();
            builder.RegisterType<DocumentBusinessLogic>().As<IDocumentBusinessLogic>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
            InjectServiceProxies(builder);
            return builder.Build();
        }

        private static void InjectServiceProxies(ContainerBuilder builder)
        {
            var clientSection = (ClientSection) ConfigurationManager.GetSection(SectionName);
            for (int i = 0; i < clientSection.Endpoints.Count; i++)
            {
                switch (clientSection.Endpoints[i].Name)
                {
                    case "LIMService":
                        var limEndPoint = clientSection.Endpoints[i].Address;
                        builder
                            .Register(c => new ChannelFactory<ILIMServiceContract>("LIMService")).SingleInstance();
                        builder
                            .Register(c => c.Resolve<ChannelFactory<ILIMServiceContract>>().CreateChannel())
                            .As<ILIMServiceContract>()
                            .UseWcfSafeRelease();
                        break;
                    case "AuditLogServiceEndpoint":
                        var auditLogEndPoint = clientSection.Endpoints[i].Address;
                        builder
                            .Register(c => new ChannelFactory<IAuditLogService>("AuditLogServiceEndpoint")).SingleInstance();
                        builder
                            .Register(c => c.Resolve<ChannelFactory<IAuditLogService>>().CreateChannel())
                            .As<IAuditLogService>()
                            .UseWcfSafeRelease();
                        break;
                    case "PaymentService":
                        var paymentEndPoint = clientSection.Endpoints[i].Address;
                        builder
                            .Register(c => new ChannelFactory<IPaymentTransferService>("PaymentService")).SingleInstance();
                        builder
                            .Register(c => c.Resolve<ChannelFactory<IPaymentTransferService>>().CreateChannel())
                            .As<IPaymentTransferService>()
                            .UseWcfSafeRelease();
                        break;
                    case "PayeeService":
                        var payeeEndPoint = clientSection.Endpoints[i].Address;
                        builder
                            .Register(c => new ChannelFactory<IEPSPayeeService>("PayeeService")).SingleInstance();
                        builder
                            .Register(c => c.Resolve<ChannelFactory<IEPSPayeeService>>().CreateChannel())
                            .As<IEPSPayeeService>()
                            .UseWcfSafeRelease();
                        break;
                }
            }
        }
    }
}
