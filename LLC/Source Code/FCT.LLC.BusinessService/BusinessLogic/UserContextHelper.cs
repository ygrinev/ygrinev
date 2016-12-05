using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Security.Principal;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class UserContextHelper
    {
        public static void SetUserContext(string userid)
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userid), new string[] { "Auditlog" });
        }

        public static void SetUserContext( )
        {
            string userid =  WindowsIdentity.GetCurrent().Name ;

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userid), new string[] { "Auditlog" });
        }

    }
}
