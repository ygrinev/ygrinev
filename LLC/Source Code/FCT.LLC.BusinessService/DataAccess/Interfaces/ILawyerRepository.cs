using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public interface ILawyerRepository:IRepository<tblLawyer>
    {
        LawyerProfile GetNotificationDetails(int lawyerId);

        LawyerProfile GetUserDetails(int lawyerId);

        LawyerProfile GetLawyerDetails(int dealId);

        LawyerProfile GetUserDetails(string userName);

        IEnumerable<LawyerProfile> GetNotificationDetails(IEnumerable<int> lawyerIds);
    }
}
