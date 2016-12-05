using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public static class TransactionScopeBuilder
    {
        /// <summary>
        /// Creates a transactionscope with ReadCommitted Isolation
        /// </summary>
        /// <returns>A transaction scope</returns>
        public static TransactionScope CreateReadCommitted()
        {
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.DefaultTimeout
            };

            return new TransactionScope(TransactionScopeOption.Required, options);
        }
    }
}
