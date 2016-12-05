using System;
using System.Collections.Generic;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class ContactMapper:IEntityMapper<tblDealContact, Contact>
    {
        public Contact MapToData(tblDealContact tEntity, object parameters = null)
        {
            if (tEntity != null)
            {
                var contact = new Contact
                {
                    ContactID = tEntity.LawyerID,
                   
                };
                if (parameters != null)
                {
                    var contactDetails = parameters as IEnumerable<DealContactDetails>;
                    if (contactDetails != null)
                    {
                        foreach (var contactDetail in contactDetails)
                        {
                            if (contactDetail.LawyerContactID == tEntity.LawyerID)
                            {
                                contact.FirstName = contactDetail.ContactFirstName;
                                contact.LastName = contactDetail.ContactLastName;
                            }
                        }

                    }
                }
                return contact; 
            }
            return null;
        }

        public tblDealContact MapToEntity(Contact tData)
        {
            if (tData != null)
            {
                var entity = new tblDealContact {LawyerID = tData.ContactID};

                return entity;
            }
            return null;
        }

    }
}
