using System.Web.Http;
using System.Web.Http.Results;
using LLCLite.Controllers;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Should;
using SpecsFor;
using FCT.LLC.Portal.DTOs.Requests;

namespace FCT.LLC.WebApi.Specs
{
    public class DealHistoryControllerSpecs:SpecsFor<DealHistoryController>
    {
        protected override void When()
        {
            base.When();
        }

        public class when_calling_With_RQA_Deal : SpecsFor<DealHistoryController>
        {
            private IHttpActionResult _result;
            protected override void When()
            {
                _result = SUT.GetDealHistories(new DealHistoryRequest() { DealID = 54513 });
            }

            [Test]
            public void then_it_returns_HttpActionResult()
            {
                _result.ShouldBeType<NegotiatedContentResult<JObject>>();
            }
        }
    }
}
