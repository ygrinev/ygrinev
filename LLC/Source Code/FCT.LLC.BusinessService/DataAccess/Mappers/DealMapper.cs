using System;
using System.Collections.Generic;
using System.Linq;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;


namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class DealMapper : IEntityMapper<tblDeal, Deal>
    {
        private readonly IEntityMapper<tblLawyer, Lawyer> _lawyerMapper;
        private readonly IEntityMapper<tblMortgagor, Mortgagor> _mortgagorMapper;
        private readonly IEntityMapper<tblVendor, Vendor> _vendorMapper;
        private readonly IEntityMapper<tblProperty, Property> _propertymapper; 
        private readonly IEntityMapper<tblLender, Lender> _lenderMapper;
        private readonly IEntityMapper<tblMortgage, Mortgage> _mortgageMapper;

        public DealMapper(IEntityMapper<tblLawyer, Lawyer> lawyerMapper,
            IEntityMapper<tblMortgagor, Mortgagor> mortgagorMapper, IEntityMapper<tblVendor, Vendor> vendorMapper,
            IEntityMapper<tblProperty, Property> propertymapper,IEntityMapper<tblLender, Lender> lenderMapper,
            IEntityMapper<tblMortgage, Mortgage> mortgageMapper)
        {
            _lawyerMapper = lawyerMapper;
            _lenderMapper = lenderMapper;
            _mortgagorMapper = mortgagorMapper;
            _vendorMapper = vendorMapper;
            _propertymapper = propertymapper;
            _mortgageMapper = mortgageMapper;
        }

        public Deal MapToData(tblDeal tEntity, object parameters = null)
        {
            if (tEntity != null)
            {
                Deal deal = new Deal
                {
                    DealID = tEntity.DealID,
                    DealFCTURN = tEntity.FCTRefNum,
                    BusinessModel = tEntity.BusinessModel,
                    DealStatus = tEntity.Status,
                    DealType = tEntity.DealType,
                    LawyerActingFor = tEntity.LawyerActingFor,
                    LawyerApplication = tEntity.LawyerApplication,
                    LawyerFileNumber = tEntity.LawyerMatterNumber,
                    LenderRefNumber = tEntity.LenderRefNum,
                    Property = _propertymapper.MapToData(tEntity.tblProperties.FirstOrDefault()),
                    Mortgagors = new MortgagorCollection()
                };
                if (tEntity.tblDealScope != null)
                {
                    deal.DealScopeFCTURN = tEntity.tblDealScope.FCTRefNumber; //tEntity.tblDealScope.ShortFCTRefNumber;


                    //Currently vendors are not part of Data Contracts
                    //deal.Vendors.AddRange(tEntity.tblDealScope.tblVendors.Select(v => _vendorMapper.MapToData(v)));
                }

                if (tEntity.tblLawyer != null)
                {
                    deal.Lawyer = _lawyerMapper.MapToData(tEntity.tblLawyer);
                }

                deal.Mortgagors.AddRange(tEntity.tblMortgagors.Select(m => _mortgagorMapper.MapToData(m)));              
                
                if (tEntity.tblLender != null)
                {
                    deal.Lender = _lenderMapper.MapToData(tEntity.tblLender);
                }

                if (tEntity.tblMortgages != null)
                {
                    deal.Mortgage = _mortgageMapper.MapToData(tEntity.tblMortgages.FirstOrDefault());
                }
                deal.ClosingDate = ComposeClosingDate(tEntity.tblMortgages);
                return deal;
            }
            return null;
        }

        private DateTime ComposeClosingDate(ICollection<tblMortgage> mortgages)
        {
            if (mortgages.Count > 0 && mortgages.FirstOrDefault().ClosingDate.HasValue)
            {
                return mortgages.First().ClosingDate.Value;
            }

            return DateTime.MinValue;//DateTime.MinValue;

        }

        public tblDeal MapToEntity(Deal tData)
        {
            var deal = new tblDeal
            {
                FCTRefNum = tData.DealFCTURN,
                BusinessModel = tData.BusinessModel,
                Status = tData.DealStatus,
                DealType = tData.DealType,
                LawyerMatterNumber = tData.LawyerFileNumber,
                LawyerActingFor = tData.LawyerActingFor,
                LawyerApplication = tData.LawyerApplication,
                LawyerID = tData.Lawyer.LawyerID,
                StatusUserID = tData.Lawyer.LawyerID,
                DealID = tData.DealID
            };
            
            return deal;
        }
    }
}
