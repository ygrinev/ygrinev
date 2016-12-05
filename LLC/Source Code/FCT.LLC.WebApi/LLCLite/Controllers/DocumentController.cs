using Breeze.WebApi2;
using FCT.LLC.Portal.DTOs.Requests;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace LLCLite.Controllers
{
    [BreezeController]
    public class DocumentController : BaseController
    {
        [HttpPost]
        public IHttpActionResult GetDealDocuments(GetDealDocumentsRequest getDealDocumentsRequest)
        {
            IHttpActionResult returnValue;
            try
            {
                HttpClientHandler hndlr = HttpClientHandlerBuilder();
                ByteArrayContent byteArrayContent = ByteArrayContentBuilder(getDealDocumentsRequest);
                using (HttpClient httpClient = HttpClientBuilder(hndlr))
                {
                    var result = httpClient.PostAsync($"{LLCBusinessServerUrl}{LLCBusinessServiceWebAPIURL}/GetDealDocuments/{getDealDocumentsRequest}"
                        , byteArrayContent).Result;
                    result.EnsureSuccessStatusCode();
                    returnValue = ResponseMessage(result);
                }
            }
            catch (Exception e)
            {
                returnValue =
                    ResponseMessage(new HttpResponseMessage() { Content = new StringContent(e.Message), StatusCode = HttpStatusCode.InternalServerError });
            }
            return returnValue;
        }
    }
}
