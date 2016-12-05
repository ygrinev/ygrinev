using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess.Interfaces
{
    public interface IFeeRepository:IRepository<tblFee>
    {
        Fee InsertFee(int disbursementCount, string propertyProvince, bool splitEqually);

        Fee UpdateFee(int disbursementCount, string propertyProvince, bool splitEqually, int feeID);

        Fee CalculateFee(int disbursementCount, string propertyProvince, bool splitEqually, int feeID);

        decimal CalculateBaseFee(int disbursementCount, bool splitEqually);

        void CalculateFee(string propertyProvince, int feeId);

        Fee CalculateFee(string propertyProvince);

        int SaveFee(Fee fee);

        decimal GetBaseFeeFromConfiguration(string key);

        DisbursementFee InsertFees(int disbursementCount, string propertyProvince, bool splitEqually);
    }
}
