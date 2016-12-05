using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class DealAmendmentsChecker:IDealAmendmentsChecker
    {
        private readonly IDealHistoryRepository _dealHistoryRepository;
        private readonly IFundingDealRepository _dealFundingRepository;
        private readonly IDealScopeRepository _dealScopeRepository;

        public DealAmendmentsChecker(IDealScopeRepository dealScopeRepository, IFundingDealRepository fundingDealRepository,
            IDealHistoryRepository dealHistoryRepository)
        {
            _dealFundingRepository = fundingDealRepository;
            _dealScopeRepository = dealScopeRepository;
            _dealHistoryRepository = dealHistoryRepository;
        }

        public bool CheckAmendments(FundingDeal oldFundingDeal, FundingDeal newFundingDeal, UserContext user)
        {

            bool hasAmendments;

            if (oldFundingDeal == null || newFundingDeal == null) return false;

            //only check amendments when deal is active prior update
            if (oldFundingDeal.DealStatus != DealStatus.Active) return false;

            var histories = GetDealHistoriesForAmendments(oldFundingDeal, newFundingDeal, out hasAmendments);

            _dealHistoryRepository.CreateDealChangeHistories(histories, user);
            return hasAmendments;
        }

        public List<UserHistory> GetDealHistoriesForAmendments(FundingDeal oldFundingDeal, FundingDeal newFundingDeal, out bool hasAmendments)
        {
            hasAmendments = false;
            int dealId = oldFundingDeal.DealID.GetValueOrDefault();
            int dealScopeId = _dealScopeRepository.GetDealScope(newFundingDeal.FCTURN);
            bool hasOtherDeal = (newFundingDeal.ActingFor != LawyerActingFor.Both &&
                                 newFundingDeal.ActingFor != LawyerActingFor.Mortgagor);
            int otherDealId = 0;
            if (hasOtherDeal) otherDealId = _dealFundingRepository.GetOtherDealInScope(dealId, dealScopeId);

            var histories = new List<UserHistory>();
            if (newFundingDeal.ActingFor == LawyerActingFor.Vendor || newFundingDeal.ActingFor == LawyerActingFor.Both)
            {
                //find added Vendor list
                hasAmendments = HasVendorAmendments(oldFundingDeal, newFundingDeal, histories, dealId, hasOtherDeal, otherDealId);
            }


            if (newFundingDeal.ActingFor == LawyerActingFor.Purchaser ||
                newFundingDeal.ActingFor == LawyerActingFor.Both ||
                newFundingDeal.ActingFor == LawyerActingFor.Mortgagor)
            {
                hasAmendments = CheckClosingDateAmendment(oldFundingDeal, newFundingDeal, hasAmendments, histories, dealId,
                    hasOtherDeal, otherDealId);

                hasAmendments = CheckIfPurchaserAdded(oldFundingDeal, newFundingDeal, hasAmendments, histories, dealId,
                    hasOtherDeal, otherDealId);

                hasAmendments = CheckIfPurchaserDeleted(oldFundingDeal, newFundingDeal, hasAmendments, histories, dealId,
                    hasOtherDeal, otherDealId);

                CheckIfPurchaserUpdated(oldFundingDeal, newFundingDeal, hasAmendments, histories, dealId, hasOtherDeal,
                    otherDealId);

                hasAmendments = CheckPropertyAmendments(oldFundingDeal, newFundingDeal, hasAmendments, histories, dealId,
                    hasOtherDeal, otherDealId);

                hasAmendments = CheckPINAmendments(oldFundingDeal, newFundingDeal, hasAmendments, histories, dealId,
                    hasOtherDeal, otherDealId);
            }
            return histories;
        }

        private static void CheckIfPurchaserUpdated(FundingDeal oldFundingDeal, FundingDeal newFundingDeal, bool hasAmendments,
            List<UserHistory> histories, int dealId, bool hasOtherDeal, int otherDealId)
        {
            //Get updated Mortgagor list
            List<Mortgagor> newMortgagors =
                newFundingDeal.Mortgagors.Where(
                    m => oldFundingDeal.Mortgagors.Any(m1 => m1.MortgagorID == m.MortgagorID)).ToList();

            foreach (Mortgagor newMortgagor in newMortgagors)
            {
                Mortgagor oldMortgagor =
                    oldFundingDeal.Mortgagors.SingleOrDefault(m => m.MortgagorID == newMortgagor.MortgagorID);
                if (oldMortgagor.MortgagorType != newMortgagor.MortgagorType)
                {
                    hasAmendments = MortgagorTypeAmended(newFundingDeal, hasAmendments, histories, dealId, oldMortgagor,
                        newMortgagor, hasOtherDeal, otherDealId);
                }
                else
                {
                    if (oldMortgagor.MortgagorType.ToUpper() == PartyType.Business)
                    {
                        hasAmendments = CheckIfCompanyNameAmended(newFundingDeal, oldMortgagor, newMortgagor, hasAmendments,
                            histories, dealId, hasOtherDeal, otherDealId);
                        hasAmendments = CheckIfContactFirstNameAmended(newFundingDeal, oldMortgagor, newMortgagor, hasAmendments,
                            histories, dealId, hasOtherDeal, otherDealId);
                        hasAmendments = CheckIfContactLastNameAmended(newFundingDeal, oldMortgagor, newMortgagor, hasAmendments,
                            histories, dealId, hasOtherDeal, otherDealId);
                    }
                    else
                    {
                        hasAmendments = CheckIfFirstNameAmended(newFundingDeal, oldMortgagor, newMortgagor, hasAmendments,
                            histories, dealId, hasOtherDeal, otherDealId);
                        hasAmendments = CheckIfMiddleNameAmended(newFundingDeal, oldMortgagor, newMortgagor, hasAmendments,
                            histories, dealId, hasOtherDeal, otherDealId);
                        hasAmendments = CheckIfLastNameAmended(newFundingDeal, oldMortgagor, newMortgagor, hasAmendments,
                            histories, dealId, hasOtherDeal, otherDealId);
                    }
                }
            }
        }

        private static bool CheckIfLastNameAmended(FundingDeal newFundingDeal, Mortgagor oldMortgagor, Mortgagor newMortgagor,
            bool hasAmendments, List<UserHistory> histories, int dealId, bool hasOtherDeal, int otherDealId)
        {
            if (oldMortgagor.LastName != newMortgagor.LastName)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = newFundingDeal.ActingFor == LawyerActingFor.Mortgagor
                        ? HistoryMessage.MortgagorLastName
                        : HistoryMessage.PurchaserLastName,
                    DealId = dealId,
                    OldValue = oldMortgagor.LastName,
                    NewValue = newMortgagor.LastName
                });
                if (hasOtherDeal)
                    histories.Add(new UserHistory()
                    {
                        Activity = newFundingDeal.ActingFor == LawyerActingFor.Mortgagor
                            ? HistoryMessage.MortgagorLastName
                            : HistoryMessage.PurchaserLastName,
                        DealId = otherDealId,
                        OldValue = oldMortgagor.LastName,
                        NewValue = newMortgagor.LastName
                    });
            }
            return hasAmendments;
        }

        private static bool CheckIfMiddleNameAmended(FundingDeal newFundingDeal, Mortgagor oldMortgagor, Mortgagor newMortgagor,
            bool hasAmendments, List<UserHistory> histories, int dealId, bool hasOtherDeal, int otherDealId)
        {
            if (oldMortgagor.MiddleName != newMortgagor.MiddleName)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = newFundingDeal.ActingFor == LawyerActingFor.Mortgagor
                        ? HistoryMessage.MortgagorMiddleName
                        : HistoryMessage.PurchaserMiddleName,
                    DealId = dealId,
                    OldValue = oldMortgagor.MiddleName,
                    NewValue = newMortgagor.MiddleName
                });
                if (hasOtherDeal)
                    histories.Add(new UserHistory()
                    {
                        Activity = newFundingDeal.ActingFor == LawyerActingFor.Mortgagor
                            ? HistoryMessage.MortgagorMiddleName
                            : HistoryMessage.PurchaserMiddleName,
                        DealId = otherDealId,
                        OldValue = oldMortgagor.MiddleName,
                        NewValue = newMortgagor.MiddleName
                    });
            }
            return hasAmendments;
        }

        private static bool CheckIfFirstNameAmended(FundingDeal newFundingDeal, Mortgagor oldMortgagor, Mortgagor newMortgagor,
            bool hasAmendments, List<UserHistory> histories, int dealId, bool hasOtherDeal, int otherDealId)
        {
//Mortgagor Information – Person: First Name, Middle Name, Last Name
            if (oldMortgagor.FirstName != newMortgagor.FirstName)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = newFundingDeal.ActingFor == LawyerActingFor.Mortgagor
                        ? HistoryMessage.MortgagorFirstName
                        : HistoryMessage.PurchaserFirstName,
                    DealId = dealId,
                    OldValue = oldMortgagor.FirstName,
                    NewValue = newMortgagor.FirstName
                });
                if (hasOtherDeal)
                    histories.Add(new UserHistory()
                    {
                        Activity = newFundingDeal.ActingFor == LawyerActingFor.Mortgagor
                            ? HistoryMessage.MortgagorFirstName
                            : HistoryMessage.PurchaserFirstName,
                        DealId = otherDealId,
                        OldValue = oldMortgagor.FirstName,
                        NewValue = newMortgagor.FirstName
                    });
            }
            return hasAmendments;
        }

        private static bool CheckIfContactLastNameAmended(FundingDeal newFundingDeal, Mortgagor oldMortgagor,
            Mortgagor newMortgagor, bool hasAmendments, List<UserHistory> histories, int dealId, bool hasOtherDeal, int otherDealId)
        {
            if (oldMortgagor.LastName != newMortgagor.LastName)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = newFundingDeal.ActingFor == LawyerActingFor.Mortgagor
                        ? HistoryMessage.MortgagorContactLastName
                        : HistoryMessage.PurchaserContactLastName,
                    DealId = dealId,
                    OldValue = oldMortgagor.LastName,
                    NewValue = newMortgagor.LastName
                });
                if (hasOtherDeal)
                    histories.Add(new UserHistory()
                    {
                        Activity = newFundingDeal.ActingFor == LawyerActingFor.Mortgagor
                            ? HistoryMessage.MortgagorContactLastName
                            : HistoryMessage.PurchaserContactLastName,
                        DealId = otherDealId,
                        OldValue = oldMortgagor.LastName,
                        NewValue = newMortgagor.LastName
                    });
            }
            return hasAmendments;
        }

        private static bool CheckIfContactFirstNameAmended(FundingDeal newFundingDeal, Mortgagor oldMortgagor,
            Mortgagor newMortgagor, bool hasAmendments, List<UserHistory> histories, int dealId, bool hasOtherDeal, int otherDealId)
        {
            if (oldMortgagor.FirstName != newMortgagor.FirstName)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = newFundingDeal.ActingFor == LawyerActingFor.Mortgagor
                        ? HistoryMessage.MortgagorContactFirstName
                        : HistoryMessage.PurchaserContactFirstName,
                    DealId = dealId,
                    OldValue = oldMortgagor.FirstName,
                    NewValue = newMortgagor.FirstName
                });
                if (hasOtherDeal)
                    histories.Add(new UserHistory()
                    {
                        Activity = newFundingDeal.ActingFor == LawyerActingFor.Mortgagor
                            ? HistoryMessage.MortgagorContactFirstName
                            : HistoryMessage.PurchaserContactFirstName,
                        DealId = otherDealId,
                        OldValue = oldMortgagor.FirstName,
                        NewValue = newMortgagor.FirstName
                    });
            }
            return hasAmendments;
        }

        private static bool CheckIfCompanyNameAmended(FundingDeal newFundingDeal, Mortgagor oldMortgagor, Mortgagor newMortgagor,
            bool hasAmendments, List<UserHistory> histories, int dealId, bool hasOtherDeal, int otherDealId)
        {
//Mortgagor Information – Business: Company Name, Contact First Name, Contact Last Name
            if (oldMortgagor.CompanyName != newMortgagor.CompanyName)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = newFundingDeal.ActingFor == LawyerActingFor.Mortgagor
                        ? HistoryMessage.MortgagorCompanyName
                        : HistoryMessage.PurchaserCompanyName,
                    DealId = dealId,
                    OldValue = oldMortgagor.CompanyName,
                    NewValue = newMortgagor.CompanyName
                });
                if (hasOtherDeal)
                    histories.Add(new UserHistory()
                    {
                        Activity = newFundingDeal.ActingFor == LawyerActingFor.Mortgagor
                            ? HistoryMessage.MortgagorCompanyName
                            : HistoryMessage.PurchaserCompanyName,
                        DealId = otherDealId,
                        OldValue = oldMortgagor.CompanyName,
                        NewValue = newMortgagor.CompanyName
                    });
            }
            return hasAmendments;
        }

        private static bool MortgagorTypeAmended(FundingDeal newFundingDeal, bool hasAmendments, List<UserHistory> histories, int dealId,
            Mortgagor oldMortgagor, Mortgagor newMortgagor, bool hasOtherDeal, int otherDealId)
        {
            hasAmendments = true;
            histories.Add(new UserHistory()
            {
                Activity = newFundingDeal.ActingFor == LawyerActingFor.Mortgagor
                    ? HistoryMessage.MortgagorTypeChanged
                    : HistoryMessage.PurchaserTypeChanged,
                DealId = dealId,
                OldValue = oldMortgagor.MortgagorType,
                NewValue = newMortgagor.MortgagorType
            });
            if (hasOtherDeal)
                histories.Add(new UserHistory()
                {
                    Activity = newFundingDeal.ActingFor == LawyerActingFor.Mortgagor
                        ? HistoryMessage.MortgagorTypeChanged
                        : HistoryMessage.PurchaserTypeChanged,
                    DealId = otherDealId,
                    OldValue = oldMortgagor.MortgagorType,
                    NewValue = newMortgagor.MortgagorType
                });
            return hasAmendments;
        }

        private static bool CheckIfPurchaserDeleted(FundingDeal oldFundingDeal, FundingDeal newFundingDeal, bool hasAmendments,
            List<UserHistory> histories, int dealId, bool hasOtherDeal, int otherDealId)
        {
//find deleted Mortgagor list
            var deletedMortgagors = oldFundingDeal.Mortgagors.Where(
                m => newFundingDeal.Mortgagors.All(m1 => m1.MortgagorID != m.MortgagorID));
            if (deletedMortgagors.Any())
            {
                hasAmendments = true;
            }
            foreach (var deletedMortgagor in deletedMortgagors)
            {
                histories.Add(new UserHistory()
                {
                    Activity = newFundingDeal.ActingFor == LawyerActingFor.Mortgagor
                        ? HistoryMessage.MortgagorRemoved
                        : HistoryMessage.PurchaserRemoved,
                    DealId = dealId,
                });
                if (hasOtherDeal)
                    histories.Add(new UserHistory()
                    {
                        Activity = newFundingDeal.ActingFor == LawyerActingFor.Mortgagor
                            ? HistoryMessage.MortgagorRemoved
                            : HistoryMessage.PurchaserRemoved,
                        DealId = otherDealId,
                    });
            }
            return hasAmendments;
        }

        private static bool CheckIfPurchaserAdded(FundingDeal oldFundingDeal, FundingDeal newFundingDeal, bool hasAmendments,
            List<UserHistory> histories, int dealId, bool hasOtherDeal, int otherDealId)
        {
            var addedMortgagors = newFundingDeal.Mortgagors.Where(
                m => oldFundingDeal.Mortgagors.All(m1 => m1.MortgagorID != m.MortgagorID));

            if (addedMortgagors.Any())
            {
                hasAmendments = true;
            }

            foreach (var addedMortgagor in addedMortgagors)
            {
                histories.Add(new UserHistory()
                {
                    Activity =
                        newFundingDeal.ActingFor == LawyerActingFor.Mortgagor
                            ? HistoryMessage.MortgagorAdded
                            : HistoryMessage.PurchaserAdded,
                    DealId = dealId,
                });
                if (hasOtherDeal)
                    histories.Add(new UserHistory()
                    {
                        Activity = newFundingDeal.ActingFor == LawyerActingFor.Mortgagor
                            ? HistoryMessage.MortgagorAdded
                            : HistoryMessage.PurchaserAdded,
                        DealId = otherDealId,
                    });
            }
            return hasAmendments;
        }

        private static bool CheckPropertyAmendments(FundingDeal oldFundingDeal, FundingDeal newFundingDeal, bool hasAmendments,
            List<UserHistory> histories, int dealId, bool hasOtherDeal, int otherDealId)
        {
//Property: Unit Number, Street Number, Address 1, Address 2, City, Province, Postal Code
            if (oldFundingDeal.Property.UnitNumber != newFundingDeal.Property.UnitNumber)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.UnitNumberUpdated,
                    DealId = dealId,
                    OldValue = oldFundingDeal.Property.UnitNumber,
                    NewValue = newFundingDeal.Property.UnitNumber
                });
                if (hasOtherDeal)
                    histories.Add(new UserHistory()
                    {
                        Activity = HistoryMessage.UnitNumberUpdated,
                        DealId = otherDealId,
                        OldValue = oldFundingDeal.Property.UnitNumber,
                        NewValue = newFundingDeal.Property.UnitNumber
                    });
            }
            if (oldFundingDeal.Property.StreetNumber != newFundingDeal.Property.StreetNumber)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.StreetNumberUpdated,
                    DealId = dealId,
                    OldValue = oldFundingDeal.Property.StreetNumber,
                    NewValue = newFundingDeal.Property.StreetNumber
                });
                if (hasOtherDeal)
                    histories.Add(new UserHistory()
                    {
                        Activity = HistoryMessage.StreetNumberUpdated,
                        DealId = otherDealId,
                        OldValue = oldFundingDeal.Property.StreetNumber,
                        NewValue = newFundingDeal.Property.StreetNumber
                    });
            }
            if (oldFundingDeal.Property.Address != newFundingDeal.Property.Address)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.StreetNameUpdated,
                    DealId = dealId,
                    OldValue = oldFundingDeal.Property.Address,
                    NewValue = newFundingDeal.Property.Address
                });
                if (hasOtherDeal)
                    histories.Add(new UserHistory()
                    {
                        Activity = HistoryMessage.StreetNameUpdated,
                        DealId = otherDealId,
                        OldValue = oldFundingDeal.Property.Address,
                        NewValue = newFundingDeal.Property.Address
                    });
            }
            if (oldFundingDeal.Property.Address2 != newFundingDeal.Property.Address2)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.StreetNameUpdated,
                    DealId = dealId,
                    OldValue = oldFundingDeal.Property.Address2,
                    NewValue = newFundingDeal.Property.Address2
                });
                if (hasOtherDeal)
                    histories.Add(new UserHistory()
                    {
                        Activity = HistoryMessage.StreetNameUpdated,
                        DealId = otherDealId,
                        OldValue = oldFundingDeal.Property.Address2,
                        NewValue = newFundingDeal.Property.Address2
                    });
            }
            if (oldFundingDeal.Property.City != newFundingDeal.Property.City)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.CityUpdated,
                    DealId = dealId,
                    OldValue = oldFundingDeal.Property.City,
                    NewValue = newFundingDeal.Property.City
                });
                if (hasOtherDeal)
                    histories.Add(new UserHistory()
                    {
                        Activity = HistoryMessage.CityUpdated,
                        DealId = otherDealId,
                        OldValue = oldFundingDeal.Property.City,
                        NewValue = newFundingDeal.Property.City
                    });
            }
            if (oldFundingDeal.Property.Province != newFundingDeal.Property.Province)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.PropertyProvince,
                    DealId = dealId,
                    OldValue = oldFundingDeal.Property.Province,
                    NewValue = newFundingDeal.Property.Province
                });
                if (hasOtherDeal)
                    histories.Add(new UserHistory()
                    {
                        Activity = HistoryMessage.PropertyProvince,
                        DealId = otherDealId,
                        OldValue = oldFundingDeal.Property.Province,
                        NewValue = newFundingDeal.Property.Province
                    });
            }
            if (oldFundingDeal.Property.PostalCode != newFundingDeal.Property.PostalCode)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.PostalCodeUpdated,
                    DealId = dealId,
                    OldValue = oldFundingDeal.Property.PostalCode,
                    NewValue = newFundingDeal.Property.PostalCode
                });
                if (hasOtherDeal)
                    histories.Add(new UserHistory()
                    {
                        Activity = HistoryMessage.PostalCodeUpdated,
                        DealId = otherDealId,
                        OldValue = oldFundingDeal.Property.PostalCode,
                        NewValue = newFundingDeal.Property.PostalCode
                    });
            }
            return hasAmendments;
        }

        private static bool CheckPINAmendments(FundingDeal oldFundingDeal, FundingDeal newFundingDeal, bool hasAmendments,
            List<UserHistory> histories, int dealId, bool hasOtherDeal, int otherDealId)
        {
//PIN(s)
            //find added Property.PIN list
            List<Pin> addedPins =
                newFundingDeal.Property.Pins.Where(p => oldFundingDeal.Property.Pins.All(p1 => p1.PinID != p.PinID))
                    .ToList();
            if (addedPins.Any())
            {
                hasAmendments = true;
                foreach (Pin addedPin in addedPins)
                {
                    histories.Add(new UserHistory()
                    {
                        Activity = HistoryMessage.PinAdd,
                        DealId = dealId,
                        OldValue = addedPin.PINNumber
                    });
                    if (hasOtherDeal)
                        histories.Add(new UserHistory()
                        {
                            Activity = HistoryMessage.PinAdd,
                            DealId = otherDealId,
                            OldValue = addedPin.PINNumber
                        });
                }
            }
            //find deleted Property.PIN list
            List<Pin> removedPins =
                oldFundingDeal.Property.Pins.Where(p => newFundingDeal.Property.Pins.All(p1 => p1.PinID != p.PinID))
                    .ToList();
            if (removedPins.Any())
            {
                hasAmendments = true;
                foreach (Pin removedPin in removedPins)
                {
                    histories.Add(new UserHistory()
                    {
                        Activity = HistoryMessage.PinRemove,
                        DealId = dealId,
                        OldValue = removedPin.PINNumber
                    });
                    if (hasOtherDeal)
                        histories.Add(new UserHistory()
                        {
                            Activity = HistoryMessage.PinRemove,
                            DealId = otherDealId,
                            OldValue = removedPin.PINNumber
                        });
                }
            }
            //Get updated Property.PIN list
            List<Pin> newPins =
                newFundingDeal.Property.Pins.Where(p => oldFundingDeal.Property.Pins.Any(p1 => p1.PinID == p.PinID))
                    .ToList();
            foreach (Pin newPin in newPins)
            {
                Pin oldPin = oldFundingDeal.Property.Pins.SingleOrDefault(p => p.PinID == newPin.PinID);
                if (oldPin.PINNumber != newPin.PINNumber)
                {
                    hasAmendments = true;
                    histories.Add(new UserHistory()
                    {
                        Activity = HistoryMessage.PinUpdate,
                        DealId = dealId,
                        OldValue = oldPin.PINNumber,
                        NewValue = newPin.PINNumber
                    });
                    if (hasOtherDeal)
                        histories.Add(new UserHistory()
                        {
                            Activity = HistoryMessage.PinUpdate,
                            DealId = otherDealId,
                            OldValue = oldPin.PINNumber,
                            NewValue = newPin.PINNumber
                        });
                }
            }
            return hasAmendments;
        }

        private static bool CheckClosingDateAmendment(FundingDeal oldFundingDeal, FundingDeal newFundingDeal, bool hasAmendments,
            List<UserHistory> histories, int dealId, bool hasOtherDeal, int otherDealId)
        {
//Closing Date
            if (oldFundingDeal.ClosingDate != newFundingDeal.ClosingDate)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.ClosingDateChange,
                    DealId = dealId,
                    OldValue = oldFundingDeal.ClosingDate.Value.ToLongDateString(),
                    NewValue = newFundingDeal.ClosingDate.Value.ToLongDateString()
                });
                if (hasOtherDeal)
                    histories.Add(new UserHistory()
                    {
                        Activity = HistoryMessage.ClosingDateChange,
                        DealId = otherDealId,
                        OldValue = oldFundingDeal.ClosingDate.Value.ToLongDateString(),
                        NewValue = newFundingDeal.ClosingDate.Value.ToLongDateString()
                    });
            }
            return hasAmendments;
        }

        public  bool HasVendorAmendments(FundingDeal oldFundingDeal, FundingDeal newFundingDeal,
            List<UserHistory> histories, int dealId, bool hasOtherDeal, int otherDealId)
        {
            bool hasAmendments=false;
            var addedVendors =
                newFundingDeal.Vendors.Where(v => oldFundingDeal.Vendors.All(v1 => v1.VendorID != v.VendorID));
            foreach (var addedVendor in addedVendors)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.VendorAdded,
                    DealId = dealId,
                });
                if (hasOtherDeal)
                    histories.Add(new UserHistory()
                    {
                        Activity = HistoryMessage.VendorAdded,
                        DealId = otherDealId,
                    });
            }
            //find deleted Vendor list
            var deletedVendors =
                oldFundingDeal.Vendors.Where(v => newFundingDeal.Vendors.All(v1 => v1.VendorID != v.VendorID));
            foreach (var deletedVendor in deletedVendors)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.VendorRemoved,
                    DealId = dealId,
                });
                if (hasOtherDeal)
                    histories.Add(new UserHistory()
                    {
                        Activity = HistoryMessage.VendorRemoved,
                        DealId = otherDealId,
                    });
            }
            //Get updated Vendor list
            List<Vendor> newVendors =
                newFundingDeal.Vendors.Where(v => oldFundingDeal.Vendors.Any(v1 => v1.VendorID == v.VendorID))
                    .ToList();
            foreach (Vendor newVendor in newVendors)
            {
                Vendor oldVendor = oldFundingDeal.Vendors.SingleOrDefault(v => v.VendorID == newVendor.VendorID);
                if (oldVendor.VendorType != newVendor.VendorType)
                {
                    hasAmendments = true;
                    histories.Add(new UserHistory()
                    {
                        Activity = HistoryMessage.VendorTypeChanged,
                        DealId = dealId,
                        OldValue = oldVendor.VendorType,
                        NewValue = newVendor.VendorType
                    });
                    if (hasOtherDeal)
                        histories.Add(new UserHistory()
                        {
                            Activity = HistoryMessage.VendorTypeChanged,
                            DealId = otherDealId,
                            OldValue = oldVendor.VendorType,
                            NewValue = newVendor.VendorType
                        });
                }
                else
                {
                    if (oldVendor.VendorType.ToUpper() == PartyType.Business)
                    {
                        //Vendor Information – Business: Company Name, Contact First Name, Contact Last Name
                        if (oldVendor.CompanyName != newVendor.CompanyName)
                        {
                            hasAmendments = true;
                            histories.Add(new UserHistory()
                            {
                                Activity = HistoryMessage.VendorCompanyName,
                                DealId = dealId,
                                OldValue = oldVendor.CompanyName,
                                NewValue = newVendor.CompanyName
                            });
                            if (hasOtherDeal)
                                histories.Add(new UserHistory()
                                {
                                    Activity = HistoryMessage.VendorCompanyName,
                                    DealId = otherDealId,
                                    OldValue = oldVendor.CompanyName,
                                    NewValue = newVendor.CompanyName
                                });
                        }
                        if (oldVendor.FirstName != newVendor.FirstName)
                        {
                            hasAmendments = true;
                            histories.Add(new UserHistory()
                            {
                                Activity = HistoryMessage.VendorContactFirstName,
                                DealId = dealId,
                                OldValue = oldVendor.FirstName,
                                NewValue = newVendor.FirstName
                            });
                            if (hasOtherDeal)
                                histories.Add(new UserHistory()
                                {
                                    Activity = HistoryMessage.VendorContactFirstName,
                                    DealId = otherDealId,
                                    OldValue = oldVendor.FirstName,
                                    NewValue = newVendor.FirstName
                                });
                        }
                        if (oldVendor.LastName != newVendor.LastName)
                        {
                            hasAmendments = true;
                            histories.Add(new UserHistory()
                            {
                                Activity = HistoryMessage.VendorContactLastName,
                                DealId = dealId,
                                OldValue = oldVendor.LastName,
                                NewValue = newVendor.LastName
                            });
                            if (hasOtherDeal)
                                histories.Add(new UserHistory()
                                {
                                    Activity = HistoryMessage.VendorContactLastName,
                                    DealId = otherDealId,
                                    OldValue = oldVendor.LastName,
                                    NewValue = newVendor.LastName
                                });
                        }
                    }
                    else
                    {
                        //Vendor Information – Person: First Name, Middle Name, Last Name
                        if (oldVendor.FirstName != newVendor.FirstName)
                        {
                            hasAmendments = true;
                            histories.Add(new UserHistory()
                            {
                                Activity = HistoryMessage.VendorFirstName,
                                DealId = dealId,
                                OldValue = oldVendor.FirstName,
                                NewValue = newVendor.FirstName
                            });
                            if (hasOtherDeal)
                                histories.Add(new UserHistory()
                                {
                                    Activity = HistoryMessage.VendorFirstName,
                                    DealId = otherDealId,
                                    OldValue = oldVendor.FirstName,
                                    NewValue = newVendor.FirstName
                                });
                        }
                        if (oldVendor.MiddleName != newVendor.MiddleName)
                        {
                            hasAmendments = true;
                            histories.Add(new UserHistory()
                            {
                                Activity = HistoryMessage.VendorMiddleName,
                                DealId = dealId,
                                OldValue = oldVendor.MiddleName,
                                NewValue = newVendor.MiddleName
                            });
                            if (hasOtherDeal)
                                histories.Add(new UserHistory()
                                {
                                    Activity = HistoryMessage.VendorMiddleName,
                                    DealId = otherDealId,
                                    OldValue = oldVendor.MiddleName,
                                    NewValue = newVendor.MiddleName
                                });
                        }
                        if (oldVendor.LastName != newVendor.LastName)
                        {
                            hasAmendments = true;
                            histories.Add(new UserHistory()
                            {
                                Activity = HistoryMessage.VendorLastName,
                                DealId = dealId,
                                OldValue = oldVendor.LastName,
                                NewValue = newVendor.LastName
                            });
                            if (hasOtherDeal)
                                histories.Add(new UserHistory()
                                {
                                    Activity = HistoryMessage.VendorLastName,
                                    DealId = otherDealId,
                                    OldValue = oldVendor.LastName,
                                    NewValue = newVendor.LastName
                                });
                        }
                    }
                }
            }
            return hasAmendments;
        }

        public bool CheckLLCAmendments(FundingDeal targetFundingDeal, FundingDeal sourceFundingDeal, UserContext user)
        {
            const string typeBusiness = "BUSINESS";
            bool hasAmendments = false;

            if (targetFundingDeal == null || sourceFundingDeal == null) return false;

            //LLC deal should be acting for purchaser, the target deal should be acting for vendor
            if (sourceFundingDeal.ActingFor == LawyerActingFor.Both ||
                sourceFundingDeal.ActingFor == LawyerActingFor.Mortgagor ||
                sourceFundingDeal.ActingFor == LawyerActingFor.Vendor) return false;

            int targetDealId = targetFundingDeal.DealID.GetValueOrDefault();

            IEnumerable<UserHistory> histories = GetLLCDealHistoriesForAmendments(targetFundingDeal, sourceFundingDeal,
                targetDealId, typeBusiness, ref hasAmendments);

            _dealHistoryRepository.CreateDealChangeHistories(histories, user);

            //Report amendments only when deal is active 
            if (sourceFundingDeal.DealStatus != DealStatus.Active || targetFundingDeal.DealStatus != DealStatus.Active)
                hasAmendments = false;
            return hasAmendments;
        }

        public static IEnumerable<UserHistory> GetLLCDealHistoriesForAmendments(FundingDeal targetFundingDeal, FundingDeal sourceFundingDeal,
            int targetDealId, string TYPE_BUSINESS, ref bool hasAmendments)
        {
            var histories = new List<UserHistory>();

            hasAmendments = CheckLLCClosingDateAmendment(targetFundingDeal, sourceFundingDeal, hasAmendments, histories,
                targetDealId);

            CheckIfLLCPurchaserAddedOrDeleted(targetFundingDeal, sourceFundingDeal, histories, targetDealId);

            CheckIfLLCPurchaserUpdated(targetFundingDeal, sourceFundingDeal, histories, targetDealId, TYPE_BUSINESS);

            hasAmendments = CheckLLCPropertyAmendments(targetFundingDeal, sourceFundingDeal, hasAmendments, histories,
                targetDealId);

            hasAmendments = CheckLLCPINAmendments(targetFundingDeal, sourceFundingDeal, hasAmendments, histories, targetDealId);
            return histories;
        }

        private static bool CheckLLCClosingDateAmendment(FundingDeal targetFundingDeal, FundingDeal sourceFundingDeal,
            bool hasAmendments, List<UserHistory> histories, int targetDealId)
        {
//Closing Date
            if (targetFundingDeal.ClosingDate != sourceFundingDeal.ClosingDate)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.ClosingDateChange,
                    DealId = targetDealId,
                    OldValue =
                        targetFundingDeal.ClosingDate == null
                            ? string.Empty
                            : targetFundingDeal.ClosingDate.Value.ToLongDateString(),
                    NewValue =
                        sourceFundingDeal.ClosingDate == null
                            ? string.Empty
                            : sourceFundingDeal.ClosingDate.Value.ToLongDateString()
                });
            }
            return hasAmendments;
        }

        private static void CheckIfLLCPurchaserAddedOrDeleted(FundingDeal targetFundingDeal, FundingDeal sourceFundingDeal,
            List<UserHistory> histories, int targetDealId)
        {
            bool hasAmendments;
//find added Mortgagor list
            var addedMortgagors =
                sourceFundingDeal.Mortgagors.Where(
                    m => targetFundingDeal.Mortgagors.All(m1 => m1.SourceID != m.MortgagorID));
            foreach (var addedMortgagor in addedMortgagors)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.PurchaserAdded,
                    DealId = targetDealId,
                });
            }
            //find deleted Mortgagor list
            var deletedMortgagors =
                targetFundingDeal.Mortgagors.Where(
                    m => sourceFundingDeal.Mortgagors.All(m1 => m1.MortgagorID != m.SourceID));
            foreach (var deletedMortgagor in deletedMortgagors)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.PurchaserRemoved,
                    DealId = targetDealId,
                });
            }
        }

        private static void CheckIfLLCPurchaserUpdated(FundingDeal targetFundingDeal, FundingDeal sourceFundingDeal,
            List<UserHistory> histories, int targetDealId, string TYPE_BUSINESS)
        {
            //Get updated Mortgagor list
            bool hasAmendments;
            List<Mortgagor> newMortgagors =
                sourceFundingDeal.Mortgagors.Where(m => targetFundingDeal.Mortgagors.Any(m1 => m1.SourceID == m.MortgagorID))
                    .ToList();
            foreach (Mortgagor newMortgagor in newMortgagors)
            {
                Mortgagor oldMortgagor =
                    targetFundingDeal.Mortgagors.SingleOrDefault(m => m.SourceID == newMortgagor.MortgagorID);
                if (oldMortgagor.MortgagorType != newMortgagor.MortgagorType)
                {
                    hasAmendments = true;
                    histories.Add(new UserHistory()
                    {
                        Activity = HistoryMessage.PurchaserTypeChanged,
                        DealId = targetDealId,
                        OldValue = oldMortgagor.MortgagorType,
                        NewValue = newMortgagor.MortgagorType
                    });
                }
                else
                {
                    if (oldMortgagor.MortgagorType.ToUpper() == TYPE_BUSINESS)
                    {
                        //Mortgagor Information – Business: Company Name, Contact First Name, Contact Last Name
                        if (oldMortgagor.CompanyName != newMortgagor.CompanyName)
                        {
                            hasAmendments = true;
                            histories.Add(new UserHistory()
                            {
                                Activity = HistoryMessage.PurchaserCompanyName,
                                DealId = targetDealId,
                                OldValue = oldMortgagor.CompanyName,
                                NewValue = newMortgagor.CompanyName
                            });
                        }
                        if (oldMortgagor.FirstName != newMortgagor.FirstName)
                        {
                            hasAmendments = true;
                            histories.Add(new UserHistory()
                            {
                                Activity = HistoryMessage.PurchaserContactFirstName,
                                DealId = targetDealId,
                                OldValue = oldMortgagor.FirstName,
                                NewValue = newMortgagor.FirstName
                            });
                        }
                        if (oldMortgagor.LastName != newMortgagor.LastName)
                        {
                            hasAmendments = true;
                            histories.Add(new UserHistory()
                            {
                                Activity = HistoryMessage.PurchaserContactLastName,
                                DealId = targetDealId,
                                OldValue = oldMortgagor.LastName,
                                NewValue = newMortgagor.LastName
                            });
                        }
                    }
                    else
                    {
                        //Mortgagor Information – Person: First Name, Middle Name, Last Name
                        if (oldMortgagor.FirstName != newMortgagor.FirstName)
                        {
                            hasAmendments = true;
                            histories.Add(new UserHistory()
                            {
                                Activity = HistoryMessage.PurchaserFirstName,
                                DealId = targetDealId,
                                OldValue = oldMortgagor.FirstName,
                                NewValue = newMortgagor.FirstName
                            });
                        }
                        if (oldMortgagor.MiddleName != newMortgagor.MiddleName)
                        {
                            hasAmendments = true;
                            histories.Add(new UserHistory()
                            {
                                Activity = HistoryMessage.PurchaserMiddleName,
                                DealId = targetDealId,
                                OldValue = oldMortgagor.MiddleName,
                                NewValue = newMortgagor.MiddleName
                            });
                        }
                        if (oldMortgagor.LastName != newMortgagor.LastName)
                        {
                            hasAmendments = true;
                            histories.Add(new UserHistory()
                            {
                                Activity = HistoryMessage.PurchaserLastName,
                                DealId = targetDealId,
                                OldValue = oldMortgagor.LastName,
                                NewValue = newMortgagor.LastName
                            });
                        }
                    }
                }
            }
        }

        private static bool CheckLLCPINAmendments(FundingDeal targetFundingDeal, FundingDeal sourceFundingDeal,
            bool hasAmendments, List<UserHistory> histories, int targetDealId)
        {
//PIN(s)
            //find added Property.PIN list
            List<Pin> addedPins =
                sourceFundingDeal.Property.Pins.Where(p => targetFundingDeal.Property.Pins.All(p1 => p1.SourceID != p.PinID))
                    .ToList();
            if (addedPins.Any())
            {
                hasAmendments = true;
                foreach (Pin addedPin in addedPins)
                {
                    histories.Add(new UserHistory()
                    {
                        Activity = HistoryMessage.PinAdd,
                        DealId = targetDealId,
                        OldValue = addedPin.PINNumber
                    });
                }
            }
            //find deleted Property.PIN list
            List<Pin> removedPins =
                targetFundingDeal.Property.Pins.Where(p => sourceFundingDeal.Property.Pins.All(p1 => p1.PinID != p.SourceID))
                    .ToList();
            if (removedPins.Any())
            {
                hasAmendments = true;
                foreach (Pin removedPin in removedPins)
                {
                    histories.Add(new UserHistory()
                    {
                        Activity = HistoryMessage.PinRemove,
                        DealId = targetDealId,
                        OldValue = removedPin.PINNumber
                    });
                }
            }
            //Get updated Property.PIN list
            List<Pin> newPins =
                sourceFundingDeal.Property.Pins.Where(p => targetFundingDeal.Property.Pins.Any(p1 => p1.SourceID == p.PinID))
                    .ToList();
            foreach (Pin newPin in newPins)
            {
                Pin oldPin = targetFundingDeal.Property.Pins.SingleOrDefault(p => p.SourceID == newPin.PinID);
                if (oldPin.PINNumber != newPin.PINNumber)
                {
                    hasAmendments = true;
                    histories.Add(new UserHistory()
                    {
                        Activity = HistoryMessage.PinUpdate,
                        DealId = targetDealId,
                        OldValue = oldPin.PINNumber,
                        NewValue = newPin.PINNumber
                    });
                }
            }
            return hasAmendments;
        }

        private static bool CheckLLCPropertyAmendments(FundingDeal targetFundingDeal, FundingDeal sourceFundingDeal,
            bool hasAmendments, List<UserHistory> histories, int targetDealId)
        {
//Property: Unit Number, Street Number, Address 1, Address 2, City, Province, Postal Code
            if (targetFundingDeal.Property.UnitNumber != sourceFundingDeal.Property.UnitNumber)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.UnitNumberUpdated,
                    DealId = targetDealId,
                    OldValue = targetFundingDeal.Property.UnitNumber,
                    NewValue = sourceFundingDeal.Property.UnitNumber
                });
            }
            if (targetFundingDeal.Property.StreetNumber != sourceFundingDeal.Property.StreetNumber)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.StreetNumberUpdated,
                    DealId = targetDealId,
                    OldValue = targetFundingDeal.Property.StreetNumber,
                    NewValue = sourceFundingDeal.Property.StreetNumber
                });
            }
            if (targetFundingDeal.Property.Address != sourceFundingDeal.Property.Address)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.StreetNameUpdated,
                    DealId = targetDealId,
                    OldValue = targetFundingDeal.Property.Address,
                    NewValue = sourceFundingDeal.Property.Address
                });
            }
            if (targetFundingDeal.Property.Address2 != sourceFundingDeal.Property.Address2)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.StreetNameUpdated,
                    DealId = targetDealId,
                    OldValue = targetFundingDeal.Property.Address2,
                    NewValue = sourceFundingDeal.Property.Address2
                });
            }
            if (targetFundingDeal.Property.City != sourceFundingDeal.Property.City)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.CityUpdated,
                    DealId = targetDealId,
                    OldValue = targetFundingDeal.Property.City,
                    NewValue = sourceFundingDeal.Property.City
                });
            }
            if (targetFundingDeal.Property.Province != sourceFundingDeal.Property.Province)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.PropertyProvince,
                    DealId = targetDealId,
                    OldValue = targetFundingDeal.Property.Province,
                    NewValue = sourceFundingDeal.Property.Province
                });
            }
            if (targetFundingDeal.Property.PostalCode != sourceFundingDeal.Property.PostalCode)
            {
                hasAmendments = true;
                histories.Add(new UserHistory()
                {
                    Activity = HistoryMessage.PostalCodeUpdated,
                    DealId = targetDealId,
                    OldValue = targetFundingDeal.Property.PostalCode,
                    NewValue = sourceFundingDeal.Property.PostalCode
                });
            }
            return hasAmendments;
        }
    }
}
