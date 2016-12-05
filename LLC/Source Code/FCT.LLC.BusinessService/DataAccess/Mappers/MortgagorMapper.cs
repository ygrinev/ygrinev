using System.Collections.Generic;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class MortgagorMapper:IEntityMapper<tblMortgagor, Mortgagor>
    {
        public Mortgagor MapToData(tblMortgagor tEntity, object parameters = null)
        {
            if (tEntity != null)
            {
                var mortgagor = new Mortgagor()
                {
                    CompanyName = tEntity.CompanyName,
                    FirstName = tEntity.FirstName,
                    LastName = tEntity.LastName,
                    MiddleName = tEntity.MiddleName,
                    MortgagorID = tEntity.MortgagorID,
                    MortgagorType = tEntity.MortgagorType,
                    SourceID = tEntity.SourceID
                };
                return mortgagor; 
            }
          return null;
        }

        public tblMortgagor MapToEntity(Mortgagor tData)
        {
            var mortgagor = new tblMortgagor()
            {
                CompanyName = tData.CompanyName,
                FirstName = tData.FirstName,
                LastName = tData.LastName,
                MiddleName = tData.MiddleName,
                MortgagorType = tData.MortgagorType,
                SourceID = tData.SourceID
            };
            if (tData.MortgagorID.HasValue)
            {
                mortgagor.MortgagorID =(int) tData.MortgagorID;
            }

            return mortgagor;
        }

        internal static tblMortgagor SyncEntities(tblMortgagor source)
        {
            if (source != null)
            {
                var target = new tblMortgagor
                {
                    Address = source.Address,
                    Address2 = source.Address2,
                    BirthDate = source.BirthDate,
                    BusinessPhone = source.BusinessPhone,
                    City = source.City,
                    CompanyName = source.CompanyName,
                    CompanyProvinceOfIncorporation = source.CompanyProvinceOfIncorporation,
                    Country = source.Country,
                    FirstName = source.FirstName,
                    HasSpouse = source.HasSpouse,
                    HomePhone = source.HomePhone,
                    IsGuarantor = source.IsGuarantor,
                    IsILARequired = source.IsILARequired,
                    IsSpouseILARequired = source.IsSpouseILARequired,
                    Language = source.Language,
                    LastName = source.LastName,
                    LenderMortgagorID = source.LenderMortgagorID,
                    MiddleName = source.MiddleName,
                    MortgagorType = source.MortgagorType,
                    Occupation = source.Occupation,
                    PostalCode = source.PostalCode,
                    PrimaryIdentificationID = source.PrimaryIdentificationID,
                    PriorityIndicator = source.PriorityIndicator,
                    Province = source.Province,
                    SecondaryIdentificationID = source.SecondaryIdentificationID,
                    SpousalStatement = source.SpousalStatement,
                    SpouseFirstName = source.SpouseFirstName,
                    SpouseLastName = source.SpouseLastName,
                    SpouseMiddleName = source.SpouseMiddleName,
                    SpouseOccupation = source.SpouseOccupation,
                    SpousePrimaryIdentificationID = source.SpousePrimaryIdentificationID,
                    StreetNumber = source.StreetNumber,
                    UnitNumber = source.UnitNumber,
                };
                return target;
            }
            return null;
        }
    }
}
