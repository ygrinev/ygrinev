using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public interface IFeeCalculator
    {

        Fee RecalculateFee(Disbursement disbursement, int disbursementCount, int fundedDealId);

        void ReCalculateFee(string changedProvince, int dealId);

        void ReAssignFee(int dealId);

        DisbursementFee CalculateDefaultFees(int disbursementcount, string province, string actingFor);

        DisbursementFee RecalculateAndSaveFees(Disbursement disbursement, int disbursementCount, int fundedDealId);
    }
}
