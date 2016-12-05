using FCT.LLC.Portal.DTOs.Interfaces;

namespace FCT.LLC.Portal.DTOs.Requests
{
    public class GetDealRequest : IRequest
    {
        public string FCTURN { get; set; }
        public int DealID { get; set; }
    }
}