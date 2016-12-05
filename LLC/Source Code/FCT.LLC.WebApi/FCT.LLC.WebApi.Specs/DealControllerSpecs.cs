using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using FCT.LLC.Portal.DTOs.Dto;
using LLCLite.Controllers;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Should;
using SpecsFor;
using FCT.LLC.Portal.DTOs.Requests;

namespace FCT.LLC.WebApi.Specs
{
    public class DealControllerSpecs:SpecsFor<DealController>
    {
        protected override void When()
        {
            base.When();
        }

        public class when_calling_With_RQA_Deal:SpecsFor<DealController>
        {
            private IHttpActionResult _result;
            protected override void When()
            {
                _result = SUT.GetDealByFCTURN(new GetDealRequest() {FCTURN = "16323037233"});
            }

            [Test]
            public void then_it_should_not_be_null()
            {
                _result.ShouldNotBeNull();
            }
        }

        public class when_calling_With_RQA_Deal_By_DealID : SpecsFor<DealController>
        {
            private IHttpActionResult _result;
            protected override void When()
            {
                _result = SUT.GetDealByDealID(new GetDealRequest() { DealID = 54536 });
            }

            [Test]
            public void then_it_should_not_be_null()
            {
                _result.ShouldNotBeNull();
            }
        }

        public class When_Searching_Questions_With_dealId : SpecsFor<DealController>
        {
            private IHttpActionResult _result;
            protected override void When()
            {
                _result = SUT.GetPifQuestionsByDealId(new GetPifQuestionsRequest(32589, true) {UserContext = new UserContext() {UserType = "Lawyer", UserID = "lawyer11"} });
            }

            [Test]
            public void then_it_should_not_be_null()
            {
                _result.ShouldNotBeNull();
                
            }
        }
    }
}
