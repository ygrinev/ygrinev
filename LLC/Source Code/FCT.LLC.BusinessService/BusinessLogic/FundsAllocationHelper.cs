using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using FCT.Common.Configuration;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.Common.NotificationEmailDispatching.Client;
using FCT.LLC.Common.NotificationEmailDispatching.Entities;
using FCT.Services.LIM.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class FundsAllocationHelper
    {
        private readonly IVendorLawyerHelper _lawyerHelper;
        public FundsAllocationHelper(IVendorLawyerHelper vendorLawyerHelper)
        {
            _lawyerHelper = vendorLawyerHelper;
        }
        private const string DBContextName = "EFBusinessContext";

        //logic won't be valid after 2099
        public static string IsProbableFCTURN(string probableFCTRef)
        {
            var formattedString = new string(probableFCTRef.Where(char.IsDigit).ToArray());
            if (formattedString.Length <= 11 && formattedString.Length >= 5)
            {
                string probYear = "20" + formattedString.Substring(0, 2);
                string probDay = formattedString.Substring(2, 3);
                int year;
                if (int.TryParse(probYear, out year))
                {
                    if (2000 <= year && year <= 2099)
                    {
                        int dayofYear;
                        if (int.TryParse(probDay, out dayofYear))
                        {
                            if (dayofYear >= 001 && dayofYear <= 366)
                            {
                                return formattedString;
                            }
                        }
                    }
                }
            }

            return string.Empty;
        }

        public static bool IsProbableWireCode(string possibleWireCode)
        {
            var formattedWireCode = new string(possibleWireCode.Where(char.IsLetterOrDigit).ToArray());
            if (formattedWireCode.Length == FCTURNHelper.WireDepositCodeLength)
            {
                var arr = formattedWireCode.ToCharArray();
                var rules=new bool[arr.Length];

                for (int i = 0; i < arr.Length; i++)
                {
                    if (i == 0 || i % 2 == 0)
                    {
                        if (char.IsDigit(arr[i]))
                        {
                            rules[i] = true;
                        }
                    }
                    else
                    {
                        if (char.IsLetter(arr[i]))
                        {
                            rules[i] = true;
                        }
                        
                    }
                }
                if (rules.Any(r => r.Equals(false)))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        internal static void SendNotificationEmail(FundingDeal dealInfo, LawyerProfile lawyerProfile,
            StandardNotificationKey notificationKey, string mailingList = null)
        {
            var connection = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings[DBContextName].ConnectionString
            };
            IDictionary<string, object> tokens;
            Mortgagor firstMortgagor;

            string toEmails = !string.IsNullOrWhiteSpace(mailingList) ? mailingList : lawyerProfile.Email;

            string actingFor = string.Empty;
            if (dealInfo.ActingFor == LawyerActingFor.Mortgagor)
                actingFor = LawyerActingFor.Mortgagor;
            else
                actingFor = LawyerActingFor.Purchaser;
            switch (notificationKey)
            {
                case StandardNotificationKey.EFDepositFundsMatch:
                    tokens = EmailHelper.CreateEmailTokens(actingFor, FCTURNHelper.FormatShortFCTURN(dealInfo.FCTURN),
                        dealInfo);

                    firstMortgagor = dealInfo.Mortgagors.FirstOrDefault();
                    if (firstMortgagor != null)
                    {
                        EmailHelper.ConcatFileInformation(dealInfo, actingFor,
                            firstMortgagor.MortgagorType == "Person"
                                ? firstMortgagor.LastName
                                : firstMortgagor.CompanyName, tokens);
                    }
                    else
                    {
                        EmailHelper.ConcatFileInformation(dealInfo, actingFor,
                                "", tokens);
                    }
                    tokens.Add(EmailTemplateTokenList.LawyerFileNumber, dealInfo.LawyerFileNumber);

                    EasyFundDispatcher.SendDealNotificationEmailToSpecificEmailId(connection,
                        lawyerProfile.UserLanguage, StandardNotificationKey.EFDepositFundsMatch, tokens,
                        toEmails);
                    break;

                case StandardNotificationKey.EFDepositFundsMismatch:
                    tokens = EmailHelper.CreateEmailTokens(actingFor, FCTURNHelper.FormatShortFCTURN(dealInfo.FCTURN),
                        dealInfo);
                    firstMortgagor = dealInfo.Mortgagors.FirstOrDefault();
                    if (firstMortgagor != null)
                    {
                        EmailHelper.ConcatFileInformation(dealInfo, actingFor,
                            firstMortgagor.MortgagorType == "Person"
                                ? firstMortgagor.LastName
                                : firstMortgagor.CompanyName, tokens);
                    }
                    else 
                    {
                        EmailHelper.ConcatFileInformation(dealInfo, actingFor,
                           "", tokens);
                    }
                    tokens.Add(EmailTemplateTokenList.LawyerFileNumber, dealInfo.LawyerFileNumber);

                    EasyFundDispatcher.SendDealNotificationEmailToSpecificEmailId(connection,
                        lawyerProfile.UserLanguage, StandardNotificationKey.EFDepositFundsMismatch, tokens,
                        toEmails);
                    break;

                case StandardNotificationKey.EFDepositVendor:
                    tokens = EmailHelper.CreateEmailTokens(LawyerActingFor.Vendor, FCTURNHelper.FormatShortFCTURN(dealInfo.FCTURN),
                        dealInfo);
                    Vendor lastVendor = dealInfo.Vendors.FirstOrDefault();
                    if (lastVendor != null)
                    {
                        EmailHelper.ConcatFileInformation(dealInfo, LawyerActingFor.Vendor,
                            lastVendor.VendorType == "Person" ? lastVendor.LastName : lastVendor.CompanyName, tokens);
                    }
                    else
                    {
                        EmailHelper.ConcatFileInformation(dealInfo, LawyerActingFor.Vendor,"", tokens);
                    }
                    tokens.Add(EmailTemplateTokenList.LawyerFileNumber, dealInfo.LawyerFileNumber);
                    EasyFundDispatcher.SendDealNotificationEmailToSpecificEmailId(connection,
                        lawyerProfile.UserLanguage, StandardNotificationKey.EFDepositVendor, tokens,
                        toEmails);
                    break;
            }
        }

        internal static void SendNotificationEmail(DealFundsAllocation dealFunds, string lawyerlanguage,
            string mailingList)
        {
            var connection = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings[DBContextName].ConnectionString
            };
            IDictionary<string, object> tokens = new Dictionary<string, object>()
            {
                {EmailTemplateTokenList.DepositDate, dealFunds.DepositDate},
                {EmailTemplateTokenList.DepositAmount, dealFunds.Amount.ToString("C",new CultureInfo("en-US"))},
                {EmailTemplateTokenList.DepositDateFR, dealFunds.DepositDate.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("fr-CA"))},
                {EmailTemplateTokenList.DepositAmountFR, dealFunds.Amount.ToString("C",new CultureInfo("fr-CA"))},
                {EmailTemplateTokenList.WireDepositDetails, string.Format("({0})",dealFunds.WireDepositDetails)}
            };
            EasyFundDispatcher.SendDealNotificationEmailToSpecificEmailId(connection,
                lawyerlanguage, StandardNotificationKey.EFDepositDealNotFound, tokens,
                mailingList);
        }

        internal static void SendNotificationEmail(string email)
        {
            var connection = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings[DBContextName].ConnectionString
            };
            IDictionary<string, object> tokens = new Dictionary<string, object>();
            EasyFundDispatcher.SendDealNotificationEmailToSpecificEmailId(connection, "English",
                StandardNotificationKey.EFUnAllocatedFunds, tokens, email);
        }

        internal static string GetIPAddress()
        {
            System.ServiceModel.OperationContext context = System.ServiceModel.OperationContext.Current;
            if (context != null)
            {
                System.ServiceModel.Channels.MessageProperties prop = context.IncomingMessageProperties;
                var endpoint =
                   prop[System.ServiceModel.Channels.RemoteEndpointMessageProperty.Name] as System.ServiceModel.Channels.RemoteEndpointMessageProperty;
                if (endpoint != null)
                {
                    string ip = endpoint.Address;
                    //if IP is localhost, get local machine's IP
                    if (ip == "::1")
                    {
                        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                        foreach (IPAddress ipAddress in host.AddressList)
                        {
                            if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                            {
                                ip = ipAddress.ToString();
                            }
                        }
                    }
                    return ip;
                }  
            }           
            return string.Empty;
        }

        internal static string FormatDuplicatesErrorMessage(PaymentNotification paymentNotification)
        {
            const string error = "Duplicate payment request.";
            string message = string.Format("{0}. Payment Reference Number: {1}, Amount:{2}, AdditionalInfo: {3}", error,
                paymentNotification.PaymentReferenceNumber, paymentNotification.PaymentAmount,
                paymentNotification.AdditionalInformation);
            return message;
        }

        public string GetLawyerRegistration(string userName)
        {
            var businessModel = new StringBuilder();

            var userProfile = _lawyerHelper.GetUserProfile(userName);
            if (userProfile != null)
            {
                foreach (var registration in userProfile.Registrations)
                {
                    switch (registration.SolutionID)
                    {
                        case (int)SolutionType.LLC:
                            if (registration.UserStatusID == (int)UserStatus.Active)
                            {
                                businessModel.Append(SolutionType.LLC);
                            }
                            break;
                        case (int)SolutionType.EASYFUND:
                            if (registration.UserStatusID == (int)UserStatus.Active)
                            {
                                if (businessModel.Length > 0)
                                {
                                    businessModel.Append("/");
                                }

                                businessModel.Append(SolutionType.EASYFUND);
                            }
                            break;

                    }
                }
                return businessModel.ToString();
            }
            return string.Empty;
        }

    }
}
