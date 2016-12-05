using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class DealHistoryHelper
    {
        private readonly IGlobalizationRepository _globalizationRepository;
        public DealHistoryHelper(IGlobalizationRepository globalizationRepository)
        {
            _globalizationRepository = globalizationRepository;
        }


        internal DealHistoryEntry GetDealHistoryEntry(string activity, string lawyerName = null)
        {
            var entry = _globalizationRepository.GetEntry(activity);
            if (entry != null)
            {
                if (!string.IsNullOrEmpty(lawyerName))
                {
                    switch (activity)
                    {
                        case DealActivity.EFInviteReceived:
                        case DealActivity.EFCreatedInviteSent:
                        case DealActivity.EFDealSigned:
                        case DealActivity.EFDealSignatureRemoved:
                        case DealActivity.EFDealDeclined:
                        case DealActivity.EFDealCancelled:
                            entry.EnglishVersion = string.Format("{0} {1}", entry.EnglishVersion, lawyerName);
                            entry.FrenchVersion = string.Format("{0} {1}", entry.FrenchVersion, lawyerName);
                            break;
                    }
                }

                return entry;
            }
            return null;
        }

        internal DealHistoryEntry GetDealHistoryEntryDirectly(string resourceSet, string activity)
        {
            DealHistoryEntry entry = _globalizationRepository.GetEntry(resourceSet, activity);
            return entry;
        }

        internal IEnumerable<DealHistoryEntry> GetDealHistoryEntries(IEnumerable<UserHistory> userHistories)
        {
            var histories = new List<DealHistoryEntry>();
            var activities = userHistories.Select(u => u.Activity).AsEnumerable();
            var entries=_globalizationRepository.GetEntries(activities);

            foreach (var userHistory in userHistories)
            {
                DealHistoryEntry entry;
                if (entries.TryGetValue(userHistory.Activity,out entry))
                {
                    var dealHistory = new DealHistoryEntry()
                    {
                        DealId = userHistory.DealId,
                        EnglishVersion = entry.EnglishVersion,
                        FrenchVersion = entry.FrenchVersion
                    };
                    switch (userHistory.Activity)
                    {
                        case DealActivity.EFInviteReceived:
                        case DealActivity.EFCreatedInviteSent:
                        case DealActivity.EFDealSigned:
                        case DealActivity.EFDealSignatureRemoved:
                        case DealActivity.EFDealDeclined:
                        case DealActivity.EFDealCancelled:
                            dealHistory.EnglishVersion = string.Format("{0} {1}", dealHistory.EnglishVersion, userHistory.LawyerName);
                            dealHistory.FrenchVersion = string.Format("{0} {1}", dealHistory.FrenchVersion, userHistory.LawyerName);
                            histories.Add(dealHistory);
                            break;
                        default:
                            histories.Add(dealHistory);
                            break;
                    }
                }
            }
            return histories;
        }

        internal DealHistoryEntry GetDealHistoryEntry(string activity, decimal amount = 0)
        {
            var entry = _globalizationRepository.GetEntry(activity);
            if (entry != null)
            {
                if (amount > 0 && (activity == DealActivity.FundsAllocated))
                {
                    entry.EnglishVersion = string.Format("{0:0.00} {1}", amount, entry.EnglishVersion);
                    entry.FrenchVersion = string.Format("{0:0.00} {1}", amount, entry.FrenchVersion);
                }

                if (amount > 0 && entry.EnglishVersion.Contains("{0}"))
                {
                    entry.EnglishVersion = string.Format(entry.EnglishVersion, amount.ToString("c"));
                    entry.FrenchVersion = string.Format(entry.FrenchVersion, amount.ToString("c"));
                }

                return entry;
            }
            return null;
        }

        internal DealHistoryEntry GetDealHistoryEntry(string activity, DisbursementHistory disbursementHistory)
        {
             var entry = _globalizationRepository.GetEntry(activity);
            if (entry != null)
            {
                entry.EnglishVersion = ProcessEntry(disbursementHistory, entry.EnglishVersion);
                entry.FrenchVersion = ProcessEntry(disbursementHistory, entry.FrenchVersion);
                return entry;
            }
            return null;
        }

        internal IEnumerable<DealHistoryEntry> GetDealHistoryEntries(string resourceSet,
            IEnumerable<IDictionary<string, Variance>> varianceCollection)
        {
            var entries = _globalizationRepository.GetEntries(resourceSet).Values;

            var tobeInsertedEntries = new List<DealHistoryEntry>();
            foreach (var dealHistoryEntry in entries)
            {
                foreach (var varianceItem in varianceCollection)
                {
                    Variance variance;
                    if (varianceItem.TryGetValue(dealHistoryEntry.ResourceKey, out variance))
                    {
                        dealHistoryEntry.EnglishVersion = dealHistoryEntry.EnglishVersion.Replace(PlaceHolder.OldValue,
                            variance.SourceValue);
                        dealHistoryEntry.FrenchVersion = dealHistoryEntry.FrenchVersion.Replace(PlaceHolder.OldValue,
                            variance.SourceValue);
                        dealHistoryEntry.EnglishVersion = dealHistoryEntry.EnglishVersion.Replace(PlaceHolder.NewValue,
                            variance.SinkValue);
                        dealHistoryEntry.FrenchVersion = dealHistoryEntry.FrenchVersion.Replace(PlaceHolder.NewValue,
                            variance.SinkValue);
                        dealHistoryEntry.EnglishVersion = dealHistoryEntry.EnglishVersion.Replace(PlaceHolder.PayeeName, 
                            variance.Identifier);
                        dealHistoryEntry.FrenchVersion = dealHistoryEntry.FrenchVersion.Replace(PlaceHolder.PayeeName,
                            variance.Identifier);

                        tobeInsertedEntries.Add(dealHistoryEntry);
                    }
                }
            }
            return tobeInsertedEntries;
        }

        private static string ProcessEntry(DisbursementHistory disbursementHistory, string historyVersion)
        {
            historyVersion = historyVersion.Replace(PlaceHolder.PayeeName, disbursementHistory.Payee);
            historyVersion = historyVersion.Replace(PlaceHolder.OldValue, disbursementHistory.OldValue);
            historyVersion = historyVersion.Replace(PlaceHolder.NewValue, disbursementHistory.NewValue);
            historyVersion = historyVersion.Replace(PlaceHolder.TrustAccount, disbursementHistory.TrustAccount);
            // added by saman
            historyVersion = historyVersion.Replace(PlaceHolder.DisbursementStatus, disbursementHistory.OldDisbursementStatus);
            historyVersion = historyVersion.Replace(PlaceHolder.Amount, disbursementHistory.Amount.ToString("c"));
            if (!string.IsNullOrWhiteSpace(disbursementHistory.OldPayee))
            {
                historyVersion = historyVersion.Replace(PlaceHolder.OldPayeeName, disbursementHistory.OldPayee);
            }
            if (disbursementHistory.OldAmount > 0)
            {
                historyVersion = historyVersion.Replace(PlaceHolder.OldAmount, disbursementHistory.OldAmount.ToString("c"));
            }            
            return historyVersion;
        }



        internal DealHistoryEntry GetDealHistoryMessage(string historyMessage)
        {
            var entry = _globalizationRepository.GetEntry(ResourceSet.LawyerPortalMessage, historyMessage.ToString());
            return entry;
        }

        internal DealHistoryEntry GetDealHistoryMessageDealCancelledAdmin(string historyMessage)
        {
            var entry = _globalizationRepository.GetEntry(ResourceSet.FCTLLCBusinessService, "dealCancelled");
            return entry;
        }

        internal DealHistoryEntry GetDealHistoryMessage(DealHistoryEntry entry, string oldValueEnglish, string newValueEnglish, string oldValueFrench, string newValueFrench)
        {
            if (entry != null)
            {
                if (entry.EnglishVersion.Contains("{0}"))
                {
                    entry.EnglishVersion = string.Format(entry.EnglishVersion, oldValueEnglish, newValueEnglish);
                    entry.FrenchVersion = string.Format(entry.FrenchVersion, oldValueFrench, newValueFrench);
                }
                else
                {
                    //some lawyer portal history message does not contain whole sentence
                    entry.EnglishVersion = string.Format("{0} from {1} to {2}.", entry.EnglishVersion, oldValueEnglish, newValueEnglish);
                    entry.FrenchVersion = string.Format("{0} mis à jour de {1} à {2}.", entry.FrenchVersion, oldValueFrench, newValueFrench);
                }
            }
            return entry;
        }


        internal IEnumerable<DealHistoryEntry> GetDealHistoryMessages(IEnumerable<UserHistory> userhistories)
        {
            var entries = _globalizationRepository.GetEntries(ResourceSet.LawyerPortalMessage, "DealChanges");
            var histories = new List<DealHistoryEntry>();
            foreach (var userHistory in userhistories)
            {
                DealHistoryEntry entry;
                if (entries.TryGetValue(userHistory.Activity, out entry))
                {
                        var dealHistory = new DealHistoryEntry()
                        {
                            DealId = userHistory.DealId,
                            EnglishVersion = entry.EnglishVersion,
                            FrenchVersion = entry.FrenchVersion
                        };

                    if (entry.ResourceKey == HistoryMessage.ClosingDateChange)
                    {
                        dealHistory = FormatDateHistoryEntry(userHistory, dealHistory);
                        histories.Add(dealHistory); 
                    }
                    else if (string.IsNullOrEmpty(userHistory.OldValue) && string.IsNullOrEmpty(userHistory.NewValue))
                    {
                        histories.Add(dealHistory);  
                    }

                    else
                    {
                        if (dealHistory.EnglishVersion.Contains("{0}") && dealHistory.EnglishVersion.Contains("{1}"))
                        {
                            dealHistory.EnglishVersion = string.Format(dealHistory.EnglishVersion, userHistory.OldValue, userHistory.NewValue);
                            dealHistory.FrenchVersion = string.Format(dealHistory.FrenchVersion, userHistory.OldValue, userHistory.NewValue);
                        }
                        else if (dealHistory.EnglishVersion.Contains("{0}") && !dealHistory.EnglishVersion.Contains("{1}"))
                        {
                            dealHistory.EnglishVersion = string.Format(dealHistory.EnglishVersion, userHistory.OldValue);
                            dealHistory.FrenchVersion = string.Format(dealHistory.FrenchVersion, userHistory.OldValue);
                        }
                        else
                        {
                            //some lawyer portal history message does not contain whole sentence
                            dealHistory.EnglishVersion = string.Format("{0} from {1} to {2}.", dealHistory.EnglishVersion, userHistory.OldValue, userHistory.NewValue);
                            dealHistory.FrenchVersion = string.Format("{0} mis à jour de {1} à {2}.", dealHistory.FrenchVersion, userHistory.OldValue, userHistory.NewValue);
                        }
                        histories.Add(dealHistory); 
                    }
                                                           
                }               
                
            }
            return histories;
        }

        private DealHistoryEntry FormatDateHistoryEntry(UserHistory userHistory, DealHistoryEntry entry)
        {
            const string SHORT_DATE_FORMAT_EN = "MMM dd/yyyy";
            CultureInfo cultureEn = new CultureInfo("en-ca");
            cultureEn.DateTimeFormat.ShortDatePattern = SHORT_DATE_FORMAT_EN;

            const string SHORT_DATE_FORMAT_FR = "d MMM yyyy";
            CultureInfo cultureFr = new CultureInfo("fr-ca");
            cultureFr.DateTimeFormat.ShortDatePattern = SHORT_DATE_FORMAT_FR;

            string oldValueEnglish = string.IsNullOrWhiteSpace(userHistory.OldValue) ? string.Empty : Convert.ToDateTime(userHistory.OldValue).ToString(SHORT_DATE_FORMAT_EN, cultureEn);
            string newValueEnglish = string.IsNullOrWhiteSpace(userHistory.NewValue) ? string.Empty : Convert.ToDateTime(userHistory.NewValue).ToString(SHORT_DATE_FORMAT_EN, cultureEn);
            string oldValueFrench = string.IsNullOrWhiteSpace(userHistory.OldValue) ? string.Empty : Convert.ToDateTime(userHistory.OldValue).ToString(SHORT_DATE_FORMAT_FR, cultureFr);
            string newValueFrench = string.IsNullOrWhiteSpace(userHistory.NewValue) ? string.Empty : Convert.ToDateTime(userHistory.NewValue).ToString(SHORT_DATE_FORMAT_FR, cultureFr);
            entry = GetDealHistoryMessage(entry, oldValueEnglish, newValueEnglish, oldValueFrench,
                newValueFrench);
            return entry;
        }

        internal DealHistoryEntry GetDealHistoryEntry(string historyMessage, string addedOrRemovedValue, string resourceSet, int statusReasonId=0)
        {
            DealHistoryEntry entry = _globalizationRepository.GetEntry(resourceSet, historyMessage);
            if (entry != null && entry.EnglishVersion.Contains("{0}"))
            {
                if (statusReasonId > 0)
                {
                    var messageEntry = _globalizationRepository.GetEntry(statusReasonId, ResourceSet.LawyerPortalMessage);
                    if (messageEntry.EnglishVersion.Equals("Other", StringComparison.OrdinalIgnoreCase))
                    {
                        entry.EnglishVersion = string.Format(entry.EnglishVersion, addedOrRemovedValue);
                        entry.FrenchVersion = string.Format(entry.FrenchVersion, addedOrRemovedValue); 
                    }
                    else
                    {
                        entry.EnglishVersion = string.Format(entry.EnglishVersion, messageEntry.EnglishVersion);
                        entry.FrenchVersion = string.Format(entry.FrenchVersion, messageEntry.FrenchVersion);
                    }
                }
                else
                {
                    entry.EnglishVersion = string.Format(entry.EnglishVersion, addedOrRemovedValue);
                    entry.FrenchVersion = string.Format(entry.FrenchVersion, addedOrRemovedValue);
                }

            }
            return entry;
        }


    }


}
