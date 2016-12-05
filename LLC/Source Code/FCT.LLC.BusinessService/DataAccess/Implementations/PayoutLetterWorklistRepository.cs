using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Implementations;
using FCT.LLC.GenericRepository;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class PayoutLetterWorklistRepository : Repository<vw_PayoutLetterWorklist>, IPayoutLetterWorklistRepository
    {
        private readonly EFBusinessContext _context;
        public PayoutLetterWorklistRepository(EFBusinessContext context)
            : base(context)
        {
            _context = context;
        }
        public List<vw_PayoutLetterWorklist> GetPayoutLetterWorkList(string ChequeBatchNumber, PayoutLetterWorklistOrderBySpecificationCollection orderBySpec, int pageIndex, int pageSize, UserContext usercontext, out int totalRowCount)
        {
            Expression<Func<vw_PayoutLetterWorklist, bool>> filterPredicate = ComposeFilterPredicate(ChequeBatchNumber);

            var orderByPredicate = ComposeOrderByPredicate(orderBySpec);
            

            GenericRepository.Contracts.IQueryFluent<vw_PayoutLetterWorklist> query = Query(filterPredicate);
            
            query = query.Tracking(false);
            query = query.OrderBy(orderByPredicate);
            var deals = query.SelectPage(pageIndex, pageSize, out totalRowCount);

            return deals.ToList();
        }

        private Func<IQueryable<vw_PayoutLetterWorklist>, IOrderedQueryable<vw_PayoutLetterWorklist>> ComposeOrderByPredicate(PayoutLetterWorklistOrderBySpecificationCollection orderBySpecifications)
        {
            Func<IQueryable<vw_PayoutLetterWorklist>, IOrderedQueryable<vw_PayoutLetterWorklist>> orderByPredicate;
            PayoutLetterWorklistOrderBySpecification orderBySpecification = new PayoutLetterWorklistOrderBySpecification();
            
            if (orderBySpecifications != null && orderBySpecifications.Count > 0)
            {
                orderBySpecification = orderBySpecifications.First();
            }
            else
            {
                return orderByPredicate =
                        q => q.OrderByDescending(deal => deal.DealID);
            }

            //Handle OrderBy based on field provided
            switch (orderBySpecification.OrderByColumn)
            {
                case PayoutLetterWorklistOrderByColumn.AssignedTo:
                    if (orderBySpecification.OrderByDirection == OrderByDirection.ASC)
                    {
                        orderByPredicate =
                            q => q.OrderBy(deal => deal.AssignedTo)
                                  .ThenBy(deal => deal.DealID);
                    }
                    else
                    {
                        orderByPredicate =
                            q => q.OrderByDescending(deal => deal.AssignedTo)
                                  .ThenByDescending(deal => deal.DealID);
                    }
                    break;
                case PayoutLetterWorklistOrderByColumn.BatchDescription:
                    //currently does sort for Mortgagor only
                    //Need changes to include sorting with Vendors as well
                    if (orderBySpecification.OrderByDirection == OrderByDirection.ASC)
                    {
                        orderByPredicate =
                            q => q.OrderBy(deal => deal.ChequeBatchDescription)
                            .ThenBy(deal => deal.DealID);
                    }
                    else
                    {
                        orderByPredicate =
                            q => q.OrderByDescending(deal => deal.ChequeBatchDescription)
                            .ThenByDescending(deal => deal.DealID);
                    }
                    break;

                case PayoutLetterWorklistOrderByColumn.BatchNumber:
                    if (orderBySpecification.OrderByDirection == OrderByDirection.ASC)
                    {
                        orderByPredicate =
                            q => q.OrderBy(deal => deal.ChequeBatchNumber)
                                  .ThenBy(deal => deal.DealID);
                    }
                    else
                    {
                        orderByPredicate =
                            q => q.OrderByDescending(deal => deal.ChequeBatchNumber)
                                  .ThenByDescending(deal => deal.DealID);
                    }
                    break;

                case PayoutLetterWorklistOrderByColumn.DisbursementDate:
                    if (orderBySpecification.OrderByDirection == OrderByDirection.ASC)
                    {
                        orderByPredicate =
                            q => q.OrderBy(deal => deal.DisbursementDate)
                                    .ThenBy(deal => deal.DealID);
                    }
                    else
                    {
                        orderByPredicate =
                            q => q.OrderByDescending(deal => deal.DisbursementDate)
                                    .ThenByDescending(deal => deal.DealID);
                    }
                    break;

                case PayoutLetterWorklistOrderByColumn.FCTURN:
                    //Sort should include combined ShortFCTURN and LongFCTURN
                    if (orderBySpecification.OrderByDirection == OrderByDirection.ASC)
                    {
                        //combine the order based on searchCriteria
                        orderByPredicate =
                            q => q.OrderBy(deal => deal.FCTURN)
                                    .ThenBy(deal => deal.DealID);
                    }
                    else
                    {
                        orderByPredicate =
                            q => q.OrderByDescending(deal => deal.FCTURN)
                                //  .ThenByDescending(deal => deal.tblDealScope.ShortFCTRefNumber)
                                    .ThenByDescending(deal => deal.DealID);
                    }
                    break;

                case PayoutLetterWorklistOrderByColumn.NumberOfCheques:
                    if (orderBySpecification.OrderByDirection == OrderByDirection.ASC)
                    {
                        orderByPredicate =
                            q => q.OrderBy(deal => deal.NumberOfCheques)
                                    .ThenBy(deal => deal.DealID);
                    }
                    else
                    {
                        orderByPredicate =
                            q => q.OrderByDescending(deal => deal.NumberOfCheques)
                                    .ThenByDescending(deal => deal.DealID);
                    }
                    break;

                default:
                    orderByPredicate =
                        q => q.OrderByDescending(deal => deal.DealID);
                    break;
            }

            return orderByPredicate;
        }


        private Func<IQueryable<vw_PayoutLetterWorklist>, IOrderedQueryable<vw_PayoutLetterWorklist>> ComposeOrderByPredicate()
        {
            Func<IQueryable<vw_PayoutLetterWorklist>, IOrderedQueryable<vw_PayoutLetterWorklist>> orderByPredicate; 
            orderByPredicate = q => q.OrderBy(deal => deal.DealID);
            return orderByPredicate;
        }

        private Expression<Func<vw_PayoutLetterWorklist, bool>> ComposeFilterPredicate(string ChequeBatchNumber)
        {
            var filterPredicate = PredicateBuilder.True<vw_Deal>();
            Expression<Func<vw_PayoutLetterWorklist, bool>> searchPredicate = null;

            if (!String.IsNullOrEmpty(ChequeBatchNumber))
            {
                //LenderReferenceNumber
                searchPredicate = deal => deal.ChequeBatchNumber.StartsWith(ChequeBatchNumber);
            }

            return searchPredicate;
        }
    }
}
