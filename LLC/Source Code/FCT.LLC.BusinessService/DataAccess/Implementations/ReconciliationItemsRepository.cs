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

namespace FCT.LLC.BusinessService.DataAccess.Implementations
{
    public class ReconciliationItemsRepository : Repository<vw_ReconciliationItem>, IReconciliationItemsRepository
    {
        private readonly EFBusinessContext _context;
        public ReconciliationItemsRepository(EFBusinessContext context)
            : base(context)
        {
            _context = context;
        }

        public GetReconciliationWorklistResponse GetReconciliationItems(GetReconciliationWorklistRequest request)
        {
            Expression<Func<vw_ReconciliationItem, bool>> filter = getSelectionFilter(request.SearchCriteria);
            Func<IQueryable<vw_ReconciliationItem>, IOrderedQueryable<vw_ReconciliationItem>> sorting = getSorting(request.OrderBySpecifications);

            GenericRepository.Contracts.IQueryFluent<vw_ReconciliationItem> query = null;
            if (filter != null)
                query = this.Query(filter);
            else
                query = this.Query();

            query = query.Tracking(false);
            query = query.OrderBy(sorting);
            int totalRowCount = 0;
            //temporary solution
            var itemsTotal = query.Select();
            List<vw_ReconciliationItem> listItemsTotal = itemsTotal.ToList();
            var totalin = listItemsTotal.Sum(x => x.AmountIn);
            var totalout = listItemsTotal.Sum(x => x.AmountOut);
            //end temp solution

            var items = query.SelectPage(request.PageIndex,request.PageSize, out totalRowCount);
            List<vw_ReconciliationItem> listItems = items.ToList();
            ReconciliationCollection collection = getCollection(listItems);
            GetReconciliationWorklistResponse response = new GetReconciliationWorklistResponse();
            response.SearchResults = collection;
            response.TotalRowsCount = totalRowCount;
            response.PageIndex = request.PageIndex;
            //totalamountin and totalamountout
            if(totalin.HasValue)
                 response.TotalAmountIn = totalin.Value;
            if(totalout.HasValue)
                 response.TotalAmountOut = totalout.Value;

            return response;
        }
        private ReconciliationCollection getCollection(IEnumerable<vw_ReconciliationItem> items)
        {
                ReconciliationCollection itemsCollection = new ReconciliationCollection();

                foreach (vw_ReconciliationItem vItem in items)
                {
                    itemsCollection.Add(getItem(vItem));
                }

                return itemsCollection;
        }
        private ReconciliationItem getItem(vw_ReconciliationItem vItem)
        {
            ReconciliationItem item = new ReconciliationItem();
            item.ItemID = vItem.ItemID;
            item.ItemType = vItem.ItemType;
            item.FCTURN = vItem.FCTURN;
            item.ReferenceNumber = vItem.ReferenceNumber;
            item.TransactionDate = vItem.TransactionDate.ToShortDateString();
            item.TransactionType = vItem.TransactionType;
            item.AmountIn = vItem.AmountIn;
            item.AmountOut = vItem.AmountOut;
            item.BatchNumber = vItem.BatchNumber;
            return item;
        }
        private Func<IQueryable<vw_ReconciliationItem>, IOrderedQueryable<vw_ReconciliationItem>> getSorting(ReconciliationWorklistOrderBySpecificationCollection sortings)
        {
             ReconciliationWorklistOrderBySpecification spec = sortings.First();
             Func<IQueryable<vw_ReconciliationItem>, IOrderedQueryable<vw_ReconciliationItem>> orderByPredicate = null;


             switch (spec.OrderByColumn)
             {
                 case ReconciliationWorklistOrderByColumn.BatchNumber:
                     if (spec.OrderByDirection == OrderByDirection.ASC)
                         orderByPredicate = p => p.OrderBy(item => item.BatchNumber).ThenBy(item => item.ItemID);
                     else
                         orderByPredicate = p => p.OrderByDescending(item => item.BatchNumber).ThenBy(item => item.ItemID);
                     break;
                 case ReconciliationWorklistOrderByColumn.FCTURN:
                     if (spec.OrderByDirection == OrderByDirection.ASC)
                         orderByPredicate = p => p.OrderBy(item => item.FCTURN).ThenBy(item => item.ItemID);
                     else
                         orderByPredicate = p => p.OrderByDescending(item => item.FCTURN).ThenBy(item => item.ItemID);
                     break;
                 case ReconciliationWorklistOrderByColumn.TransactionDate:
                     if (spec.OrderByDirection == OrderByDirection.ASC)
                         orderByPredicate = p => p.OrderBy(item => item.TransactionDate).ThenBy(item => item.ItemID);
                     else
                         orderByPredicate = p => p.OrderByDescending(item => item.TransactionDate).ThenBy(item => item.ItemID);
                     break;
                 case ReconciliationWorklistOrderByColumn.TransactionType:
                     if (spec.OrderByDirection == OrderByDirection.ASC)
                         orderByPredicate = p => p.OrderBy(item => item.TransactionType).ThenBy(item => item.ItemID);
                     else
                         orderByPredicate = p => p.OrderByDescending(item => item.TransactionType).ThenBy(item => item.ItemID);
                     break;
                 case ReconciliationWorklistOrderByColumn.ReferenceNumber:
                     if (spec.OrderByDirection == OrderByDirection.ASC)
                         orderByPredicate = p => p.OrderBy(item => item.ReferenceNumber).ThenBy(item => item.ItemID);
                     else
                         orderByPredicate = p => p.OrderByDescending(item => item.ReferenceNumber).ThenBy(item => item.ItemID);
                     break;
                 case ReconciliationWorklistOrderByColumn.AmountIn:
                     if (spec.OrderByDirection == OrderByDirection.ASC)
                         orderByPredicate = p => p.OrderBy(item => item.AmountIn).ThenBy(item => item.ItemID);
                     else
                         orderByPredicate = p => p.OrderByDescending(item => item.AmountIn).ThenBy(item => item.ItemID);
                     break;
                 case ReconciliationWorklistOrderByColumn.AmountOut:
                     if (spec.OrderByDirection == OrderByDirection.ASC)
                         orderByPredicate = p => p.OrderBy(item => item.AmountOut).ThenBy(item => item.ItemID);
                     else
                         orderByPredicate = p => p.OrderByDescending(item => item.AmountOut).ThenBy(item => item.ItemID);
                     break;
             }

             return orderByPredicate;
        }
        private Expression<Func<vw_ReconciliationItem, bool>> getSelectionFilter(ReconciliationWorklistSearchCriteria criteria)
        {
            if (criteria == null)
            {
                var filterPredicate = PredicateBuilder.True<vw_ReconciliationItem>();
                return filterPredicate;
            }

            List<Expression<Func<vw_ReconciliationItem, bool>>> list = new List<Expression<Func<vw_ReconciliationItem, bool>>>();

            Expression<Func<vw_ReconciliationItem, bool>> searchPredicate = null;

            if (!string.IsNullOrEmpty(criteria.BatchNumber))
            {
                searchPredicate = basedOnBatchNumber(criteria);
            }
            
            if (searchPredicate != null)
            {
                list.Add(searchPredicate);
            }

            if (!string.IsNullOrEmpty(criteria.ReferenceNumber))
            {
                searchPredicate = basedOnReferenceNumber(criteria);
            }
            
            if (searchPredicate != null)
            {
                list.Add(searchPredicate);
            }

            if (!string.IsNullOrEmpty(criteria.TransactionType))
            {
                searchPredicate = basedOnTransactionType(criteria);
            }
            
            if (searchPredicate != null)
            {
                list.Add(searchPredicate);
            }

            if (!string.IsNullOrEmpty(criteria.FCTURN))
            {
                searchPredicate = basedOnFCTURN(criteria);
            }
            
            if (searchPredicate != null)
            {
                list.Add(searchPredicate);
            }
            searchPredicate = basedOnDate(criteria);
            if (searchPredicate != null)
                list.Add(searchPredicate);

            searchPredicate = basedOnAmount(criteria);
            if (searchPredicate != null)
            {
                list.Add(searchPredicate);
            }

            searchPredicate = null;
            
            foreach (Expression<Func<vw_ReconciliationItem, bool>> search in list)
            {
                if (searchPredicate == null)
                    searchPredicate = search;
                else
                {
                    searchPredicate = PredicateBuilder.And<vw_ReconciliationItem>(searchPredicate, search);
                }
            }
            return searchPredicate;
        }
        private Expression<Func<vw_ReconciliationItem, bool>> basedOnAmount(ReconciliationWorklistSearchCriteria criteria)
        {
            if (!criteria.AmountFrom.HasValue && !criteria.AmountTo.HasValue)
                return null;

            Expression<Func<vw_ReconciliationItem, bool>> predicate = null;
            if (criteria.AmountFrom.HasValue && criteria.AmountTo.HasValue)
            {
                predicate = item => ( (item.AmountIn.HasValue 
                                      && item.AmountIn>=criteria.AmountFrom
                                      && item.AmountIn<=criteria.AmountTo )
                                      || (item.AmountOut.HasValue 
                                      && item.AmountOut>=criteria.AmountFrom
                                      && item.AmountOut<=criteria.AmountTo)
                                     );
            }
            else
            {
                if (criteria.AmountFrom.HasValue)
                {
                    predicate = item => ((item.AmountIn.HasValue
                                          && item.AmountIn >= criteria.AmountFrom)
                                          || (item.AmountOut.HasValue
                                          && item.AmountOut >= criteria.AmountFrom)
                                         );
                }
                else
                {
                    predicate = item => ((item.AmountIn.HasValue
                                          && item.AmountIn <= criteria.AmountTo)
                                          || (item.AmountOut.HasValue
                                          && item.AmountOut <= criteria.AmountTo)
                                         );
                }
            }
            return predicate;
        }
        private Expression<Func<vw_ReconciliationItem, bool>> basedOnDate(ReconciliationWorklistSearchCriteria criteria)
        {
            if (string.IsNullOrWhiteSpace(criteria.DateFrom) && string.IsNullOrWhiteSpace(criteria.DateTo))
                return null;
            DateTime DateFrom = DateTime.MinValue;
            DateTime DateTo = DateTime.MinValue;
            
            DateTime modDateFrom = DateTime.MinValue;
            DateTime modDateTo = DateTime.MinValue;

            if(!string.IsNullOrWhiteSpace(criteria.DateFrom))
                DateFrom = DateTime.Parse(criteria.DateFrom);
            if (!string.IsNullOrWhiteSpace(criteria.DateTo))
                DateTo = DateTime.Parse(criteria.DateTo);
            
            modDateFrom = new DateTime(DateFrom.Year, DateFrom.Month, DateFrom.Day, 0, 0, 0);
            modDateTo = new DateTime(DateTo.Year, DateTo.Month, DateTo.Day, 23, 59, 59);

            Expression<Func<vw_ReconciliationItem, bool>> predicate = null;
            if (DateFrom != DateTime.MinValue && DateTo != DateTime.MinValue)
            {
                predicate = item => (item.TransactionDate >= modDateFrom && item.TransactionDate <= modDateTo);
            }
            else
            {
                if(DateFrom != DateTime.MinValue)
                    predicate = item => item.TransactionDate >= modDateFrom;
                else
                    predicate = item => item.TransactionDate <= modDateTo;
            }
            return predicate;
        }
        private Expression<Func<vw_ReconciliationItem, bool>> basedOnFCTURN(ReconciliationWorklistSearchCriteria criteria)
        {
            if (string.IsNullOrWhiteSpace(criteria.FCTURN))
                return null;

            Expression<Func<vw_ReconciliationItem, bool>> predicate = null;
            predicate = item => item.FCTURN.StartsWith(criteria.FCTURN);
            return predicate;
        }
        private Expression<Func<vw_ReconciliationItem, bool>> basedOnTransactionType(ReconciliationWorklistSearchCriteria criteria)
        {
            if (string.IsNullOrWhiteSpace(criteria.TransactionType))
                return null;

            Expression<Func<vw_ReconciliationItem, bool>> predicate = null;
            predicate = item => item.TransactionType == criteria.TransactionType;
            return predicate;
        }
        private Expression<Func<vw_ReconciliationItem, bool>> basedOnReferenceNumber(ReconciliationWorklistSearchCriteria criteria)
        {
            if (string.IsNullOrWhiteSpace(criteria.ReferenceNumber))
                return null;

            Expression<Func<vw_ReconciliationItem, bool>> predicate = null;
            predicate = item => item.ReferenceNumber.StartsWith(criteria.ReferenceNumber);
            return predicate;
        }
        private Expression<Func<vw_ReconciliationItem, bool>> basedOnBatchNumber(ReconciliationWorklistSearchCriteria criteria)
        {
            if (string.IsNullOrWhiteSpace(criteria.BatchNumber))
                return null;

            Expression<Func<vw_ReconciliationItem, bool>> predicate = null;
            predicate = item => item.BatchNumber.StartsWith(criteria.BatchNumber);
            return predicate;
        }

        
    }
}
