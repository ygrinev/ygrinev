using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.Common.NotificationEmailDispatching.Client;
using FCT.LLC.Common.NotificationEmailDispatching.Entities;
using FCT.Services.LIM.DataContracts;
using FCT.Services.LIM.MessageContracts;
using FCT.Services.LIM.ServiceContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class EmailHelper : IEmailHelper 
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMortgagorRepository _mortgagorRepository;
        private readonly IFundingDealRepository _fundingDealRepository;
        private readonly ILIMServiceContract _limService;
        private readonly ILawyerRepository _lawyerRepository;
        private readonly IDealContactRepository _dealContactRepository;
        private readonly IDealRepository _dealRepository;

        public EmailHelper(IVendorRepository vendorRepository, IMortgagorRepository mortgagorRepository,
            IFundingDealRepository fundingDealRepository, ILIMServiceContract limService,
            IDealContactRepository dealContactRepository, ILawyerRepository lawyerRepository, IDealRepository dealRepository)
        {
            _vendorRepository = vendorRepository;
            _mortgagorRepository = mortgagorRepository;
            _fundingDealRepository = fundingDealRepository;
            _limService = limService;
            _lawyerRepository = lawyerRepository;
            _dealContactRepository = dealContactRepository;
            _dealRepository = dealRepository;
        }
        internal const string DBContextName = "EFBusinessContext";
        internal static IDictionary<string, object> CreateEmailTokens(string party, string fctURN, FundingDeal requestDeal)
        {
            var tokens = new Dictionary<string, object>
            {
                {EmailTemplateTokenList.SolicitorFirstName, requestDeal.Lawyer.FirstName},
                {EmailTemplateTokenList.SolicitorLastName, requestDeal.Lawyer.LastName},
                {EmailTemplateTokenList.FCTURN, fctURN},
                //{EmailTemplateTokenList.DealClosingDate, requestDeal.ClosingDate.GetValueOrDefault().ToString("MMMM dd, yyyy")},
                {EmailTemplateTokenList.PropertyAddress, ConcatPropertyAddress(requestDeal.Property)},
            };

            if (requestDeal.ClosingDate != null)
            {
                tokens.Add(EmailTemplateTokenList.DealClosingDate, requestDeal.ClosingDate.GetValueOrDefault().ToString("MMMM d, yyyy"));
                tokens.Add(EmailTemplateTokenList.DealClosingDateFr, requestDeal.ClosingDate.GetValueOrDefault().ToString("d MMMM yyyy", new CultureInfo("fr-FR")));
            }
            else
            {
                tokens.Add(EmailTemplateTokenList.DealClosingDate, "Closing Date TBD");
                tokens.Add(EmailTemplateTokenList.DealClosingDateFr, "Date de clôture à être déterminée");
            }
            if (party == LawyerActingFor.Vendor)
            {
                party = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(party.ToLower());
                tokens.Add(EmailTemplateTokenList.Party, party);
                tokens.Add(EmailTemplateTokenList.PartyFr, "Vendeur");
                tokens.Add(EmailTemplateTokenList.PartyContact, ConcatCollection(requestDeal.Vendors));
            }
            if (party == LawyerActingFor.Purchaser)
            {
                party = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(party.ToLower());
                tokens.Add(EmailTemplateTokenList.Party, party);
                tokens.Add(EmailTemplateTokenList.PartyFr, "Acheteur");
                tokens.Add(EmailTemplateTokenList.PartyContact, ConcatCollection(requestDeal.Mortgagors));
            }
            if (party == LawyerActingFor.Mortgagor)
            {
                party = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(party.ToLower());
                tokens.Add(EmailTemplateTokenList.Party, party);
                tokens.Add(EmailTemplateTokenList.PartyFr, "Débiteur hypothécaire");
                tokens.Add(EmailTemplateTokenList.PartyContact, ConcatCollection(requestDeal.Mortgagors));
            }
            if (party == LawyerActingFor.Both)
            {
                party = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(party.ToLower());
                tokens.Add(EmailTemplateTokenList.Party, "Purchaser");
                tokens.Add(EmailTemplateTokenList.PartyFr, "Acheteur");
                tokens.Add(EmailTemplateTokenList.PartyContact, ConcatCollection(requestDeal.Mortgagors));
            }
            return tokens;
        }


        private static string ConcatPropertyAddress(Property property)
        {
            if (property != null)
            {
                var emailComponents = new List<string>();
                if (!string.IsNullOrWhiteSpace(property.Address))
                {
                    if (!string.IsNullOrWhiteSpace(property.StreetNumber))
                    {
                        emailComponents.Add(!string.IsNullOrWhiteSpace(property.UnitNumber)
                            ? string.Format("{0}-{1} {2}", property.UnitNumber, property.StreetNumber, property.Address)
                            : string.Format("{0} {1}", property.StreetNumber, property.Address));
                    }
                    else
                    {
                        emailComponents.Add(!string.IsNullOrWhiteSpace(property.UnitNumber)
                            ? string.Format("{0} {1}", property.UnitNumber, property.Address)
                            : string.Format("{0}", property.Address));
                    }

                }
                if (!string.IsNullOrWhiteSpace(property.City))
                {
                    emailComponents.Add(property.City);
                }
                if (!string.IsNullOrWhiteSpace(property.Province))
                {
                    emailComponents.Add(property.Province);
                }
                if (!string.IsNullOrWhiteSpace(property.PostalCode))
                {
                    emailComponents.Add(property.PostalCode);
                }

                return String.Join(", ", emailComponents);
            }
            return string.Empty;
        }

        private static string ConcatCollection(IEnumerable<Vendor> vendors)
        {
            if (vendors != null)
            {
                var sb = new StringBuilder();
                foreach (var vendor in vendors)
                {
                    if (sb.Length != 0) sb.Append("; ");
                    if (!string.IsNullOrWhiteSpace(vendor.CompanyName))
                    {
                        var emailcomponents = new List<string> { vendor.CompanyName, string.Concat(vendor.FirstName, " ", vendor.LastName).Trim() };
                        sb.Append(String.Join(", ", emailcomponents.FindAll(c => !string.IsNullOrWhiteSpace(c))));
                    }
                    else
                    {
                        var emailcomponents = new List<string> { vendor.LastName, vendor.FirstName };
                        sb.Append(String.Join(", ", emailcomponents.FindAll(c => !string.IsNullOrWhiteSpace(c))));
                    }
                }
                return sb.ToString();
            }
            return string.Empty;
        }

        private static string ConcatCollection(IEnumerable<Mortgagor> mortgagors)
        {
            if (mortgagors != null)
            {
                var sb = new StringBuilder();
                foreach (var mortgagor in mortgagors)
                {
                    if (sb.Length != 0) sb.Append("; ");
                    if (!string.IsNullOrWhiteSpace(mortgagor.CompanyName))
                    {
                        var emailcomponents = new List<string> { mortgagor.CompanyName, string.Concat(mortgagor.FirstName, " ", mortgagor.LastName).Trim() };
                        sb.Append(String.Join(", ", emailcomponents.FindAll(c => !string.IsNullOrWhiteSpace(c))));
                    }
                    else
                    {
                        var emailcomponents = new List<string> { mortgagor.LastName, mortgagor.FirstName };
                        sb.Append(String.Join(", ", emailcomponents.FindAll(c => !string.IsNullOrWhiteSpace(c))));
                    }
                }
                return sb.ToString();
            }
            return string.Empty;
        }

        internal static void ConcatFileInformation(FundingDeal dealInfo, string actingFor, string partyLastName, IDictionary<string, object> tokens)
        {
            string LawyerFileNumber;

            if (dealInfo.ActingFor == actingFor ||
                dealInfo.ActingFor == LawyerActingFor.Both ||
                dealInfo.ActingFor == LawyerActingFor.Mortgagor)
                LawyerFileNumber = dealInfo.LawyerFileNumber;
            else
                LawyerFileNumber = dealInfo.OtherLawyerFileNumber;

            string fileInformation;
            if (!string.IsNullOrWhiteSpace(LawyerFileNumber) && !string.IsNullOrWhiteSpace(partyLastName))
            {
                fileInformation = string.Format("{0} - {1}", partyLastName,
                    LawyerFileNumber);
            }
            else if (!string.IsNullOrWhiteSpace(partyLastName))
            {
                fileInformation = partyLastName;
            }
            else
            {
                fileInformation = LawyerFileNumber;
            }

            tokens.Add(EmailTemplateTokenList.FileInformation, fileInformation);

            
        }

        public void SendStandardNotification(FundingDeal fundingDeal, LawyerProfile lawyerProfile,
          string recipientActingFor, StandardNotificationKey notificationKey)
        {
            var connection = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings[DBContextName].ConnectionString
            };

            var tokens = CreateTokens(fundingDeal, recipientActingFor);

            EasyFundDispatcher.SendDealNotificationEmailToSpecificEmailId(connection,
                lawyerProfile.UserLanguage, notificationKey, tokens,
                lawyerProfile.Email);
        }

        internal static void SendStandardNotificationwithTokens(FundingDeal fundingDeal, LawyerProfile lawyerProfile,
          string actingFor, StandardNotificationKey notificationKey, IDictionary<string, object> tokens)
        {
            var connection = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings[DBContextName].ConnectionString
            };

            EasyFundDispatcher.SendDealNotificationEmailToSpecificEmailId(connection,
                lawyerProfile.UserLanguage, notificationKey, tokens,
                lawyerProfile.Email);
        }

        public IDictionary<string, object> CreateTokens(FundingDeal fundingDeal, string actingFor)
        {
            IDictionary<string, object> tokens = CreateEmailTokens(actingFor, FCTURNHelper.FormatShortFCTURN(fundingDeal.FCTURN),
                fundingDeal);
            int otherDealId;
            otherDealId = _fundingDealRepository.GetOtherDealInScope(fundingDeal.DealID.GetValueOrDefault());
            if (otherDealId == 0 && actingFor != LawyerActingFor.Both && actingFor != LawyerActingFor.Mortgagor)
            {
                FundingDeal cancelDeal = _fundingDealRepository.GetFundingDeal(fundingDeal.DealID.GetValueOrDefault());
                if (cancelDeal.DealStatus == DealStatus.Cancelled)
                {
                    otherDealId = _fundingDealRepository.GetOtherCancelledDealInScope(fundingDeal.DealID.GetValueOrDefault());
                }
            }
             
            switch (actingFor)
            {
                case LawyerActingFor.Purchaser:
                    var mortgagors = _mortgagorRepository.GetMortgagors(otherDealId);
                    var firstMortgagor = mortgagors.FirstOrDefault();
                    if (firstMortgagor != null)
                    {
                        if (firstMortgagor.MortgagorType == "Person")
                            ConcatFileInformation(fundingDeal, LawyerActingFor.Purchaser, firstMortgagor.LastName, tokens);
                        else
                            ConcatFileInformation(fundingDeal, LawyerActingFor.Purchaser, firstMortgagor.CompanyName, tokens);
                    }
                    else
                    {
                        ConcatFileInformation(fundingDeal, LawyerActingFor.Purchaser, "", tokens);
                    }
                    break;
                case LawyerActingFor.Both:
                    var mortgagors1 = _mortgagorRepository.GetMortgagors(fundingDeal.DealID.GetValueOrDefault());
                    var firstMortgagor1 = mortgagors1.FirstOrDefault();
                    if (firstMortgagor1 != null)
                    {
                        if (firstMortgagor1.MortgagorType == "Person")
                            ConcatFileInformation(fundingDeal, LawyerActingFor.Both, firstMortgagor1.LastName, tokens);
                        else
                            ConcatFileInformation(fundingDeal, LawyerActingFor.Both, firstMortgagor1.CompanyName, tokens);
                    }
                    else
                    {
                        ConcatFileInformation(fundingDeal, LawyerActingFor.Both, "", tokens);
                    }
                    break;
                case LawyerActingFor.Mortgagor:
                    var mortgagors2 = _mortgagorRepository.GetMortgagors(fundingDeal.DealID.GetValueOrDefault());
                    var firstMortgagor2 = mortgagors2.FirstOrDefault();
                    if (firstMortgagor2 != null)
                    {
                        if (firstMortgagor2.MortgagorType == "Person")
                            ConcatFileInformation(fundingDeal, LawyerActingFor.Mortgagor, firstMortgagor2.LastName, tokens);
                        else
                            ConcatFileInformation(fundingDeal, LawyerActingFor.Mortgagor, firstMortgagor2.CompanyName, tokens);
                    }
                    else
                    {
                        ConcatFileInformation(fundingDeal, LawyerActingFor.Mortgagor, "", tokens);
                    }
                    break;
                case LawyerActingFor.Vendor:
                    var vendors = _vendorRepository.GetVendorsByDeal(otherDealId);
                    var firstVendor = vendors.FirstOrDefault();
                    if (firstVendor != null)
                    {
                        if (firstVendor.VendorType == "Person")
                            ConcatFileInformation(fundingDeal, LawyerActingFor.Vendor, firstVendor.LastName, tokens);
                        else
                            ConcatFileInformation(fundingDeal, LawyerActingFor.Vendor, firstVendor.CompanyName, tokens);
                    }
                    else
                    {
                        ConcatFileInformation(fundingDeal, LawyerActingFor.Vendor, "", tokens);
                    }
                    break;
            }

            if (fundingDeal.ActingFor == actingFor ||
                fundingDeal.ActingFor == LawyerActingFor.Both ||
                fundingDeal.ActingFor == LawyerActingFor.Mortgagor)
            {
                tokens.Add(EmailTemplateTokenList.LawyerFileNumber, fundingDeal.LawyerFileNumber);
                tokens.Add(EmailTemplateTokenList.RecipientLawyerName,
String.Format("{0} {1}", fundingDeal.Lawyer.FirstName, fundingDeal.Lawyer.LastName));
            }

            else
            {
                tokens.Add(EmailTemplateTokenList.LawyerFileNumber, fundingDeal.OtherLawyerFileNumber);
                tokens.Add(EmailTemplateTokenList.RecipientLawyerName,
                String.Format("{0} {1}", fundingDeal.OtherLawyer.FirstName, fundingDeal.OtherLawyer.LastName));
            }
            return tokens;
        }

        internal static string GetEmailList(LawyerProfile lawyerProfile, DelegateInfoCollection delegates)
        {
            string emails;
            if (delegates.Any())
            {
                var emailList = new string[delegates.Count + 1];
                emailList[0] = lawyerProfile.Email;
                for (int i = 0; i < delegates.Count; i++)
                {
                    emailList[i + 1] = delegates[i].Email;
                }
                emailList = emailList.Distinct().ToArray();
                emails = String.Join(";", emailList);
            }
            else
            {
                emails = lawyerProfile.Email;
            }
            return emails;
        }

        public string GetEmailRecipientList(LawyerProfile user, string businessModel)
        {
            IEnumerable<string> products = businessModel.Split('/');


            var delegatesRequest = new GetAuthorizedDelegatesRequest() { UserName = user.UserName, Products = products.ToList() };
            var delegateResponse = _limService.GetAuthorizedDelegates(delegatesRequest);

            if (delegateResponse != null)
            {
                string emails = GetEmailList(user, delegateResponse.Delegates);
                return emails;
            }
            return string.Empty;
        }


        public KeyValuePair<string, string>? GetEmailRecipientList(FundingDeal requestDeal, int otherDealId = 0)
        {
            var lawyerDelegateIds =
                _dealContactRepository.GetDealContactIDs(otherDealId > 0
                    ? otherDealId
                    : requestDeal.DealID.GetValueOrDefault());

            DelegateInfoCollection delegateInfoCollection = null;

            if (otherDealId > 0)
            {
                var otherDeal = _dealRepository.GetDealDetails(otherDealId, new UserContext(), true);
                delegateInfoCollection = GetDelegates(requestDeal.OtherLawyer.LawyerID, otherDeal.BusinessModel);

            }
            else
            {
                delegateInfoCollection = GetDelegates(requestDeal.Lawyer.LawyerID, requestDeal.BusinessModel);
            }


            var mailerIds = new List<int>
            {
                otherDealId > 0 ? requestDeal.OtherLawyer.LawyerID : requestDeal.Lawyer.LawyerID
            };

            //Check if each delegate is active or disabled(disabled through myprofile page but still existing in DealContacts table)
            if (lawyerDelegateIds != null && delegateInfoCollection != null)
            {
                mailerIds.AddRange(from iIndex in lawyerDelegateIds
                    let delegateInfo = delegateInfoCollection.FirstOrDefault(p => p.UserID == iIndex)
                    where delegateInfo != null && delegateInfo.Enabled
                    select iIndex);
            }

            var userprofiles = _lawyerRepository.GetNotificationDetails(mailerIds);

            var lawyerProfile =
                otherDealId > 0
                    ? userprofiles.SingleOrDefault(u => u.LawyerID == requestDeal.OtherLawyer.LawyerID)
                    : userprofiles.SingleOrDefault(u => u.LawyerID == requestDeal.Lawyer.LawyerID);

            if (lawyerProfile != null)
            {
                string lawyerLanguage = lawyerProfile.UserLanguage;
                var emails = userprofiles.Select(u => u.Email);
                string mailingList = string.Join(";", emails.Distinct());
                var kvp = new KeyValuePair<string, string>(lawyerLanguage, mailingList);
                return kvp;
            }
            return null;
        }

        private DelegateInfoCollection GetDelegates(int UserId, string DealBusinessModel)
        {
            DelegateInfoCollection _delegateCollection = new DelegateInfoCollection();
                       
            string userName = GetUserName(UserId);
            if (!string.IsNullOrEmpty(userName))
            {
                IEnumerable<string> products = DealBusinessModel.Split('/');

                GetAuthorizedDelegatesRequest delegatesRequest = new GetAuthorizedDelegatesRequest() { UserName = userName, Products = products.ToList() };
                GetAuthorizedDelegatesResponse delegateResponse = _limService.GetAuthorizedDelegates(delegatesRequest);

                if (delegateResponse != null && delegateResponse.Delegates != null)
                {
                    _delegateCollection = delegateResponse.Delegates;
                }
            }
            return _delegateCollection;
        }

        private string GetUserName(int UserId)
        {
            string UserName = string.Empty;
            GetUserProfileByUserIDRequest request = new GetUserProfileByUserIDRequest() { UserID = UserId };

            GetUserProfileByUserIDResponse respone = _limService.GetUserProfileByUserID(request);

            if (respone != null && respone.UserProfile != null)
            {
                UserProfile _userProfile = respone.UserProfile;

                UserName = _userProfile.UserName;
            }

            return UserName;
        }

        private IEnumerable<string> GetDealProducts()//;string dealBusinessModel)
        {
            var products = new List<string>();
            products.Add("LLC");

            products.Add("EASYFUND");

            return products;
        }
    }
}
