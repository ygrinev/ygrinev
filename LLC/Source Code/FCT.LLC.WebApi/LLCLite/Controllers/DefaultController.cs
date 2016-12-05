using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Breeze.WebApi2;
using Breeze.ContextProvider;
using Newtonsoft.Json.Linq;

namespace LLCLite.Controllers
{
    [BreezeController]
    public class DefaultController : BaseController
    {
        #region Private Properties

        private static string _metadata;

        #endregion

        #region Controller Actions
        // ~/breeze/Metadata
        [HttpGet]
        public IHttpActionResult Metadata()
        {
            IHttpActionResult result = NotFound();
            if (string.IsNullOrWhiteSpace(_metadata))
            {
                var response = new HttpResponseMessage();
                try
                {
                    using (var httpHandler = new HttpClientHandler())
                    {
                        httpHandler.UseDefaultCredentials = true;
                        using (var client = new HttpClient(httpHandler))
                        {
                            client.BaseAddress = new Uri(LLCBusinessServerUrl);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue(JsonResponseType));

                            response =
                                client.GetAsync($"{LLCBusinessServerUrl}{LLCBusinessServiceWebAPIURL}/Metadata").Result;
                            response.EnsureSuccessStatusCode();

                            var dealJson = response.Content.ReadAsAsync<string>().Result;
                            var jsonToken = JToken.Parse(dealJson);
                            _metadata = jsonToken.ToString();
                            result = Content(HttpStatusCode.OK, JObject.Parse(jsonToken.ToString()));
                        }
                    }
                }
                catch (Exception e)
                {
                    result = Content(HttpStatusCode.BadRequest, e.ToString());
                }
            }
            else
            {
                result = Content(HttpStatusCode.OK, JObject.Parse(_metadata));
            }

            return result;
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return new SaveResult();
            //return _unitOfWork.Commit(saveBundle);
        }

        // ~/breeze/Lookups
        [HttpGet]
        public LookupBundle Lookups()
        {
            return new LookupBundle();
        }
        #endregion
    }
}