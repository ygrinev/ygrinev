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
using System.Data.SqlClient;
using System.Data;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class DealSearchRepository : Repository<vw_Deal>,IDealSearchRepository
    {

        private readonly EFBusinessContext _context;
        public DealSearchRepository(EFBusinessContext context)
            : base(context)
        {
            _context = context;
            //this._context.Database.CommandTimeout = 180;
        }

        public List<vw_Deal> SearchDeal(SearchDealCriteria searchDealCriteria, OrderBySpecificationCollection orderBySpecifications, int pageIndex, int pageSize
                , UserContext usercontext, out int totalRowCount)
        {
            var rowCount = new SqlParameter{ParameterName = "@RowCount", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output};
            var lenderRefNum = new SqlParameter { ParameterName = "@LenderRefNum", Value = searchDealCriteria.LenderReferenceNumber ?? "" };
            var vendorName = new SqlParameter { ParameterName = "@VendorName", Value = searchDealCriteria.VendorName ?? "" };
            var fctRefNum = new SqlParameter { ParameterName = "@FCTRefNum", Value = searchDealCriteria.LLCFCTURN ?? "" };
            var shortFCTRefNumber = new SqlParameter { ParameterName = "@ShortFCTRefNumber", Value = searchDealCriteria.DealScopeFCTURN ?? "" };
            var lawyerName = new SqlParameter { ParameterName = "@LawyerName", Value = searchDealCriteria.LawyerName ?? "" };
            var clientName = new SqlParameter { ParameterName = "@ClientName", Value = searchDealCriteria.ClientName ?? "" };
            var lawyerMatterNumber = new SqlParameter { ParameterName = "@LawyerMatterNumber", Value = searchDealCriteria.LawyerFileNumber ?? "" };
            var mortgageNumber = new SqlParameter { ParameterName = "@MortgageNumber", Value = searchDealCriteria.MortgageNumber ?? "" };
            var wireRefNumber = new SqlParameter { ParameterName = "@WireRefNumber", Value = searchDealCriteria.DisbursementReferenceNumber ?? "" };
            var disbursementAmount = new SqlParameter { ParameterName = "@DisbursementAmount", Value = ParseDecimal(searchDealCriteria.DisbursementAmount) };
            var closingDateFrom = new SqlParameter { ParameterName = "@ClosingDateFrom", Value = ParseDate(searchDealCriteria.ClosingDateFrom) };
            var closingDateTo = new SqlParameter { ParameterName = "@ClosingDateTo", Value = ParseDate(searchDealCriteria.ClosingDateTo) };
            var chequeNumber = new SqlParameter { ParameterName = "@ChequeNumber", Value = searchDealCriteria.ChequeNumber ?? "" };
            var wireDepositCode = new SqlParameter { ParameterName = "@WireDepositCode", Value = searchDealCriteria.WireDepositCode ?? "" };
            var outstandingDepositAmountFrom = new SqlParameter { ParameterName = "@OutstandingDepositAmountFrom", Value = ParseDecimal(searchDealCriteria.OutstandingDepositAmountFrom) };
            var outstandingDepositAmountTo = new SqlParameter { ParameterName = "@OutstandingDepositAmountTo", Value = ParseDecimal(searchDealCriteria.OutstandingDepositAmountTo) };
            var paymentReferenceNumber = new SqlParameter { ParameterName = "@PaymentReferenceNumber", Value = searchDealCriteria.PaymentReferenceNumber ?? "" };
            var batchNumber = new SqlParameter { ParameterName = "@BatchNumber", Value = searchDealCriteria.BatchNumber ?? "" };
            var orderByStrings = "";
            foreach(OrderBySpecification orderBySpec in orderBySpecifications)
            {
                    if (orderByStrings != "") orderByStrings += ", ";
                    switch (orderBySpec.OrderByColumn)
                    {
                        case OrderByColumn.DealStatus:
                            orderByStrings += "Status";
                            break;
                        case OrderByColumn.LenderReferenceNumber:
                            orderByStrings += "LenderRefNum";
                            break;
                        case OrderByColumn.ActingFor:
                            orderByStrings += "LawyerActingFor";
                            break;
                        case OrderByColumn.FCTURN:
                            orderByStrings += "FCTRefNum";
                            break;
                        case OrderByColumn.PropertyAddress:
                            orderByStrings += "Address";
                            break;
                        default:
                            orderByStrings += orderBySpec.OrderByColumn.ToString();
                            break;
                    }
                    if (orderBySpec.OrderByDirection == OrderByDirection.ASC)
                    {
                        orderByStrings += " ASC ";
                    }
                    else
                    {
                        orderByStrings += " DESC ";
                    }

            }
            var orderBy = new SqlParameter { ParameterName = "@OrderBy", Value = orderByStrings };
            var page_Index = new SqlParameter { ParameterName = "@PageIndex", Value = pageIndex };
            var page_Size = new SqlParameter { ParameterName = "@PageSize", Value = pageSize };
            var deals = _context.Database.SqlQuery<vw_Deal>("exec @RowCount = sp_BusinessService_DealSearch @LenderRefNum, @VendorName, @FCTRefNum, @ShortFCTRefNumber, @LawyerName, @ClientName, @LawyerMatterNumber, @MortgageNumber, @WireRefNumber, @DisbursementAmount, @ClosingDateFrom, @ClosingDateTo, @ChequeNumber, @WireDepositCode, @OutstandingDepositAmountFrom, @OutstandingDepositAmountTo, @PaymentReferenceNumber, @BatchNumber, @OrderBy, @PageIndex, @PageSize"
                , rowCount, lenderRefNum, vendorName, fctRefNum, shortFCTRefNumber, lawyerName, clientName, lawyerMatterNumber, mortgageNumber, wireRefNumber, disbursementAmount, closingDateFrom, closingDateTo, chequeNumber, wireDepositCode, outstandingDepositAmountFrom, outstandingDepositAmountTo, paymentReferenceNumber, batchNumber, orderBy, page_Index, page_Size);
            //Expression<Func<vw_Deal, bool>> filterPredicate = ComposeFilterPredicate(searchDealCriteria);

            //var orderByPredicate = ComposeOrderByPredicate(orderBySpecifications);

            //GenericRepository.Contracts.IQueryFluent<vw_Deal> query = Query(filterPredicate);
            //    //.Include(deal => deal.tblLawyer)
            //    //.Include(deal => deal.tblDealScope);

            ////is will set AsNoTracking() for select statements
            //query = query.Tracking(false);
            //query = query.OrderBy(orderByPredicate);
 
            //var deals = query.SelectPage(pageIndex, pageSize, out totalRowCount);

            ////r deals = _context.vw_Deals.Where(d => d.DealID == 13523);
            ////totalRowCount = 0;
            var vwDeals = deals.ToList();
            totalRowCount = (int)rowCount.Value;
            return vwDeals;
        }

        private object ParseDecimal(string amountString)
        {
            if (string.IsNullOrEmpty(amountString))
                return DBNull.Value;
            else
            {
                decimal amountt;
                if (decimal.TryParse(amountString, out amountt))
                    return amountt;
                else
                    return 0.0m;
            }
        }
        private object ParseDate(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
                return DBNull.Value;
            else
            {
                DateTime date;
                if (DateTime.TryParse(dateString, out date))
                    return date;
                else
                    return DBNull.Value;
            }
        }

        private Expression<Func<vw_Deal, bool>> ComposeFilterPredicate(SearchDealCriteria searchDealCriteria)
        {

            var filterPredicate = PredicateBuilder.True<vw_Deal>();
            Expression<Func<vw_Deal, bool>> searchPredicate=null;

            filterPredicate = deal => deal.Status.ToUpper() != "SYSTEM_DRAFT";

            if (!String.IsNullOrEmpty(searchDealCriteria.LenderReferenceNumber))
            {
                //LenderReferenceNumber
                searchPredicate = deal => deal.LenderRefNum.StartsWith(searchDealCriteria.LenderReferenceNumber);
            }
            else if (!String.IsNullOrEmpty(searchDealCriteria.VendorName))
            {
                searchPredicate = deal => deal.VendorLastName.StartsWith(searchDealCriteria.VendorName);
            }
            else if (!String.IsNullOrEmpty(searchDealCriteria.LLCFCTURN))
            {
                //LLCFCTRefNumber - The Long format will be searchable via the LLC FCT Ref Number.  
                //This is searched only for LLC related deals
                searchPredicate = (deal => deal.LLCRefNum.StartsWith(searchDealCriteria.LLCFCTURN));
            }
            else if(!string.IsNullOrEmpty(searchDealCriteria.DealScopeFCTURN))
            {
                //short format fcturn - FCT Reference Number
                //Related to EasyFund deals only
                //since this is going to be any easyfund, so the search with <> LLC
                //deal.BusinessModel.Contains("EASYFUND")
                searchPredicate = (deal => deal.BusinessModel != "LLC" && deal.tblDealScope.ShortFCTRefNumber.StartsWith(searchDealCriteria.DealScopeFCTURN)
                                   );
            }
            else if (!String.IsNullOrEmpty(searchDealCriteria.LawyerName))
            {
                //Lawyer Name
                //searchPredicate = deal => deal.tblLawyer.LastName.StartsWith(searchDealCriteria.LawyerName);
                List<int> dealIDs = GetLawyerNameDealIDs(searchDealCriteria.LawyerName);
                searchPredicate = BuildOrExpression<vw_Deal, int>(d => d.DealID, dealIDs);

            }
            else if (!String.IsNullOrEmpty(searchDealCriteria.ClientName))
            {
                //Mortgagor/Purchaser Name and Vendor Name
                 searchPredicate = deal => deal.ClientName.StartsWith(searchDealCriteria.ClientName);
            }
            else if (!String.IsNullOrEmpty(searchDealCriteria.LawyerFileNumber))
            {
                //TODO://Lawyer File #
                searchPredicate = deal => deal.LawyerMatterNumber.StartsWith(searchDealCriteria.LawyerFileNumber);
            }
            else if (!string.IsNullOrEmpty(searchDealCriteria.MortgageNumber))
            {
                //Mortgage Number
                searchPredicate = deal => deal.MortgageNumber.StartsWith(searchDealCriteria.MortgageNumber);
            }
            else if(!string.IsNullOrEmpty(searchDealCriteria.DisbursementReferenceNumber))
            {
                //DisbursementReferenceNumber
                List<int> dealIDs = GetWireRefNumberDealIDs(searchDealCriteria.DisbursementReferenceNumber);
                searchPredicate = BuildOrExpression<vw_Deal, int>(d => d.DealID, dealIDs);
            }
            else if (!string.IsNullOrEmpty(searchDealCriteria.DisbursementAmount))
            {
                decimal amt = 0.0m;
                if (decimal.TryParse(searchDealCriteria.DisbursementAmount, out amt))
                {
                    List<int> dealIDs = GetDisbursementAmountDealIDs(amt);
                    searchPredicate = BuildOrExpression<vw_Deal, int>(d => d.DealID, dealIDs); 
                }
                
            }
            else if (!string.IsNullOrEmpty(searchDealCriteria.ClosingDateFrom) &&
                    !string.IsNullOrEmpty(searchDealCriteria.ClosingDateTo))
            {
                //Closing Date
                DateTime ClosingDateFrom = DateTime.Parse(searchDealCriteria.ClosingDateFrom);
                DateTime ClosingDateTo = DateTime.Parse(searchDealCriteria.ClosingDateTo);

                searchPredicate = deal => deal.ClosingDate.HasValue
                    && deal.ClosingDate.Value >= ClosingDateFrom
                    && deal.ClosingDate.Value <= ClosingDateTo;
            }
            else if(!string.IsNullOrEmpty(searchDealCriteria.ChequeNumber))
            {
                //searchPredicate = deal => deal.ChequeNum.StartsWith(searchDealCriteria.ChequeNumber);
                List<int> dealIDs = GetChequeNumberDealIDs(searchDealCriteria.ChequeNumber);
                searchPredicate = BuildOrExpression<vw_Deal, int>(d => d.DealID, dealIDs); 

            }
            else if (!string.IsNullOrEmpty(searchDealCriteria.WireDepositCode))
            {
                searchPredicate = deal => deal.WireDepositVerificationCode.StartsWith(searchDealCriteria.WireDepositCode);
            }
            else if (!string.IsNullOrEmpty(searchDealCriteria.OutstandingDepositAmountFrom) &&
                !string.IsNullOrEmpty(searchDealCriteria.OutstandingDepositAmountTo))
            {

                decimal fromamt = 0.0m;
                decimal toamt = 0.0m;
                decimal.TryParse(searchDealCriteria.OutstandingDepositAmountFrom, out fromamt);
                decimal.TryParse(searchDealCriteria.OutstandingDepositAmountTo, out toamt);
                List<int> dealIDs = GetOustandingDepositDealIDs(fromamt, toamt);
                searchPredicate = BuildOrExpression<vw_Deal, int>(d => d.DealID, dealIDs); 
            }
            else if (!string.IsNullOrEmpty(searchDealCriteria.PaymentReferenceNumber))
            {
                List<int> dealIDs = GetWireRefEFTDealIDs(searchDealCriteria.PaymentReferenceNumber);
                searchPredicate = BuildOrExpression<vw_Deal, int>(d => d.DealID, dealIDs); 
            }

            else if (!string.IsNullOrEmpty(searchDealCriteria.BatchNumber))
            {
                List<int> dealIDs = GetBatchIDs(searchDealCriteria.BatchNumber);
                searchPredicate = BuildOrExpression<vw_Deal, int>(d => d.DealID, dealIDs); 
                
            }

           if(searchPredicate != null)
           {               
               filterPredicate = filterPredicate.And(searchPredicate);
           }                        

            return filterPredicate;
        }

        private List<int> GetLawyerNameDealIDs(string LawyerName)
        {
            List<int> LawyerIDs = _context.tblLawyers.AsNoTracking()
                                     .Where(p => p.LastName.StartsWith(LawyerName))
                                     .Select(p => p.LawyerID).ToList();
            List<int> DealIDs = _context.tblDeals.AsNoTracking()
                                .Where(p => LawyerIDs.Contains(p.LawyerID) && p.DealScopeID == null)
                                .Select(p => p.DealID).ToList();
            List<int> DealScopeIDs = _context.tblDeals.AsNoTracking()
                                    .Where(p => LawyerIDs.Contains(p.LawyerID) && p.DealScopeID != null)
                                    .Select(p => p.DealScopeID.Value).ToList();

            List<int> OtherDealIds = _context.tblDeals.AsNoTracking()
                                    .Where(p => DealScopeIDs.Contains(p.DealScopeID.Value))
                                    .Select(p => p.DealID).ToList();
            if (DealIDs == null)
            {
                DealIDs = new List<int>();
            }

            if (OtherDealIds != null)
            {
                DealIDs.AddRange(OtherDealIds);
            }

            return DealIDs;
        }


        private List<int> GetBatchIDs(string batchNum)
        {
            List<int> dealIDs = new List<int>();

            List<tblPaymentNotification> lstPaymentNotification = new List<tblPaymentNotification>();
            List<tblPaymentRequest> lstPaymentRequests = new List<tblPaymentRequest>();
            List<tblDisbursement> lstDisbursements = new List<tblDisbursement>();

            lstPaymentNotification = _context.tblPaymentNotifications.Where (p=>p.BatchID.StartsWith(batchNum) && p.NotificationType == "ChequeConfirmation").ToList();
            foreach (tblPaymentNotification pn in lstPaymentNotification)
            {
                lstPaymentRequests = _context.tblPaymentRequests.Where(pr=>pr.PaymentRequestID == pn.PaymentRequestID).ToList();
                foreach (tblPaymentRequest pr in lstPaymentRequests) {
                    lstDisbursements = _context.tblDisbursements.Where (d=>d.DisbursementID == pr.DisbursementID).ToList();
                    foreach (tblDisbursement disbursement in lstDisbursements) {
                        tblFundingDeal fundingDeal = _context.tblFundingDeals.Where(d => d.FundingDealID == disbursement.FundingDealID).First();
                        List<tblDeal> deals = _context.tblDeals.Where(d => d.DealScopeID == fundingDeal.DealScopeID).ToList();
                        foreach (tblDeal deal in deals)
                        {
                            dealIDs.Add(deal.DealID);
                        }
                    }
                    
                }
            }
            return dealIDs;
        }

        private List<int> GetWireRefEFTDealIDs(string wireRefEFT)
        {
            List<int> dealIDs = new List<int>();
            List<int> FundingDealIDs = new List<int>();
            List<int> FundsAllocFundingDealIDs = new List<int>();

            FundingDealIDs =    _context.tblDisbursements.AsNoTracking()
                                .Where(d => d.PaymentMethod == "WIRE"
                                    && d.PaymentReferenceNumber.StartsWith(wireRefEFT))
                                .Select(d => d.FundingDealID).ToList();

            //Get records where Funding DealID not null as the search is for deals
            FundsAllocFundingDealIDs =  _context.tblDealFundsAllocations.AsNoTracking()
                                        .Where(d => d.FundingDealID != null 
                                            //&& (d.RecordType == "DEPOSIT" || d.RecordType == "RETURN")
                                            && d.ReferenceNumber.StartsWith(wireRefEFT))
                                        .Select(d => d.FundingDealID.Value).ToList();

            if (FundingDealIDs == null)
            {
                FundingDealIDs = new List<int>();
            }

            if(FundsAllocFundingDealIDs != null && FundsAllocFundingDealIDs.Count > 0)
            {
                FundingDealIDs.AddRange(FundsAllocFundingDealIDs);
            }

            //Get the List of DealIDs by FundingDealIDs
            List<int> dealScopeIDs = _context.tblFundingDeals.AsNoTracking()
                        .Where(p => FundingDealIDs.Contains(p.FundingDealID))
                        .Select(d => d.DealScopeID)
                        .Distinct().ToList();

             if(dealScopeIDs != null && dealScopeIDs.Count > 0)
             {
                 dealIDs = _context.tblDeals.AsNoTracking()
                            .Where(d => d.DealScopeID != null && dealScopeIDs.Contains(d.DealScopeID.Value))
                            .Select(d => d.DealID)
                            .Distinct().ToList();
             }
            
            return dealIDs;            
        }


        private List<int> GetWireRefNumberDealIDs(string WireRefNumber)
        {
            List<int> dealIDs = new List<int>();
            List<tblDisbursement> lstDisbursements = new List<tblDisbursement>();
            lstDisbursements = _context.tblDisbursements.Where(d => (d.PaymentMethod == "WIRE TRANSFER" ||  d.PaymentMethod == "EFT") && d.ReferenceNumber.StartsWith(WireRefNumber)).ToList();
            foreach (tblDisbursement disbursement in lstDisbursements)
            {
                tblFundingDeal fundingDeal = _context.tblFundingDeals.Where(d => d.FundingDealID == disbursement.FundingDealID).First();
                List<tblDeal> deals = _context.tblDeals.Where(d => d.DealScopeID == fundingDeal.DealScopeID).ToList();
                foreach (tblDeal deal in deals)
                {
                    dealIDs.Add(deal.DealID);
                }
            }
            return dealIDs;
        }

        private List<int> GetDisbursementAmountDealIDs(decimal DisbursementAmount)
        {
            List<int> disbursementAmountDealIDs = new List<int>();
            List<tblDisbursement> lstDisbursements = _context.tblDisbursements.Where(d => d.Amount == DisbursementAmount).ToList();
            foreach (tblDisbursement disbursement in lstDisbursements)
            {
                tblFundingDeal fundingDeal = _context.tblFundingDeals.Where(d => d.FundingDealID == disbursement.FundingDealID).First();
                List<tblDeal> deals = _context.tblDeals.Where(d => d.DealScopeID == fundingDeal.DealScopeID).ToList();
                foreach (tblDeal deal in deals)
                {
                    disbursementAmountDealIDs.Add(deal.DealID);
                }
            }
            return disbursementAmountDealIDs;
        }
        private List<int> GetOustandingDepositDealIDs(decimal OutstandingDepositAmountFrom, decimal OutstandingDepositAmountTo)
        {
            List<int> outstandingDepositDealIDs = new List<int>();

            List<vw_EFDisbursementSummary> lstDisbursementSums = _context.vw_EFDisbursementSummaries.Where(ds=>ds.DepositAmountRequired - ds.DepositAmountReceived >= OutstandingDepositAmountFrom
                && ds.DepositAmountRequired - ds.DepositAmountReceived <= OutstandingDepositAmountTo).ToList();
            foreach (vw_EFDisbursementSummary disbSum in lstDisbursementSums)
            {
                tblFundingDeal fundingDeal = _context.tblFundingDeals.Where(d => d.FundingDealID == disbSum.FundingDealID).First();
                List<tblDeal> deals = _context.tblDeals.Where(d => d.DealScopeID == fundingDeal.DealScopeID).ToList();
                foreach (tblDeal deal in deals)
                {
                    outstandingDepositDealIDs.Add(deal.DealID);
                }
            }
            return outstandingDepositDealIDs;

        }

        private List<int> GetChequeNumberDealIDs(string ChequeNumber)
        {
            List<int> dealIDs = new List<int>();
            List<tblDisbursement> lstDisbursements = new List<tblDisbursement>();
            lstDisbursements = _context.tblDisbursements.Where(d => d.PaymentMethod == "CHEQUE" && d.PaymentReferenceNumber.StartsWith(ChequeNumber)).ToList();
            foreach (tblDisbursement disbursement in lstDisbursements)
            {
                tblFundingDeal fundingDeal = _context.tblFundingDeals.Where(d => d.FundingDealID == disbursement.FundingDealID).First();
                List<tblDeal> deals = _context.tblDeals.Where(d => d.DealScopeID == fundingDeal.DealScopeID).ToList();
                foreach (tblDeal deal in deals)
                {
                    dealIDs.Add(deal.DealID);
                }
            }
            return dealIDs;

        }

        private Expression<Func<TElement, bool>> BuildOrExpression<TElement, TValue>(
        Expression<Func<TElement, TValue>> valueSelector,
        IEnumerable<TValue> values)
        {
            if (null == valueSelector)
                throw new ArgumentNullException("valueSelector");
            if (null == values)
                throw new ArgumentNullException("values");
            ParameterExpression p = valueSelector.Parameters.Single();

            if (!values.Any())
                return e => false;

            var equals = values.Select(value =>
                (Expression)Expression.Equal(
                     valueSelector.Body,
                     Expression.Constant(
                         value,
                         typeof(TValue)
                     )
                )
            );
            var body = equals.Aggregate<Expression>(
                     (accumulate, equal) => Expression.Or(accumulate, equal)
             );

            return Expression.Lambda<Func<TElement, bool>>(body, p);
        }
        private Func<IQueryable<vw_Deal>, IOrderedQueryable<vw_Deal>> ComposeOrderByPredicate(OrderBySpecificationCollection orderBySpecifications)
        {
            Func<IQueryable<vw_Deal>, IOrderedQueryable<vw_Deal>> orderByPredicate;
            OrderBySpecification orderBySpecification = new OrderBySpecification();

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
                case OrderByColumn.DealStatus:
                    if (orderBySpecification.OrderByDirection == OrderByDirection.ASC)
                    {
                        orderByPredicate =
                            q => q.OrderBy(deal => deal.Status)
                                  .ThenBy(deal => deal.DealID);
                    }
                    else
                    {
                        orderByPredicate =
                            q => q.OrderByDescending(deal => deal.Status)
                                  .ThenByDescending(deal => deal.DealID);
                    }
                    break;
                case OrderByColumn.LenderReferenceNumber:
                    if (orderBySpecification.OrderByDirection == OrderByDirection.ASC)
                    {
                        orderByPredicate =
                            q => q.OrderBy(deal => deal.LenderRefNum)
                                  .ThenBy(deal => deal.DealID);
                    }
                    else
                    {
                        orderByPredicate =
                            q => q.OrderByDescending(deal => deal.LenderRefNum)
                                  .ThenByDescending(deal => deal.DealID);
                    }
                    break;
                case OrderByColumn.BusinessModel:
                    if (orderBySpecification.OrderByDirection == OrderByDirection.ASC)
                    {
                        orderByPredicate =
                            q => q.OrderBy(deal => deal.BusinessModel)
                            .ThenBy(deal => deal.DealID);
                    }
                    else
                    {
                        orderByPredicate =
                            q => q.OrderByDescending(deal => deal.BusinessModel)
                            .ThenByDescending(deal => deal.DealID);
                    }
                    break;

                case OrderByColumn.ClientName:
                    //currently does sort for Mortgagor only
                    //Need changes to include sorting with Vendors as well
                    if (orderBySpecification.OrderByDirection == OrderByDirection.ASC)
                    {
                        orderByPredicate =
                            q => q.OrderBy(deal => deal.ClientName)
                            .ThenBy(deal => deal.DealID);
                    }
                    else
                    {
                        orderByPredicate =
                            q => q.OrderByDescending(deal => deal.ClientName)
                            .ThenByDescending(deal => deal.DealID);
                    }
                    break;

                case OrderByColumn.ActingFor:
                    if (orderBySpecification.OrderByDirection == OrderByDirection.ASC)
                    {
                        orderByPredicate =
                            q => q.OrderBy(deal => deal.LawyerActingFor)
                                  .ThenBy(deal => deal.DealID);
                    }
                    else
                    {
                        orderByPredicate =
                            q => q.OrderByDescending(deal => deal.LawyerActingFor)
                                  .ThenByDescending(deal => deal.DealID);
                    }
                    break;

                case OrderByColumn.ClosingDate:
                    if (orderBySpecification.OrderByDirection == OrderByDirection.ASC)
                    {
                        orderByPredicate =
                            q => q.OrderBy(deal => deal.ClosingDate)
                                    .ThenBy(deal => deal.DealID);
                    }
                    else
                    {
                        orderByPredicate =
                            q => q.OrderByDescending(deal => deal.ClosingDate)
                                    .ThenByDescending(deal => deal.DealID);
                    }
                    break;

                case OrderByColumn.FCTURN:
                    //Sort should include combined ShortFCTURN and LongFCTURN
                    if (orderBySpecification.OrderByDirection == OrderByDirection.ASC)
                    {
                        //combine the order based on searchCriteria
                        orderByPredicate =
                            q => q.OrderBy(deal =>deal.FCTRefNum)
                                    .ThenBy(deal => deal.DealID);
                    }
                    else
                    {
                        orderByPredicate =
                            q => q.OrderByDescending(deal =>deal.FCTRefNum)
                                  //  .ThenByDescending(deal => deal.tblDealScope.ShortFCTRefNumber)
                                    .ThenByDescending(deal => deal.DealID);
                    }
                    break;

                case OrderByColumn.PropertyAddress:
                    if (orderBySpecification.OrderByDirection == OrderByDirection.ASC)
                    {
                        orderByPredicate =
                            q => q.OrderBy(deal => deal.Address)
                                    .ThenBy(deal => deal.DealID);
                    }
                    else
                    {
                        orderByPredicate =
                            q => q.OrderByDescending(deal => deal.Address)
                                    .ThenByDescending(deal => deal.DealID);
                    }
                    break;
                case OrderByColumn.LawyerName:
                    if (orderBySpecification.OrderByDirection == OrderByDirection.ASC)
                    {
                        orderByPredicate =
                            q => q.OrderBy(deal => deal.tblLawyer.LastName)
                                    .ThenBy(deal => deal.DealID);
                    }
                    else
                    {
                        orderByPredicate =
                            q => q.OrderByDescending(deal => deal.tblLawyer.LastName)
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
    }
}
