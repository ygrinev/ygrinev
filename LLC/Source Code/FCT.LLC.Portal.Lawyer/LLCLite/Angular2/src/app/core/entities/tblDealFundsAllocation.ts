import { tblFundingDeal } from './tblFundingDeal';
import { tblFee } from './tblFee';

/// <code-import> Place custom imports between <code-import> tags

/// </code-import>

export class tblDealFundsAllocation {

   /// <code> Place custom code between <code> tags
   
   /// </code>

   // Generated code. Do not place code below this line.
   DealFundsAllocationID: number;
   ReferenceNumber: string;
   Amount: number;
   DepositDate: Date;
   BankNumber: string;
   BranchNumber: string;
   AccountNumber: string;
   WireDepositDetails: string;
   NotificationTimeStamp: Date;
   ShortFCTRefNumber: string;
   FundingDealID: number;
   LawyerID: number;
   AllocationStatus: string;
   Version: any;
   RecordType: string;
   TrustAccountID: number;
   FeeID: number;
   Reconciled: Date;
   ReconciledBy: string;
   PaymentOriginatorName: string;
   Fee: tblFee;
   tblFundingDeal: tblFundingDeal;
}

