using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PaymentService.DataContracts;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;
using FCT.Services.LIM.MessageContracts;
using FCT.Services.LIM.ServiceContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class PaymentReportDetailsCollector : IPaymentReportDetailsCollector
    {
        private const string NotApplicable = "N/A";

        private readonly IFundingDealRepository _fundingDealRepository;
        private readonly ILawyerRepository _lawyerRepository;

        public PaymentReportDetailsCollector(IFundingDealRepository fundingDealRepository, ILawyerRepository lawyerRepository)
        {
            _fundingDealRepository = fundingDealRepository;
            _lawyerRepository = lawyerRepository;
        }

        public BatchPaymentReport GetBatchPaymentReportDetails(Disbursement disbursement)
        {
            var batchpaymentReport = new BatchPaymentReport();

            var fundingDeal = _fundingDealRepository.GetFundingDeal(disbursement.FundingDealID,false);

            PopulateInfoFromDeal(fundingDeal, batchpaymentReport);

            PopulateInfoFromDisbursement(disbursement, batchpaymentReport);

            PopulateInfoFromLawyerProfile(fundingDeal, batchpaymentReport);

            return batchpaymentReport;
        }

        private static void PopulateInfoFromDisbursement(Disbursement disbursement,
            BatchPaymentReport batchpaymentReport)
        {
            switch (disbursement.PayeeType)
            {
                case PayeeType.Mortgagee:
                    batchpaymentReport.AccountType = "Mortgage";
                    break;
                default:
                    batchpaymentReport.AccountType = disbursement.PayeeType;
                    break;
            }
            batchpaymentReport.Mortgage_Account_Ref = disbursement.ReferenceNumber;
            batchpaymentReport.AccountAction = batchpaymentReport.AccountType == PayeeType.LineOfCredit
                ? disbursement.AccountAction
                : NotApplicable;
        }

        private static void PopulateInfoFromDeal(FundingDeal fundingDeal, BatchPaymentReport batchpaymentReport)
        {
            if (fundingDeal.ActingFor == LawyerActingFor.Mortgagor)
            {
                batchpaymentReport.VendorLawFirmName = fundingDeal.Lawyer.LawFirm;
                batchpaymentReport.VendorLawyerPhoneNumber = fundingDeal.Lawyer.Phone;
                batchpaymentReport.VendorLawyerFileNumber = fundingDeal.LawyerFileNumber;
                batchpaymentReport.VendorLawyerName = string.Format("{0}, {1}", fundingDeal.Lawyer.LastName,
                    fundingDeal.Lawyer.FirstName);

                batchpaymentReport.PurchaserLawFirmName = NotApplicable;
                batchpaymentReport.PurchaserLawyerPhoneNumber = NotApplicable;
                batchpaymentReport.PurchaserLawyerFileNumber = NotApplicable;
                batchpaymentReport.PurchaserLawyerName = NotApplicable;
            }
            else
            {
                if (fundingDeal.ActingFor != LawyerActingFor.Both)
                {
                    batchpaymentReport.VendorLawFirmName = fundingDeal.OtherLawyer.LawFirm;
                    batchpaymentReport.VendorLawyerPhoneNumber = fundingDeal.OtherLawyer.Phone;
                    batchpaymentReport.VendorLawyerFileNumber = fundingDeal.OtherLawyerFileNumber;
                    batchpaymentReport.VendorLawyerName = string.Format("{0}, {1}", fundingDeal.OtherLawyer.LastName,
                        fundingDeal.OtherLawyer.FirstName);
                }
                else
                {
                    batchpaymentReport.VendorLawFirmName = NotApplicable;
                    batchpaymentReport.VendorLawyerPhoneNumber = NotApplicable;
                    batchpaymentReport.VendorLawyerFileNumber = NotApplicable;
                    batchpaymentReport.VendorLawyerName = NotApplicable;
                }

                batchpaymentReport.PurchaserLawFirmName = fundingDeal.Lawyer.LawFirm;
                batchpaymentReport.PurchaserLawyerPhoneNumber = fundingDeal.Lawyer.Phone;
                batchpaymentReport.PurchaserLawyerFileNumber = fundingDeal.LawyerFileNumber;
                batchpaymentReport.PurchaserLawyerName = string.Format("{0}, {1}", fundingDeal.Lawyer.LastName,
                    fundingDeal.Lawyer.FirstName); 
            }
           

            batchpaymentReport.PropertyAddresCity = fundingDeal.Property.City;
            batchpaymentReport.PropertyAddresPostalCode = fundingDeal.Property.PostalCode;
            batchpaymentReport.PropertyAddresProvince = fundingDeal.Property.Province;
            batchpaymentReport.PropertyAddresStreetNumber = fundingDeal.Property.StreetNumber;
            batchpaymentReport.PropertyAddressUnitNumber = fundingDeal.Property.UnitNumber;
            batchpaymentReport.PropertyAddresStreetName = fundingDeal.Property.Address;

            var pins = fundingDeal.Property.Pins.Select(p => p.PINNumber);
            batchpaymentReport.PIN = string.Join(",", pins);

            batchpaymentReport.PropertyAddress = FormatPropertyAddress(fundingDeal.Property);

            PopulatePurchaser(fundingDeal, batchpaymentReport);
            PopulateVendor(fundingDeal, batchpaymentReport);
        }

        private static void PopulatePurchaser(FundingDeal fundingDeal, BatchPaymentReport batchpaymentReport)
        {
            if (fundingDeal.ActingFor == LawyerActingFor.Mortgagor)
            {
                batchpaymentReport.PurchaserName = NotApplicable;
            }
            else
            {
                var mortgagornames = FormatMortgagorNames(fundingDeal);
                batchpaymentReport.PurchaserName = string.Join(";", mortgagornames);  
            }

        }

        private static IEnumerable<string> FormatMortgagorNames(FundingDeal fundingDeal)
        {
            IList<string> mortgagornames = new List<string>();
            foreach (var mortgagor in fundingDeal.Mortgagors)
            {
                if (mortgagor.MortgagorType.ToUpper() == PartyType.Business)
                {
                    mortgagornames.Add(mortgagor.CompanyName);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(mortgagor.MiddleName))
                    {
                        mortgagornames.Add(string.Format("{0}, {1}", mortgagor.LastName, mortgagor.FirstName));
                    }
                    else
                    {
                        mortgagornames.Add(string.Format("{0}, {1} {2}", mortgagor.LastName,
                            mortgagor.FirstName, mortgagor.MiddleName));
                    }
                }
            }
            return mortgagornames;
        }

        private static void PopulateVendor(FundingDeal fundingDeal, BatchPaymentReport batchpaymentReport)
        {
            IEnumerable<string> vendornames = fundingDeal.ActingFor == LawyerActingFor.Mortgagor
                ? FormatMortgagorNames(fundingDeal)
                : FormatVendorNames(fundingDeal);
            batchpaymentReport.VendorName = string.Join(";", vendornames);
        }

        private static IEnumerable<string> FormatVendorNames(FundingDeal fundingDeal)
        {
            IList<string> vendornames = new List<string>();
            foreach (var vendor in fundingDeal.Vendors)
            {
                if (vendor.VendorType.ToUpper() == PartyType.Business)
                {
                    vendornames.Add(vendor.CompanyName);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(vendor.MiddleName))
                    {
                        vendornames.Add(string.Format("{0}, {1}", vendor.LastName, vendor.FirstName));
                    }
                    else
                    {
                        vendornames.Add(string.Format("{0}, {1} {2}", vendor.LastName,
                            vendor.FirstName, vendor.MiddleName));
                    }
                }
            }
            return vendornames;
        }

        private static string FormatPropertyAddress(Property property)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(property.UnitNumber))
            {
                sb.Append(property.UnitNumber);
                sb.Append(",");
            }
            if (!string.IsNullOrWhiteSpace(property.StreetNumber))
            {
                sb.Append(property.StreetNumber);
            }
            if (!string.IsNullOrWhiteSpace(property.Address))
            {
                sb.Append(" ");
                sb.Append(property.Address);
            }
            if (!string.IsNullOrWhiteSpace(property.Address2))
            {
                sb.Append(",");
                sb.Append(property.Address2);
            }
            return sb.ToString();
        }

        private void PopulateInfoFromLawyerProfile(FundingDeal fundingDeal, BatchPaymentReport batchPaymentReport)
        {
            var lawyerprofile = _lawyerRepository.GetNotificationDetails(fundingDeal.Lawyer.LawyerID);
           
            if (fundingDeal.ActingFor == LawyerActingFor.Mortgagor)
            {
                batchPaymentReport.VendorLawyerEmailAddress = lawyerprofile.Email;
                batchPaymentReport.VendorLawyerFax = lawyerprofile.Fax;
                batchPaymentReport.PurchaserLawyerEmailAddress = NotApplicable;
                batchPaymentReport.PurchaserLawyerFax = NotApplicable;
            }
            else
            {
                batchPaymentReport.PurchaserLawyerEmailAddress = lawyerprofile.Email;
                batchPaymentReport.PurchaserLawyerFax = lawyerprofile.Fax;
                if (fundingDeal.ActingFor != LawyerActingFor.Both)
                {
                    var otherLawyerprofile = _lawyerRepository.GetNotificationDetails(fundingDeal.OtherLawyer.LawyerID);
                    batchPaymentReport.VendorLawyerFax = otherLawyerprofile.Fax;
                    batchPaymentReport.VendorLawyerEmailAddress = otherLawyerprofile.Email;
                }
                else
                {
                    batchPaymentReport.VendorLawyerFax = NotApplicable;
                    batchPaymentReport.VendorLawyerEmailAddress = NotApplicable;
                }
            }           

            
        }

    }
}
