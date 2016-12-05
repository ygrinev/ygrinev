using System.Collections.Generic;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Portal.DTOs.Interfaces;

namespace FCT.LLC.Portal.DTOs.Responses
{
    public class GetPifQuestionsResponse : IResponse
    {
        public List<tblQuestion> Questions { get; set; }
    }
}
