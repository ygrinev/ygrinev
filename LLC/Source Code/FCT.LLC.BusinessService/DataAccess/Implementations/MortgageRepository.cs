using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess
{
    public sealed class MortgageRepository:Repository<tblMortgage>, IMortgageRepository
    {
        private readonly EFBusinessContext _context;
        public MortgageRepository(EFBusinessContext context) : base(context)
        {
            _context = context;
        }

        public void InsertMortgage(int dealID, DateTime? closingDate)
        {
            var mortgage = new tblMortgage() { DealID = dealID, ClosingDate = closingDate };
            Insert(mortgage);
            _context.SaveChanges();
        }

        public void UpdateMortgage(int dealID, DateTime? closingDate)
        {
            var mortgages = GetAll().Where(m => m.DealID == dealID);
            foreach (var mortgage in mortgages)
            {
                mortgage.ClosingDate = closingDate;
                Update(mortgage);
            }
            _context.SaveChanges();
        }
    }
}
