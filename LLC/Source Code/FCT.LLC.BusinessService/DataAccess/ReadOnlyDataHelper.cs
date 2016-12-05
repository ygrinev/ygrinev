using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class ReadOnlyDataHelper
    {
        private readonly EFBusinessContext _context;
        public ReadOnlyDataHelper(EFBusinessContext context)
        {
            _context = context;
        }

        public bool IsSystemClosed(DateTime dateTime)
        {
            var holidays = _context.Database.SqlQuery<string>("dbo.usp_isSystemClosed @datetime",
                new SqlParameter("@datetime", dateTime)).ToList();
            if (holidays.Count > 0)
            {
                return true;
            }
            return false;
        }

        public void StartGuardium()
        {
            IPrincipal threadPrincipal = Thread.CurrentPrincipal;
            string UserName = threadPrincipal.Identity.Name;

            var param1 = new SqlParameter("@UserID", UserName);

            _context.Database.ExecuteSqlCommand("spGuardiumStart @UserID", param1);
        }

        public IDictionary<string, string> GetConfigurationValues()
        {
            return _context.Database.SqlQuery<Lookup>("dbo.usp_getFCTFeeOptions @categoryName",
                new SqlParameter("@categoryName", EasyFundFee.CategoryName)).ToDictionary(x => x.Key, x => x.Value);

        }

    }
}
