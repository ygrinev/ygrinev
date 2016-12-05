using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public interface IMortgageRepository:IRepository<tblMortgage>
    {
        void InsertMortgage(int dealID, DateTime? closingDate);

        void UpdateMortgage(int dealID, DateTime? closingDate);
    }
}
