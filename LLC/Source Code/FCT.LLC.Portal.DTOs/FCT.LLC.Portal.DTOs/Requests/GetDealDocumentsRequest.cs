using FCT.LLC.Portal.DTOs.Interfaces;

namespace FCT.LLC.Portal.DTOs.Requests
{
    public class GetDealDocumentsRequest : IRequest
    {
        public string FCTURN { get; set; }
        public int langID { get; set; }
    }
}
