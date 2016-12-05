using System;
using Breeze.WebApi2;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Results;
using FCT.LLC.Portal.DTOs.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LLCLite.Controllers
{
    [BreezeController]
    public class DealController : BaseController
    {
        [HttpGet]
        public IHttpActionResult GetDealByFCTURN([FromUri]GetDealRequest getDealRequest)
        {
            IHttpActionResult returnValue;
            try
            {
                HttpClientHandler hndlr = HttpClientHandlerBuilder();
                using (HttpClient httpClient = HttpClientBuilder(hndlr))
                {
                    var response = httpClient.GetAsync(
                        $"{LLCBusinessServerUrl}{LLCBusinessServiceWebAPIURL}/GetDealByFCTURN?FCTURN={getDealRequest.FCTURN}").Result;
                    response.EnsureSuccessStatusCode();
                    returnValue = ResponseMessage(response);
                }
            }
            catch (Exception e)
            {
                returnValue =
                   ResponseMessage(new HttpResponseMessage() { Content = new StringContent(e.Message), StatusCode = HttpStatusCode.InternalServerError });
            }
            return returnValue;
        }

        [HttpGet]
        public IHttpActionResult GetDealByDealID([FromUri]GetDealRequest getDealRequest)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                HttpClientHandler hndlr = new HttpClientHandler();
                hndlr.UseDefaultCredentials = true;
                using (var client = new HttpClient(hndlr))
                {
                    client.BaseAddress = new Uri(LLCBusinessServerUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    response = client.GetAsync(string.Format("{0}{1}/GetDeal?DealID={2}", LLCBusinessServerUrl, LLCBusinessServiceWebAPIURL, getDealRequest.DealID)).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        response.EnsureSuccessStatusCode();
                        var dealHistoryJson = response.Content.ReadAsStringAsync().Result;
                        JToken token = JToken.Parse(dealHistoryJson);
                        return Content(HttpStatusCode.OK, JObject.Parse(token.ToString()));
                    }
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e.ToString());
            }
        }

        [HttpPost]
        public IHttpActionResult GetPifQuestionsByDealId(GetPifQuestionsRequest getPifQuestionsRequest)
        {
            IHttpActionResult returnValue;
            try
            {
                HttpClientHandler hndlr = HttpClientHandlerBuilder();
                ByteArrayContent byteArrayContent = ByteArrayContentBuilder(getPifQuestionsRequest);
                using (HttpClient httpClient = HttpClientBuilder(hndlr))
                {
                    var result = httpClient.PostAsync($"{LLCBusinessServerUrl}{LLCBusinessServiceWebAPIURL}/GetPifQuestionsByDealId/{getPifQuestionsRequest.DealID}"
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

        [HttpPost]
        public IHttpActionResult SavePifAnswersRequest(SavePifAnswersRequest savePifAnswersRequest)
        {
            IHttpActionResult returnValue;
            try
            {
                HttpClientHandler hndlr = HttpClientHandlerBuilder();
                ByteArrayContent byteArrayContent = ByteArrayContentBuilder(savePifAnswersRequest);
                using (HttpClient httpClient = HttpClientBuilder(hndlr))
                {
                    var result = httpClient.PostAsync($"{LLCBusinessServerUrl}{LLCBusinessServiceWebAPIURL}/SavePifAnswersRequest/{savePifAnswersRequest}"
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

        [HttpGet]
        public IHttpActionResult HelloWorld()
        {
            IHttpActionResult returnValue;
            try
            {
                returnValue = ResponseMessage(new HttpResponseMessage() { Content = new StringContent("Hello, World"), StatusCode = HttpStatusCode.OK });
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