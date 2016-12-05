using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess.Implementations
{
   public sealed class PaymentRequestRepository:Repository<tblPaymentRequest>, IPaymentRequestRepository
   {
       private readonly IEntityMapper<tblPaymentRequest, LLCPaymentRequest> _requestMapper;
       private readonly EFBusinessContext _context;
       public PaymentRequestRepository(EFBusinessContext context, IEntityMapper<tblPaymentRequest, LLCPaymentRequest> requestMapper) : base(context)
       {
           _requestMapper = requestMapper;
           _context = context;
       }

       public IEnumerable<LLCPaymentRequest> InsertPaymentRequestRange(IEnumerable<LLCPaymentRequest> paymentRequests)
       {
           var entities = paymentRequests.Select(p => _requestMapper.MapToEntity(p)).ToList();
           InsertRange(entities);
           _context.SaveChanges();
           return entities.Select(e => _requestMapper.MapToData(e));
       }
   }
}
