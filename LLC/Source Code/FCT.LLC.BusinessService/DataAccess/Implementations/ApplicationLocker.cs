using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.Logging;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class ApplicationLocker:IApplicationLocker
    {
        private readonly EFBusinessContext _context;
        private readonly ILogger _logger;
        private const string AppLockTimeOut = "AppLockTimeOut";
        public ApplicationLocker(EFBusinessContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public int GetApplicationLock(int fundingDealId, List<ErrorCode> errorCodes)
        {
            var output = new SqlParameter()
            {
                ParameterName = "@Result",
                SqlDbType = SqlDbType.Int,
                IsNullable = true,
                Direction = ParameterDirection.Output
            };
            int locktimeout = int.Parse(ConfigurationManager.AppSettings[AppLockTimeOut]);

            _context.Database.ExecuteSqlCommand("EXEC @Result=sys.sp_getapplock @Resource, @LockMode, @LockOwner, @LockTimeout",
                new SqlParameter("@Resource", fundingDealId),
                new SqlParameter("@LockMode", "Exclusive"),
                new SqlParameter("@LockOwner", "Transaction"),
                new SqlParameter("@LockTimeout", locktimeout), output);

            int returncode = (int) output.Value;

            if (returncode >= 0)
            {
                if (returncode > 0)
                {
                    _logger.LogError(
                        "The lock was granted successfully after waiting for other incompatible locks to be released.");
                }
            }
            else
            {
                _logger.LogError("lock request failed");
                errorCodes.Add(ErrorCode.ConcurrencyCheckFailed);
            }

            return returncode;
        }

        public int ReleaseApplicationLock()
        {
            var output = new SqlParameter()
            {
                ParameterName = "@Result",
                SqlDbType = SqlDbType.Int,
                IsNullable = true,
                Direction = ParameterDirection.Output
            };
            _context.Database.ExecuteSqlCommand("EXEC @Result=sys.sp_releaseapplock @Resource",
                new SqlParameter("@Resource", "FundingDealID"), output);

            int returncode = (int) output.Value;

            if (returncode >= 0)
            {
                return returncode;
            }
            throw new ApplicationException("System failed to release lock on resources");
        }
    }
}
