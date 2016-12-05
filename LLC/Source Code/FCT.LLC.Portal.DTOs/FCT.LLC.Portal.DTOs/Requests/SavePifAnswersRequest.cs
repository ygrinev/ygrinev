using System.Collections.Generic;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Portal.DTOs.Dto;
using FCT.LLC.Portal.DTOs.Interfaces;

namespace FCT.LLC.Portal.DTOs.Requests
{
    public class SavePifAnswersRequest:IRequest
    {
        public PifAnswerCollection Answers { get; set; }
        public int DealId { get; set; }
        public UserContext UserContext { get; set; }
    }
}