using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PayeeService.Contracts;
using FCT.EPS.PaymentService.DataContracts;
using FCT.LLC.BusinessService.BusinessLogic;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.Common.DataContracts;
using FCT.Services.AuditLog.ServiceContracts;
using FCT.Services.LIM.ServiceContracts;
using Moq;

namespace FCT.LLC.BusinessService.UnitTests
{
    public class ArgumentsFactory
    {
        public Mock<IDealFundsAllocRepository> DealFundsMock { get; private set; }
        public Mock<IDealScopeRepository> DealScopeMock { get; private set; }
        public Mock<IFundingDealRepository> FundingDealMock { get; private set; }
        public Mock<IFundedRepository> FundedMock { get; private set; }
        public Mock<IDisbursementSummaryRepository> SummaryMock { get; private set; }
        public Mock<ILawyerRepository> LawyerMock { get; private set; }
        public Mock<IDealHistoryRepository> DealHistoryMock { get; private set; }
        public Mock<ILIMServiceContract> LimServiceMock { get; private set; }
        public Mock<IAuditLogService> AuditLogMock { get; private set; }
        public Mock<IPaymentNotificationRepository> PaymentNotificationMock { get; private set; }
        public Mock<IDisbursementRepository> DisbursementMock { get; set; }
        public Mock<IPropertyRepository> PropertyMock { get; private set; }
        public Mock<IVendorRepository> VendorMock { get; private set; }
        public Mock<IMortgagorRepository> MortgagorMock { get; private set; }
        public Mock<IDealContactRepository> ContactMock { get; private set; }
        public Mock<IMortgageRepository> MortgageMock { get; private set; }
        public Mock<IPINRepository> PinMock { get; private set; }
        public Mock<IDealAmendmentsChecker> AmendMock { get; private set; }
        public Mock<IFeeCalculator> FeeMock { get; private set; }
        public Mock<IMilestoneUpdater> MilestoneUpdaterMock { get; private set; }
        public Mock<IEmailHelper> EmailMock { get; private set; }
        public Mock<IVendorLawyerHelper> VendorLawyerMock { get; private set; }
        public Mock<IValidationHelper> ValidationMock { get; private set; }
        public Mock<IPaymentTransferService> PaymentServiceMock { get; private set; }
        public Mock<IPaymentRequestProcessor> RequestProcessorMock { get; private set; }
        public Mock<IApplicationLocker> AppLockMock { get; private set; }
        public Mock<IFeeRepository> FeeRepoMock { get; private set; }
        public Mock<IEPSPayeeService> PayeeServiceMock { get; private set; }
        public Mock<IPaymentReportDetailsCollector> ReportMock { get; private set; } 

        public ArgumentsFactory(MockRepository mockRepository)
        {
            DealFundsMock = mockRepository.Create<IDealFundsAllocRepository>();
            DealScopeMock = mockRepository.Create<IDealScopeRepository>();
            FundingDealMock = mockRepository.Create<IFundingDealRepository>();
            FundedMock = mockRepository.Create<IFundedRepository>();
            SummaryMock = mockRepository.Create<IDisbursementSummaryRepository>();
            LawyerMock = mockRepository.Create<ILawyerRepository>();
            DealHistoryMock = mockRepository.Create<IDealHistoryRepository>();
            AuditLogMock = mockRepository.Create<IAuditLogService>();
            PaymentNotificationMock = mockRepository.Create<IPaymentNotificationRepository>();
            DisbursementMock = mockRepository.Create<IDisbursementRepository>();
            LimServiceMock = mockRepository.Create<ILIMServiceContract>();
            PropertyMock = mockRepository.Create<IPropertyRepository>();
            VendorMock = mockRepository.Create<IVendorRepository>();
            MortgagorMock = mockRepository.Create<IMortgagorRepository>();
            ContactMock = mockRepository.Create<IDealContactRepository>();
            MortgageMock = mockRepository.Create<IMortgageRepository>();
            PinMock = mockRepository.Create<IPINRepository>();
            AmendMock = mockRepository.Create<IDealAmendmentsChecker>();
            FeeMock = mockRepository.Create<IFeeCalculator>();
            MilestoneUpdaterMock = mockRepository.Create<IMilestoneUpdater>();
            EmailMock = mockRepository.Create<IEmailHelper>();
            VendorLawyerMock = mockRepository.Create<IVendorLawyerHelper>();
            ValidationMock = mockRepository.Create<IValidationHelper>();
            PaymentServiceMock = mockRepository.Create<IPaymentTransferService>();
            RequestProcessorMock = mockRepository.Create<IPaymentRequestProcessor>();
            AppLockMock = mockRepository.Create<IApplicationLocker>();
            FeeRepoMock = mockRepository.Create<IFeeRepository>();
            PayeeServiceMock = mockRepository.Create<IEPSPayeeService>();
            ReportMock = mockRepository.Create<IPaymentReportDetailsCollector>();
        }

    }
}
