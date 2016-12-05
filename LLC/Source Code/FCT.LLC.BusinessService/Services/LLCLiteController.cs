using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.Results;
using Breeze.ContextProvider.EF6;
using FCT.LLC.BusinessService.BusinessLogic;
using FCT.LLC.BusinessService.Contracts.FaultContracts;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.Logging;
using FCT.LLC.Portal.DTOs.Dto;
using FCT.LLC.Portal.DTOs.Requests;
using FCT.LLC.Portal.DTOs.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GetDealDocumentsRequest = FCT.LLC.Portal.DTOs.Requests.GetDealDocumentsRequest;
using FCT.LLC.BusinessService.BusinessLogic.Interfaces;

namespace FCT.LLC.BusinessService
{
    public class LLCLiteController : ApiController
    {
        private readonly IDealBusinessLogic _dealBusinessLogic;
        private readonly ILogger _logger;
        private readonly IPifQuestionsBusinessLogic _pifQuestionsBusinessLogic;
        private readonly IDocumentBusinessLogic _documentBusinessLogic;
        public LLCLiteController(IDealBusinessLogic dealBusinessLogic, ILogger logger,
            IDocumentBusinessLogic documentBusinessLogic, IPifQuestionsBusinessLogic pifQuestionsBusinessLogic)
        {
            _dealBusinessLogic = dealBusinessLogic;
            _logger = logger;
            _documentBusinessLogic = documentBusinessLogic;
            _pifQuestionsBusinessLogic = pifQuestionsBusinessLogic;
        }

        [HttpGet]
        [Route("GetDealByDealId")]
        public IHttpActionResult GetDealByDealId([FromUri] GetFundingDealRequest getFundingDealRequest)
        {
            try
            {
                tblDeal foundDeal = _dealBusinessLogic.GetTbDeal(new Common.DataContracts.GetDealRequest()
                    {
                        DealID = getFundingDealRequest.DealID,
                        UserContext = getFundingDealRequest.UserContext
                    });
                string tbDealstring = JsonConvert.SerializeObject(foundDeal, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                return Content(HttpStatusCode.OK,
                    tbDealstring.Replace(Environment.NewLine, string.Empty).Replace(@"\", string.Empty),
                    Configuration.Formatters.JsonFormatter);

            }
            catch (Exception exception)
            {

                ServiceNotAvailableFault serviceNotAvailableFault;
                if (exception is DataAccessException)
                {
                    var daEx = exception as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault,
                        serviceNotAvailableFault.Message);
                }
                if (exception is InValidDealException)
                {
                    var invEx = exception as InValidDealException;
                    _logger.LogError(invEx);
                    var concurrencyFault = new ConcurrencyViolationFault()
                    {
                        Description = invEx.ExceptionMessage,
                        ViolationCode = invEx.ViolationCode
                    };
                    throw new FaultException<ConcurrencyViolationFault>(concurrencyFault, concurrencyFault.Description);
                }
                _logger.LogUnhandledError(exception);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = exception.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault,
                    serviceNotAvailableFault.Message);
            }
        }

        [HttpGet]
        [Route("GetDealByFCTURN")]
        public IHttpActionResult GetDealByFCTURN([FromUri] FCT.LLC.Portal.DTOs.Requests.GetDealRequest getDealRequest)
        {
            try
            {
                tblDeal foundDeal = _dealBusinessLogic.GetTbDealByFCTURN(getDealRequest.FCTURN);
                string tbDealstring = JsonConvert.SerializeObject(foundDeal, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                JToken token = JToken.Parse(tbDealstring);
                return Content(HttpStatusCode.OK, JObject.Parse(token.ToString()),
                    Configuration.Formatters.JsonFormatter);
            }
            catch (Exception exception)
            {

                ServiceNotAvailableFault serviceNotAvailableFault;
                if (exception is DataAccessException)
                {
                    var daEx = exception as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault,
                        serviceNotAvailableFault.Message);
                }
                if (exception is InValidDealException)
                {
                    var invEx = exception as InValidDealException;
                    _logger.LogError(invEx);
                    var concurrencyFault = new ConcurrencyViolationFault()
                    {
                        Description = invEx.ExceptionMessage,
                        ViolationCode = invEx.ViolationCode
                    };
                    throw new FaultException<ConcurrencyViolationFault>(concurrencyFault, concurrencyFault.Description);
                }
                _logger.LogUnhandledError(exception);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = exception.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault,
                    serviceNotAvailableFault.Message);
            }
        }

        [HttpGet]
        [Route("GetDeal")]
        public IHttpActionResult GetDeal([FromUri] Portal.DTOs.Requests.GetDealRequest getDealRequest)
        {
            try
            {
                tblDeal foundDeal = _dealBusinessLogic.GetTbDealByDealId(getDealRequest.DealID);
                string dealstring = JsonConvert.SerializeObject(foundDeal, Formatting.None, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                JToken token = JToken.Parse(dealstring);
                return Content(HttpStatusCode.OK, token, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception exception)
            {

                ServiceNotAvailableFault serviceNotAvailableFault;
                if (exception is DataAccessException)
                {
                    var daEx = exception as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault,
                        serviceNotAvailableFault.Message);
                }
                if (exception is InValidDealException)
                {
                    var invEx = exception as InValidDealException;
                    _logger.LogError(invEx);
                    var concurrencyFault = new ConcurrencyViolationFault()
                    {
                        Description = invEx.ExceptionMessage,
                        ViolationCode = invEx.ViolationCode
                    };
                    throw new FaultException<ConcurrencyViolationFault>(concurrencyFault, concurrencyFault.Description);
                }
                _logger.LogUnhandledError(exception);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = exception.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault,
                    serviceNotAvailableFault.Message);
            }
        }

