using System;
using System.Collections.Generic;
using System.Linq;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class FundingDealMapper : IEntityMapper<tblDeal, FundingDeal>
    {
        private readonly IEntityMapper<tblLawyer, Lawyer> _lawyerMapper;
        private readonly IEntityMapper<tblMortgagor, Mortgagor> _mortgagorMapper;
        private readonly IEntityMapper<tblVendor, Vendor> _vendorMapper;
        private readonly IEntityMapper<tblProperty, Property> _propertymapper;


        public FundingDealMapper(IEntityMapper<tblLawyer, Lawyer> lawyerMapper,
            IEntityMapper<tblMortgagor, Mortgagor> mortgagorMapper, IEntityMapper<tblVendor, Vendor> vendorMapper,
            IEntityMapper<tblProperty, Property> propertymapper)
        {
            _lawyerMapper = lawyerMapper;
            _mortgagorMapper = mortgagorMapper;
            _vendorMapper = vendorMapper;
            _propertymapper = propertymapper;
        }

        public FundingDeal MapToData(tblDeal tEntity, object parameters = null)
        {
            if (tEntity != null)
            {
                var deal = new FundingDeal
                {
                    DealID = tEntity.DealID,
                    ActingFor = tEntity.LawyerActingFor,
                    BusinessModel = tEntity.BusinessModel,
                    DealStatus = tEntity.Status,
                    DealType = tEntity.DealType,
                    PrimaryDealContact = new Contact { ContactID = tEntity.PrimaryDealContactID ?? -1 },
                    LawyerFileNumber = tEntity.LawyerMatterNumber,
                    LawyerApplication = tEntity.LawyerApplication,
                    Property = _propertymapper.MapToData(tEntity.tblProperties.FirstOrDefault()),
                    Vendors = new VendorCollection(),
                    Mortgagors = new MortgagorCollection()
                };
                if (tEntity.tblDealScope != null)
                {
                    deal.Vendors.AddRange(tEntity.tblDealScope.tblVendors.OrderBy(v=>v.VendorID).Select(v => _vendorMapper.MapToData(v))); 
                }
                deal.Lawyer = parameters != null ? _lawyerMapper.MapToData(tEntity.tblLawyer, parameters) : _lawyerMapper.MapToData(tEntity.tblLawyer);
                deal.Mortgagors.AddRange(tEntity.tblMortgagors.OrderBy(m=>m.MortgagorID).Select(m => _mortgagorMapper.MapToData(m)));
                return deal;
            }
            return null;
        }

        public tblDeal MapToEntity(FundingDeal tData)
        {
            var deal = new tblDeal
            {
                FCTRefNum = tData.FCTURN,
                BusinessModel = tData.BusinessModel,
                Status = tData.DealStatus,
                DealType = tData.DealType,
                LawyerMatterNumber = tData.LawyerFileNumber,
                LawyerActingFor = tData.ActingFor,
                //LawyerApplication = LawyerApplication.Portal,
                LawyerApplication = string.IsNullOrEmpty(tData.LawyerApplication) ? LawyerApplication.Portal : tData.LawyerApplication,
                LawyerID = tData.Lawyer.LawyerID,
                StatusUserID = tData.Lawyer.LawyerID
            };
            if (tData.DealID.HasValue)
            {
                deal.DealID = (int) tData.DealID;
            }

            if (tData.PrimaryDealContact != null && tData.PrimaryDealContact.ContactID > 0)
            {
                deal.PrimaryDealContactID = tData.PrimaryDealContact.ContactID;
            }
            return deal;
        }
    }
}
