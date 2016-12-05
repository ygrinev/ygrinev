import { tblFundingDeal } from './tblFundingDeal';
import { tblFee } from './tblFee';

/// <code-import> Place custom imports between <code-import> tags

/// </code-import>

export class tblDisbursement {

   /// <code> Place custom code between <code> tags
   
   /// </code>

   // Generated code. Do not place code below this line.
   DisbursementID: number;
   FundingDealID: number;
   PayeeID: number;
   PayeeType: string;
   PayeeName: string;
   PayeeComments: string;
   Amount: number;
   PaymentMethod: string;
   NameOnCheque: string;
   UnitNumber: string;
   StreetNumber: string;
   City: string;
   Province: string;
   PostalCode: string;
   Country: string;
   ReferenceNumber: string;
   AssessmentRollNumber: string;
   TrustAccountID: number;
   BankNumber: string;
   BranchNumber: string;
   AccountNumber: string;
   Instructions: string;
   AgentFirstName: string;
   AgentLastName: string;
   AccountAction: string;
   FCTFeeSplit: string;
   DisbursementComment: string;
   DisbursedAmount: number;
   DisbursementStatus: string;
   VendorFeeID: number;
   PurchaserFeeID: number;
   StreetAddress1: string;
   StreetAddress2: string;
   ChainDealID: number;
   Reconciled: Date;
   ReconciledBy: string;
   PaymentReferenceNumber: string;
   AccountHolderName: string;
   Token: string;
   PurchaserFee: tblFee;
   tblFundingDeal: tblFundingDeal;
   VendorFee: tblFee;
}

