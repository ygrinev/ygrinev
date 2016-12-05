using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PaymentService.DataContracts;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public static class PaymentServiceMapper
    {
        internal static Address MapAddress(EPS.PayeeService.DataContracts.Address address)
        {
            if (address != null)
            {
                var paymentAddress = new Address
                {
                    City = address.City,
                    Country = address.Country,
                    PostalCode = address.PostalCode,
                    StreetAddress1 = address.StreetAddress1,
                    StreetAddress2 = address.StreetAddress2,
                    StreetNumber = address.StreetNumber,
                    UnitNumber = address.UnitNumber,
                    ProvinceCode = address.ProvinceCode
                };
                return paymentAddress;
            }
            return null;
        }

        internal static Address MapAddress(Disbursement disbursement)
        {
            var address = new Address()
            {
                City = disbursement.City,
                Country = disbursement.Country,
                PostalCode = disbursement.PostalCode,
                ProvinceCode = disbursement.Province,
                StreetAddress1 = disbursement.StreetAddress1,
                StreetAddress2 = disbursement.StreetAddress2,
                StreetNumber = disbursement.StreetNumber
            };
            return address;
        }

        internal static Address MapAddress(Services.LIM.DataContracts.Address address)
        {
            if (address != null)
            {
                var paymentAddress = new Address()
                {
                    City = address.City,
                    Country = address.Country,
                    PostalCode = address.PostalCode,
                    ProvinceCode = address.Province,
                    StreetAddress1 = address.AddressLine1,
                    StreetAddress2 = address.AddressLine2,
                    StreetNumber = address.StreetNumber,
                    UnitNumber = address.UnitNumber,
                };
                return paymentAddress;
            }
            return null;
        }

        internal static Account MapAccount(EPS.PayeeService.DataContracts.Account account)
        {
            if (account != null)
            {
                var paymentAccount = new Account
                {
                    AccountNumber = account.AccountNumber,
                    BankName = account.BankName,
                    BankNumber = account.BankNumber,
                    TransitNumber = account.TransitNumber,
                    CanadianClearingCode = account.CanadianClearingCode,
                    SWIFTBIC = account.SWIFTBIC,
                    BankAddress = MapAddress(account.BankAddress)
                };
                return paymentAccount;
            }
            return null;
        }

        internal static Account MapAccount(Disbursement disbursement)
        {
            if (disbursement != null)
            {
                var paymentAccount = new Account
                {
                    AccountNumber = disbursement.AccountNumber,
                    BankNumber = disbursement.BankNumber,
                    TransitNumber = disbursement.BranchNumber,
                };
                return paymentAccount;
            }
            return null;
        }

        internal static PayeeInfo MapPayeeInfo(EPS.PayeeService.DataContracts.PayeeInfo payeeInfo)
        {
            if (payeeInfo != null)
            {
                var paymentPayeeInfo = new PayeeInfo
                {
                    PayeeAddress = MapAddress(payeeInfo.PayeeAddress),
                    PayeeAccount = MapAccount(payeeInfo.PayeeAccount),
                    PayeeBankAccountHolderName = payeeInfo.PayeeBankAccountHolderName,
                    PayeeContact = payeeInfo.PayeeContact,
                    PayeeContactPhoneNumber = payeeInfo.PayeeContactPhoneNumber,
                    PayeeEmail = payeeInfo.PayeeEmail,
                    PayeeName = payeeInfo.PayeeName,
                    PayeeInfoID = payeeInfo.PayeeInfoID,
                    BatchScheduleTimeList = MapBatchScheduleTimeList(payeeInfo.BatchScheduleTimeList)
                };
                return paymentPayeeInfo;
            }
            return null;
        }

        internal static PayeeInfo MapPayeeInfo(Disbursement disbursement)
        {
            if (disbursement != null)
            {
                var paymentPayeeInfo = new PayeeInfo
                {
                    PayeeAddress = MapAddress(disbursement),
                    PayeeAccount = MapAccount(disbursement),
                    PayeeName = disbursement.NameOnCheque,
                    PayeeInfoID = disbursement.PayeeID.GetValueOrDefault(),
                    PayeeReferenceNumber = disbursement.ReferenceNumber,
                };
                return paymentPayeeInfo;
            }
            return null;
        }

        internal static BatchScheduleTimeList MapBatchScheduleTimeList(
            EPS.PayeeService.DataContracts.BatchScheduleTimeList batchScheduleTimeList)
        {
            var list = new BatchScheduleTimeList();
            list.AddRange(batchScheduleTimeList.Select(batchSchedule => new BatchScheduleTime()
            {
                BatchScheduleTimeInfo = batchSchedule.BatchScheduleTimeInfo,
            }));
            return list;
        }

        public static string MapDebtorName(FundingDeal fundingDeal, string payeetype)
        {
            const int maxCount = 6;
            const int maxLength = 80;
            string debtorname;
            if (payeetype == PayeeType.MortgageBroker || payeetype == PayeeType.Builder)
            {
                var names = GetMortgagorNames(fundingDeal, maxCount);
                debtorname = string.Join(",", names);
            }
            else
            {
                if (fundingDeal.DealType == DealType.Refinance)
                {
                    IEnumerable<string> mortgagornames = GetMortgagorNames(fundingDeal, maxCount);
                    debtorname = string.Join(",", mortgagornames);
                }
                else
                {
                    var vendors = fundingDeal.Vendors.Count <= maxCount
                        ? fundingDeal.Vendors
                        : fundingDeal.Vendors.Take(maxCount);
                    IEnumerable<string> vendornames =
                        vendors.Select(
                            vendor =>
                                vendor.VendorType.ToUpper() == PartyType.Business ? vendor.CompanyName : vendor.LastName);
                    debtorname = string.Join(",", vendornames);
                }
            }

            if (debtorname.Length > maxLength)
            {
                var sb = new StringBuilder();
                var names = debtorname.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToArray();
                int namectr = names.Count();

                for (int i = 0; i < namectr; i++)
                {
                    if (sb.Length + names[i].Length <= maxLength)
                    {
                        sb.Append(names[i]);
                        sb.Append(",");
                    }
                }
                debtorname = sb.ToString().TrimEnd(new[] {','});
            }
            return debtorname;
        }

        private static IEnumerable<string> GetMortgagorNames(FundingDeal fundingDeal, int maxCount)
        {
            var mortgagors = fundingDeal.Mortgagors.Count <= maxCount
                ? fundingDeal.Mortgagors
                : fundingDeal.Mortgagors.Take(maxCount);
            IEnumerable<string> names =
                mortgagors.Select(
                    mortgagor =>
                        mortgagor.MortgagorType.ToUpper() == PartyType.Business ? mortgagor.CompanyName : mortgagor.LastName);
            return names;
        }
    }
}
