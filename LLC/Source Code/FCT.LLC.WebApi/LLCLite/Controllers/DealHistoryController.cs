using System;
using Breeze.WebApi2;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using FCT.LLC.Portal.DTOs.Requests;
using Newtonsoft.Json.Linq;

namespace LLCLite.Controllers
{
    public class DealHistoryController : BaseController
    {
        [HttpGet]
        public IHttpActionResult GetDealHistories([FromUri]DealHistoryRequest dealHistoryRequest)
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

                    response = client.GetAsync(string.Format("{0}{1}/GetDealHistories?DealID={2}", LLCBusinessServerUrl, LLCBusinessServiceWebAPIURL, dealHistoryRequest.DealID)).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        response.EnsureSuccessStatusCode();
                        var dealHistoryJson = response.Content.ReadAsStringAsync().Result;
                        JToken token = JToken.Parse(dealHistoryJson);
                        return Content(HttpStatusCode.OK, JArray.Parse(token.ToString()));
                    }
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e.ToString());
            }
        }

        [HttpGet]
        public IHttpActionResult GetDealHistory([FromUri]DealHistoryRequest dealHistoryRequest)
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

                    response = client.GetAsync(string.Format("{0}{1}/GetDealHistories?DealHistoryID={2}", LLCBusinessServerUrl, LLCBusinessServiceWebAPIURL, dealHistoryRequest.DealHistoryID)).Result;
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
    }
}
