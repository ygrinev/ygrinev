using FCT.LLC.Portal.DTOs.Interfaces;

namespace FCT.LLC.Portal.DTOs.Requests
{
    public class DealHistoryRequest : IRequest
    {
        public int DealID { get; set; }
        public int DealHistoryID { get; set; }
    }
}