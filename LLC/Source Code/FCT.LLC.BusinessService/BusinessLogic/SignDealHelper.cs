using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.Common.NotificationEmailDispatching.Client;
using FCT.LLC.Common.NotificationEmailDispatching.Entities;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class SignDealHelper
    {
        public static bool AreAMatch(UniqueDealDescriptor d1, UniqueDealDescriptor d2)
        {
            IList<string> mortgagors = d1.Mortgagors.Select(HashProvider.GetMD5Hash).ToList();
            IList<string> vendors = d1.Vendors.Select(HashProvider.GetMD5Hash).ToList();
            IList<string> pins = d1.Pins.Select(HashProvider.GetMD5Hash).ToList();
            IList<bool> hasHashedList = new List<bool>();

            if (d1.Mortgagors.Count != d2.Mortgagors.Count)
            {
                return false;
            }

            if (d1.Vendors.Count != d2.Vendors.Count)
            {
                return false;
            }

            foreach (var mortgagor in d2.Mortgagors)
            {
                string hash = HashProvider.GetMD5Hash(mortgagor);
                hasHashedList.Add(mortgagors.Any(m => m.Equals(hash)));
            }
            if (hasHashedList.Any(h => h.Equals(false)))
            {
                return false;
            }
            hasHashedList.Clear();

            foreach (var vendor in d2.Vendors)
            {
                string hash = HashProvider.GetMD5Hash(vendor);
                hasHashedList.Add(vendors.Any(m => m.Equals(hash)));
            }

            if (hasHashedList.Any(h => h.Equals(false)))
            {
                return false;
            }
            hasHashedList.Clear();

            foreach (var pin in d2.Pins)
            {
                string hash = HashProvider.GetMD5Hash(pin);
                if (pins.Any(m => m.Equals(hash)))
                {
                    hasHashedList.Add(true);
                }
            }
            if (hasHashedList.Count != d2.Pins.Count && hasHashedList.Any(h => h.Equals(false)))
            {
                return false;
            }
            hasHashedList.Clear();

            if (d1.ClosingDate == null && d2.ClosingDate == null)
            {
                if (HashProvider.GetMD5Hash(d1.Property) == HashProvider.GetMD5Hash(d2.Property))
                {
                    return true;
                }
            }
            if (HashProvider.GetMD5Hash(d1.Property) == HashProvider.GetMD5Hash(d2.Property) &&
                HashProvider.GetMD5Hash(d1.ClosingDate) == HashProvider.GetMD5Hash(d2.ClosingDate))
            {
                return true;
            }
            return false;
        }

        public static bool AreAMatch(DisbursementCollection d1, DisbursementCollection d2)
        {
            IList<string> d1Hashes = d1.Select(HashProvider.GetMD5Hash).ToList();
            IList<bool> hasHashedList = (from disbursement in d2
                                         select HashProvider.GetMD5Hash(disbursement)
                                             into hash
                                             where d1Hashes.Any(m => m.Equals(hash))
                                             select true).ToList();

            if (hasHashedList.Count != d2.Count && hasHashedList.Any(h => h.Equals(false)))
            {
                return false;
            }
            return true;
        }
    }
}
