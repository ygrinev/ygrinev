using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Results;
using FCT.LLC.Portal.DTOs.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LLCLite.Controllers
{
    public class BaseController : ApiController
    {
        #region Constants

        protected const string JsonResponseType = "application/json";
        #endregion

        #region Protected Methods
        protected string LLCBusinessServerUrl
        {
            get
            {
                string businessServiceUrl = (string)ConfigurationManager.AppSettings["LLCBusinessServiceURL"];
                return businessServiceUrl;
            }
        }

        protected string LLCBusinessServiceWebAPIURL
        {
            get
            {
                string businessServiceUrl = (string)ConfigurationManager.AppSettings["LLCBusinessServiceWebAPIURL"];
                return businessServiceUrl;
            }
        }

        protected HttpClient HttpClientBuilder(HttpClientHandler httpClientHandler)
        {
            HttpClient httpClient = new HttpClient(httpClientHandler) {BaseAddress = new Uri(LLCBusinessServerUrl)};
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        protected HttpClientHandler HttpClientHandlerBuilder()
        {
            HttpClientHandler hndlr = new HttpClientHandler();
            hndlr.UseDefaultCredentials = true;
            return hndlr;
        }

        public JObject ConvertToJsonResponse(HttpResponseMessage httpResponseMessage)
        {            
            var dealJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
            JToken token = JToken.Parse(dealJson);
            return JObject.Parse(token.ToString());
        }

        public ByteArrayContent ByteArrayContentBuilder(IRequest request)
        {
            string contentInJson = JsonConvert.SerializeObject(request);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(contentInJson);
            ByteArrayContent byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue(JsonResponseType);
            return byteContent;
        }

        #endregion
    }
}