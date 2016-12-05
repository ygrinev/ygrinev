using System.Web.Http;
using LLCLite.Filters.Exceptions;

namespace LLCLite
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new NotImplExceptionFilterAttribute());

        }
    }
}