using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess
{
   public class DealContactRepository: Repository<tblDealContact>, IDealContactRepository
   {
       private readonly EFBusinessContext _context;
       private readonly IEntityMapper<tblDealContact, Contact> _contactMapper; 
       public DealContactRepository(EFBusinessContext context, IEntityMapper<tblDealContact, Contact> contactMapper) : base(context)
       {
           _contactMapper = contactMapper;
           _context = context;
       }

       public ContactCollection InsertDealContactRange(IEnumerable<Contact> contacts, int dealID)
       {
           if (contacts != null)
           {
               var entities = contacts.Select(c => _contactMapper.MapToEntity(c));
               var results = new List<tblDealContact>();
               foreach (var entity in entities)
               {
                   entity.DealID = dealID;
                   results.Add(entity);
               }
               InsertRange(results);
               _context.SaveChanges();

               var insertedContacts = results.Select(r => _contactMapper.MapToData(r));
               var collection = new ContactCollection();
               collection.AddRange(insertedContacts);
               return collection;  
           }
           return null;
       }

       public void UpdateDealContactRange(IEnumerable<Contact> contacts, int dealID)
       {
           if (contacts != null)
           {
               var results = GetDealContacts(dealID);
               var dealContacts = contacts.Select(c => _contactMapper.MapToEntity(c));
               var updatedContacts = new List<tblDealContact>();
               foreach (var dealContact in dealContacts)
               {
                   dealContact.DealID = dealID;
                   updatedContacts.Add(dealContact);
               }
               var deletedContacts = results.Except(updatedContacts, new ContactEqualityComparer());
               if (deletedContacts.Any())
               {
                   DeleteRange(deletedContacts);
                   _context.SaveChanges();
  
               }

               _context.Configuration.AutoDetectChangesEnabled = false;

               foreach (var updatedContact in updatedContacts)
               {
                   var dealContact = GetAll()
                       .SingleOrDefault(c => c.DealID == dealID && c.LawyerID == updatedContact.LawyerID);
                   if (dealContact != null)
                   {
                       updatedContact.DealContactID = dealContact.DealContactID;
                       Update(updatedContact);
                   }
                   else
                   {
                       Insert(updatedContact);
                   }
               }
               _context.SaveChanges();
               _context.Configuration.AutoDetectChangesEnabled = true;
           }
       }

       public IEnumerable<tblDealContact> GetDealContacts(int dealId)
       {
           var results = GetAll().Where(v => v.DealID == dealId).AsEnumerable();
           return results;
       }

       public IEnumerable<int> GetDealContactIDs(int dealId)
       {
           var ids = GetAll().Where(c => c.DealID == dealId).Select(c => c.LawyerID).ToList();
           return ids;
       }

       private class ContactEqualityComparer : IEqualityComparer<tblDealContact>
       {
           public bool Equals(tblDealContact x, tblDealContact y)
           {
               return x.LawyerID== y.LawyerID && x.DealID ==y.DealID;
           }

           public int GetHashCode(tblDealContact obj)
           {
               return ((obj.LawyerID.ToString()) + (obj.DealID.ToString())).GetHashCode();
           }
       }
   }
}
