import { tblDealFundsAllocation } from './tblDealFundsAllocation';
import { tblDisbursement } from './tblDisbursement';

/// <code-import> Place custom imports between <code-import> tags

/// </code-import>

export class tblPaymentRequest {

   /// <code> Place custom code between <code> tags
   
   /// </code>

   // Generated code. Do not place code below this line.
   PaymentRequestID: number;
   DisbursementID: number;
   DealFundsAllocationID: number;
   Message: string;
   RequestDate: Date;
   tblDealFundsAllocation: tblDealFundsAllocation;
   tblDisbursement: tblDisbursement;
}

