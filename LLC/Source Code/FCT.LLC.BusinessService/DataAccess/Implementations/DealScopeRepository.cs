using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class DealScopeRepository:Repository<tblDealScope>, IDealScopeRepository
    {
        private readonly EFBusinessContext _context;
        private const string WireDepositSeparator = "WireDepositSeparator";
        public DealScopeRepository(EFBusinessContext context) : base(context)
        {
            _context = context;
        }

        public int InsertDealScope(DealScope dealScope)
        {
            string separator = ConfigurationManager.AppSettings[WireDepositSeparator];
            try
            {
                var dealscope = new tblDealScope()
                {
                    FCTRefNumber = dealScope.FCTRefNumber,
                    ShortFCTRefNumber = dealScope.ShortFCTRefNumber,
                    WireDepositVerificationCode = dealScope.WireDepositVerificationCode,
                    WireDepositDetails =
                        String.Format("{0}{1}{2}", dealScope.FormattedFCTRefNumber,separator, dealScope.WireDepositVerificationCode)
                };
                Insert(dealscope);
                _context.SaveChanges();
                return dealscope.DealScopeID;
            }
            catch (DbEntityValidationException ex)
            {
                
               TraceExceptionInDebugMode(ex);
            }
            return 0;
        }

        //public int GetDealScope(string FCTRefNumberShort, string wireDepositCode=null)
        public int GetDealScope(string GlobalFCTRefNumber, string wireDepositCode = null)
        {
            tblDealScope dealScope;
            if (!string.IsNullOrEmpty(wireDepositCode))
            {
                dealScope =
                    GetAll()
                        .SingleOrDefault(
                            ds => (ds.FCTRefNumber == GlobalFCTRefNumber || ds.ShortFCTRefNumber == GlobalFCTRefNumber) &&
                                ds.WireDepositVerificationCode == wireDepositCode);
            }
            else
            {
                dealScope = GetAll().SingleOrDefault(ds => ds.FCTRefNumber == GlobalFCTRefNumber || ds.ShortFCTRefNumber == GlobalFCTRefNumber);   
            }            
            if (dealScope != null)
                return dealScope.DealScopeID;
            return 0;
        }

        public string GetFCTRefNumber(string GlobalFCTRefNumber)
        {
            var dealscope = GetAll().SingleOrDefault(ds => ds.FCTRefNumber == GlobalFCTRefNumber);
            if (dealscope != null)
            {
                return dealscope.FCTRefNumber;
            }
            return string.Empty;
        }

        public void UpdateWireDepositDetails(DealScope dealScope)
        {
            string separator = ConfigurationManager.AppSettings[WireDepositSeparator];
          
            if (dealScope != null)
            {
                tblDealScope dealScopeToUpdate = _context.tblDealScopes.SingleOrDefault(ds => ds.FCTRefNumber == dealScope.FCTRefNumber);
                    
                if (dealScopeToUpdate != null)
                {
                    dealScopeToUpdate.WireDepositVerificationCode = dealScope.WireDepositVerificationCode;
                    dealScopeToUpdate.WireDepositDetails =
                        String.Format("{0}{1}{2}", dealScope.FormattedFCTRefNumber,separator, dealScope.WireDepositVerificationCode);
                    _context.SaveChanges();
                }

            }
        }


        public void UpdateDealScope(DealScope dealScope)
        {
            string separator = ConfigurationManager.AppSettings[WireDepositSeparator];
            try
            {
                tblDealScope dealScopeToUpdate = _context.tblDealScopes.SingleOrDefault(ds => ds.FCTRefNumber == dealScope.FCTRefNumber);

                if(dealScopeToUpdate != null)
                {
                    dealScopeToUpdate.FCTRefNumber = dealScope.FCTRefNumber;
                    dealScopeToUpdate.ShortFCTRefNumber = dealScope.ShortFCTRefNumber;
                    dealScopeToUpdate.WireDepositVerificationCode = dealScope.WireDepositVerificationCode;
                    dealScopeToUpdate.WireDepositDetails =
                        String.Format("{0}{1}{2}", dealScope.FormattedFCTRefNumber, separator, dealScope.WireDepositVerificationCode);
                };

                Update(dealScopeToUpdate);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {

                TraceExceptionInDebugMode(ex);
            }
            
        }

        public void OverwriteDealScopeDetails(DealScope dealScope)
        {
            string separator = ConfigurationManager.AppSettings[WireDepositSeparator];
            try
            {
                tblDealScope dealScopeToUpdate = _context.tblDealScopes.SingleOrDefault(ds => ds.DealScopeID == dealScope.DealScopeId);

                if (dealScopeToUpdate != null)
                {
                    dealScopeToUpdate.FCTRefNumber = dealScope.FCTRefNumber;
                    dealScopeToUpdate.ShortFCTRefNumber = dealScope.ShortFCTRefNumber;
                    //dealScopeToUpdate.WireDepositVerificationCode = dealScope.WireDepositVerificationCode;
                    //dealScopeToUpdate.WireDepositDetails =
                    //    String.Format("{0}{1}{2}", dealScope.FormattedFCTRefNumber, separator, dealScope.WireDepositVerificationCode);
                };

                Update(dealScopeToUpdate);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {

                TraceExceptionInDebugMode(ex);
            }

        }

        public void DeleteDealScope(int DealScopeID)
        {
            try
            {
                //Delete the Orphan DealScope records
                if (!_context.tblDeals.Any(d => d.DealScopeID == DealScopeID))
                {
                    var dealscopeToBeRemoved = _context.tblDealScopes.Find(DealScopeID);
                    
                    if (null != dealscopeToBeRemoved)
                    {
                        _context.tblDealScopes.Remove(dealscopeToBeRemoved);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error in deleting Deal" + Environment.NewLine + "Error Message: " + e.Message.ToString());
            }
        }

        [Conditional("DEBUG")]
        private static void TraceExceptionInDebugMode(DbEntityValidationException dbEx)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                        validationError.ErrorMessage);
                }
            }
        }
    }
}
