using System.Web.Http;
using System.Web.Http.Results;
using LLCLite.Controllers;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Should;
using SpecsFor;
using FCT.LLC.Portal.DTOs.Requests;
using FCT.LLC.Portal.DTOs.Responses;
using System.Net;
using System.Threading;

namespace FCT.LLC.WebApi.Specs
{
    public class DocumentControllerSpecs : SpecsFor<DocumentController>
    {
        protected override void When()
        {
            base.When();
        }

        public class when_calling_With_SIT_Deal : SpecsFor<DocumentController>
        {
            private IHttpActionResult _result;
            protected override void When()
            {
                _result = SUT.GetDealDocuments(new GetDealDocumentsRequest() { FCTURN = "11221002499", langID = 0 });
            }

            [Test]
            public void then_it_returns_Documents()
            {
                _result.ExecuteAsync(new CancellationToken()).Result.StatusCode.ShouldEqual(HttpStatusCode.OK);
            }
        }
    }
}