        [HttpPost]
        [Route("GetPifQuestionsByDealId")]
        public IHttpActionResult GetPifQuestionsByDealId(FCT.LLC.Portal.DTOs.Requests.GetPifQuestionsRequest getPifQuestionsRequest)
        {
            try
            {
                var response = new Portal.DTOs.Responses.GetPifQuestionsResponse();
                response.Questions = 
                    _pifQuestionsBusinessLogic.GettblQuestionsByDealId(getPifQuestionsRequest.DealID,getPifQuestionsRequest.RecalculateQuestions);
                if (response.Questions.Any())
                {
                    string serializeObject = JsonConvert.SerializeObject(response,
                        Formatting.Indented, new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                    JToken token = JToken.Parse(serializeObject);
                    FormattedContentResult<JObject> result = Content(HttpStatusCode.OK, JObject.Parse(token.ToString()),
                        Configuration.Formatters.JsonFormatter);
                    return result;
                }      
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                HttpResponseMessage errorMessage =
                        new HttpResponseMessage() { Content = new StringContent(exception.Message) };
                return Content(HttpStatusCode.InternalServerError, errorMessage, Configuration.Formatters.JsonFormatter);               
            }

            return Content(HttpStatusCode.NotFound, "Not Found");
        }

        [HttpGet]
        [Route("GetDealHistories")]
        public IHttpActionResult GetDealHistories([FromUri]DealHistoryRequest dealHistoryRequest)
        {
            try
            {
                var dealHistories = _dealBusinessLogic.GetDealHistories(dealHistoryRequest.DealID).ToList();
                string dealHistoriesString = JsonConvert.SerializeObject(dealHistories, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                JToken token = JToken.Parse(dealHistoriesString);
                return Content(HttpStatusCode.OK, JArray.Parse(token.ToString()), Configuration.Formatters.JsonFormatter);
            }
            catch (Exception exception)
            {

                ServiceNotAvailableFault serviceNotAvailableFault;
                if (exception is DataAccessException)
                {
                    var daEx = exception as DataAccessException;
                    _logger.LogError(daEx);
                    serviceNotAvailableFault = new ServiceNotAvailableFault { Message = daEx.BaseException.Message };
                    throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
                }
                if (exception is InValidDealException)
                {
                    var invEx = exception as InValidDealException;
                    _logger.LogError(invEx);
                    var concurrencyFault = new ConcurrencyViolationFault()
                    {
                        Description = invEx.ExceptionMessage,
                        ViolationCode = invEx.ViolationCode
                    };
                    throw new FaultException<ConcurrencyViolationFault>(concurrencyFault, concurrencyFault.Description);
                }
                _logger.LogUnhandledError(exception);
                serviceNotAvailableFault = new ServiceNotAvailableFault { Message = exception.Message };
                throw new FaultException<ServiceNotAvailableFault>(serviceNotAvailableFault, serviceNotAvailableFault.Message);
            }
        }

        [HttpPost]
        [Route("SavePifAnswersRequest")]
        public IHttpActionResult SavePifAnswersRequest(FCT.LLC.Portal.DTOs.Requests.SavePifAnswersRequest savePifAnswersRequest)
        {
            try
            {
                bool savePifAnswersResponse = _pifQuestionsBusinessLogic.SavetblAnswers(savePifAnswersRequest.DealId, savePifAnswersRequest.Answers);
                if (savePifAnswersResponse)
                {
                    string result = savePifAnswersResponse.ToString();
                    return Content(HttpStatusCode.OK, result,
                        Configuration.Formatters.JsonFormatter);
                }
                HttpResponseMessage errorMessage =
                    new HttpResponseMessage() { Content = new StringContent("Can't save Pif answers") };
                return Content(HttpStatusCode.InternalServerError, errorMessage, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message); 
                HttpResponseMessage errorMessage =
                        new HttpResponseMessage() { Content = new StringContent(exception.Message) };
                return Content(HttpStatusCode.InternalServerError, errorMessage, Configuration.Formatters.JsonFormatter);  
            }
        }

        [HttpPost]
        [Route("GetDealDocuments")]
        public IHttpActionResult GetDealDocuments(GetDealDocumentsRequest getDealDocumentsRequest)  // GetDealDocumentsRequest getDealDocumentsRequest
        {
            //string FCTURN = "11221002499";
            try
            {
                tblDeal deal = _dealBusinessLogic.GetTbDealByFCTURN(getDealDocumentsRequest.FCTURN); // getDealDocumentsRequest.
                if (deal == null)
                    throw new Exception("Invalid FCTUrn = " + getDealDocumentsRequest.FCTURN);// getDealDocumentsRequest.

                DealDocuments _dealDocuments = _documentBusinessLogic.GetDealDocs(deal, getDealDocumentsRequest.langID);
                GetDealDocumentsResponse response = new GetDealDocumentsResponse { dealDocuments = _dealDocuments }; // deal.DealID, getDealDocumentsRequest.langID

                string docsString = JsonConvert.SerializeObject(response,
                    Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                JToken token = JToken.Parse(docsString);
                FormattedContentResult<JObject> result = Content(HttpStatusCode.OK, JObject.Parse(token.ToString()),
                    Configuration.Formatters.JsonFormatter);
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                HttpResponseMessage errorMessage =
                        new HttpResponseMessage() { Content = new StringContent(exception.Message) };
                return Content(HttpStatusCode.NotFound, errorMessage, Configuration.Formatters.JsonFormatter);
            }

        }

        [HttpGet]
        [Route("Metadata")]
        public string Metadata()
        {
            return new EFContextProvider<EFBusinessContext>().Metadata();
        }
    }
}