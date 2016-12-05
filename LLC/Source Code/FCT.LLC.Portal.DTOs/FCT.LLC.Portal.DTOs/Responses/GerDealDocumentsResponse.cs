using FCT.LLC.Portal.DTOs.Dto;
using FCT.LLC.Portal.DTOs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.Portal.DTOs.Responses
{
    public class GetDealDocumentsResponse : IResponse
    {
        public DealDocuments dealDocuments { get; set; }
    }
}
