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
   public class LawyerRepository:Repository<tblLawyer>, ILawyerRepository
   {
       private readonly EFBusinessContext _context;
       public LawyerRepository(EFBusinessContext context) : base(context)
       {
           _context = context;
       }

       public LawyerProfile GetUserDetails(string userName)
       {
           var details = GetAll()
               .Where(l => l.UserID == userName)
               .Select(o => new {o.EMail, o.UserLanguage, o.UserID, o.Profession, o.FirstName, o.LastName, o.Fax, o.LawyerID})
               .AsEnumerable()
               .Select(
                   o =>
                       new LawyerProfile()
                       {
                           Email = o.EMail,
                           UserLanguage = o.UserLanguage,
                           UserType = o.Profession,
                           UserName = o.UserID,
                           FirstName = o.FirstName,
                           LastName = o.LastName,
                           Fax = o.Fax,
                           LawyerID = o.LawyerID
                       });

           return details.SingleOrDefault();
       }

       public IEnumerable<LawyerProfile> GetNotificationDetails(IEnumerable<int> lawyerIds)
       {
           var entities = (from l in _context.tblLawyers where lawyerIds.Contains(l.LawyerID) select l);
           var list =
               entities.Select(
                   tblLawyer =>
                       new LawyerProfile()
                       {
                           Email = tblLawyer.EMail,
                           FirstName = tblLawyer.FirstName,
                           LastName = tblLawyer.LastName,
                           UserLanguage = tblLawyer.UserLanguage,
                           UserName = tblLawyer.UserID,
                           UserType = tblLawyer.Profession,
                           LawyerID = tblLawyer.LawyerID
                       }).ToList();
           return list;
       }

       public LawyerProfile GetNotificationDetails(int lawyerId)
       {
           var notificationDetails = GetAll()
               .Where(l => l.LawyerID == lawyerId)
               .Select(
                   o =>
                       new {o.EMail, o.UserLanguage, o.UserID, o.Profession, o.FirstName, o.LastName, o.Fax, o.LawyerID})
               .AsEnumerable()
               .Select(
                   o =>
                       new LawyerProfile()
                       {
                           Email = o.EMail,
                           UserLanguage = o.UserLanguage,
                           UserType = o.Profession,
                           UserName = o.UserID,
                           FirstName = o.FirstName,
                           LastName = o.LastName,
                           Fax = o.Fax,
                           LawyerID = o.LawyerID
                       });

           return notificationDetails.SingleOrDefault();
       }

       public LawyerProfile GetUserDetails(int lawyerId)
       {
           return GetNotificationDetails(lawyerId);

       }

       public LawyerProfile GetLawyerDetails(int dealId)
       {
           var lawyerDetail =
               _context.tblDeals.Where(d => d.DealID == dealId)
                   .Include(d => d.tblLawyer)
                   .Select(
                       o =>
                           new
                           {
                               o.tblLawyer.EMail,
                               o.tblLawyer.UserLanguage,
                               o.tblLawyer.UserID,
                               o.tblLawyer.Profession,
                               o.tblLawyer.FirstName,
                               o.tblLawyer.LastName,
                               o.tblLawyer.LawyerID
                           })
                   .AsEnumerable();
           return lawyerDetail.Select(o => new LawyerProfile()
           {
               Email = o.EMail,
               FirstName = o.FirstName,
               LastName = o.LastName,
               UserLanguage = o.UserLanguage,
               UserName = o.UserID,
               UserType = o.Profession,
               LawyerID = o.LawyerID
           }).SingleOrDefault();
       }
   }
}
