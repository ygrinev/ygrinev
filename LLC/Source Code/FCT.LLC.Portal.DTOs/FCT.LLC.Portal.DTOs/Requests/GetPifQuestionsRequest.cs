using FCT.LLC.Portal.DTOs.Dto;
using FCT.LLC.Portal.DTOs.Interfaces;

namespace FCT.LLC.Portal.DTOs.Requests
{
    public class GetPifQuestionsRequest:IRequest
    {
        public GetPifQuestionsRequest(int dealId, bool recalculateQuestions)
        {
            DealID = dealId;
            RecalculateQuestions = recalculateQuestions;
        }

        public int DealID { get; set; }
        public bool RecalculateQuestions { get; set; }
        public UserContext UserContext { get; set; }
    }
}