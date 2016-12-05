using System.Collections.Generic;
using System.Linq;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class LawyerMapper : IEntityMapper<tblLawyer, Lawyer>
    {
        private readonly IEntityMapper<tblDealContact, Contact> _contactMapper;

        public LawyerMapper(IEntityMapper<tblDealContact, Contact> contactMapper)
        {
            _contactMapper = contactMapper;
        }
        public Lawyer MapToData(tblLawyer tEntity, object parameters = null)
        {
            if (tEntity != null)
            {

                var lawyer = new Lawyer
                {
                    LawFirm = tEntity.LawFirm,
                    FirstName = tEntity.FirstName,
                    LastName = tEntity.LastName,
                    LawyerID = tEntity.LawyerID,
                    Phone = tEntity.Phone,
                    DealContacts = new ContactCollection()
                };
                lawyer.DealContacts.AddRange(parameters == null
                    ? tEntity.tblDealContacts.OrderBy(c=>c.DealContactID).Select(d => _contactMapper.MapToData(d))
                    : tEntity.tblDealContacts.OrderBy(c => c.DealContactID).Select(d => _contactMapper.MapToData(d, parameters)));

                return lawyer;
            }
            return null;
        }

        public tblLawyer MapToEntity(Lawyer tData)
        {
            var lawyer = new tblLawyer()
            {
                LawyerID = tData.LawyerID,
                LawFirm = tData.LawFirm,
                FirstName = tData.FirstName,
                LastName = tData.LastName,
                Phone = tData.Phone,
            };
            return lawyer;
        }
    }
}
