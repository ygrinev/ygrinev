using System;
using System.Web.Http;
using FCT.LLC.BusinessService.Filters.Exceptions;

namespace FCT.LLC.BusinessService.App_Data
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new NotImplExceptionFilterAttribute());

            // Other configuration code...
        }
    }
}