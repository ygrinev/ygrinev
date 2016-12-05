using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Implementations;
using System.Data.SqlClient;

namespace FCT.LLC.BusinessService.DataAccess.Implementations
{
    public class FeeRepository:Repository<tblFee>, IFeeRepository
    {
        private readonly EFBusinessContext _context;
        private readonly IEntityMapper<tblFee, Fee> _feeMapper;
        private readonly ReadOnlyDataHelper _readOnlyDataHelper;

        public FeeRepository(EFBusinessContext context, IEntityMapper<tblFee, Fee> feeMapper,
            ReadOnlyDataHelper readOnlyDataHelper) : base(context)
        {
            _context = context;
            _feeMapper = feeMapper;
            _readOnlyDataHelper = readOnlyDataHelper;
        }

        public Fee InsertFee(int disbursementCount, string propertyProvince, bool splitEqually)
        {            
            var taxRate = GetTaxRatesByProvince(propertyProvince);
            if (taxRate!=null)
            {
                var entity = CalculateEasyfundFee(disbursementCount, splitEqually, taxRate);
                var data = InsertFee(entity);
                return data;
            }

            return null;
        }

        private Fee InsertFee(tblFee entity)
        {
            Insert(entity);
            _context.SaveChanges();
            var data = _feeMapper.MapToData(entity);
            return data;
        }

        public Fee UpdateFee(int disbursementCount, string propertyProvince, bool splitEqually, int feeID)
        {
            var taxRate = GetTaxRatesByProvince(propertyProvince);
            if (taxRate != null)
            {
                var entity = CalculateEasyfundFee(disbursementCount, splitEqually, taxRate);
                entity.FeeID = feeID;
                Update(entity);
                _context.SaveChanges();
                var data = _feeMapper.MapToData(entity);
                return data;
            }
            return null;
        }

        public Fee CalculateFee(int disbursementCount, string propertyProvince, bool splitEqually, int feeID)
        {
            var feeConfigs = GetFCTFeeOptions();

            var taxRate = GetTaxRatesByProvince(propertyProvince);
            if (feeConfigs != null && taxRate != null)
            {
                var entity = CalculateEasyfundFee(disbursementCount, splitEqually, taxRate);
                entity.FeeID = feeID;
                var data = _feeMapper.MapToData(entity);
                return data;
            }
            return null;
        }

        public void CalculateFee(string propertyProvince, int feeId)
        {
            var taxRate = GetTaxRatesByProvince(propertyProvince);
            var fee = GetAll().SingleOrDefault(f => f.FeeID == feeId);
            if (fee != null)
            {
                var baseFee = fee.Amount;
                AssignFeeValues(taxRate, fee, baseFee);
                Update(fee);
                _context.SaveChanges();
            }
        }

        public Fee CalculateFee(string propertyProvince)
        {
            var feeConfigs = GetFCTFeeOptions();
            var taxRate = GetTaxRatesByProvince(propertyProvince);
            decimal baseFee = Convert.ToDecimal(feeConfigs[EasyFundFee.FCTReturnFundsFee]);
            var fee = new tblFee();
            AssignFeeValues(taxRate, fee, baseFee);
            var data = _feeMapper.MapToData(fee);
            return data;
        }

        public int SaveFee(Fee fee)
        {
            var entity = _feeMapper.MapToEntity(fee);
            var data = InsertFee(entity);
            return data.FeeID.GetValueOrDefault();
        }

        public decimal GetBaseFeeFromConfiguration(string key)
        {
            string value;
            var feeConfigs = GetFCTFeeOptions();
            if (feeConfigs.TryGetValue(key, out value))
            {
                decimal fee = Convert.ToDecimal(value);
                return fee;
            }
            return 0;
        }

        private tblFee CalculateEasyfundFee(int disbursementCount, bool splitEqually, ProvinceTax taxRate)
        {           
            var baseFee = CalculateBaseFee(disbursementCount, splitEqually);

            var fee = new tblFee();
            if (disbursementCount <= 0)
                AssignFeeValues(taxRate, fee, 0);
            else
                AssignFeeValues(taxRate, fee, baseFee);
            return fee;
        }

        private static void AssignFeeValues(ProvinceTax taxRate, tblFee fee, decimal baseFee)
        {
            if (taxRate.GSTRate > 0)
            {
                fee.GST = Math.Round(baseFee*taxRate.GSTRate, 2, MidpointRounding.AwayFromZero);
            }
            if (taxRate.HSTRate > 0)
            {
                fee.HST = Math.Round(baseFee*taxRate.HSTRate, 2, MidpointRounding.AwayFromZero);
            }
            if (taxRate.QSTRate > 0)
            {
                fee.QST = Math.Round(baseFee*taxRate.QSTRate, 2, MidpointRounding.AwayFromZero);
            }
            fee.Amount = Math.Round(baseFee, 2, MidpointRounding.AwayFromZero);
        }

        public decimal CalculateBaseFee(int disbursementCount, bool splitEqually)
        {
            var feeConfigs = GetFCTFeeOptions();
            decimal baseFee = Convert.ToDecimal(feeConfigs[EasyFundFee.FCTFeeAmount]);
            int incStep = Convert.ToInt32(feeConfigs[EasyFundFee.FCTFeeIncrementStep]);
            while (disbursementCount > incStep)
            {
                baseFee = baseFee + Convert.ToDecimal(feeConfigs[EasyFundFee.FCTFeeIncrementAmount]);
                disbursementCount--;
            }
            if (splitEqually)
            {
                baseFee = baseFee / 2;
            }
            return baseFee;
        }

        public DisbursementFee InsertFees(int disbursementCount, string propertyProvince, bool splitEqually)
        {
            var fees = new DisbursementFee();
            var taxRate = GetTaxRatesByProvince(propertyProvince);
            if (taxRate != null)
            {
                var entity = CalculateEasyfundFee(disbursementCount, splitEqually, taxRate);
                var vendorfee = InsertFee(entity);
                fees.VendorFee = vendorfee;
                var purchaserfee = InsertFee(entity);
                fees.PurchaserFee = purchaserfee;
                return fees;
            }

            return null;
        }

        private ProvinceTax GetTaxRatesByProvince(string propertyProvince)
        {
            return _context.Database.SqlQuery<ProvinceTax>("dbo.usp_getTaxRatesByProvince @province",
                new SqlParameter("@province", propertyProvince)).SingleOrDefault();
        }

        private IDictionary<string,string> GetFCTFeeOptions()
        {
            return _readOnlyDataHelper.GetConfigurationValues();
        }

    }
}
