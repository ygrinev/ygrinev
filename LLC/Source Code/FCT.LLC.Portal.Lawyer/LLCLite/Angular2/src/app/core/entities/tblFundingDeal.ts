import { tblDealScope } from './tblDealScope';
import { tblDealFundsAllocation } from './tblDealFundsAllocation';

/// <code-import> Place custom imports between <code-import> tags

/// </code-import>

export class tblFundingDeal {

   /// <code> Place custom code between <code> tags
   
   /// </code>

   // Generated code. Do not place code below this line.
   FundingDealID: number;
   DealScopeID: number;
   InvitationSent: Date;
   InvitationAccepted: Date;
   SignedByVendor: Date;
   SignedByPurchaser: Date;
   Funded: Date;
   Disbursed: Date;
   PayoutSent: Date;
   AssignedTo: string;
   SignedByPurchaserName: string;
   SignedByVendorName: string;
   OtherLawyerFirstName: string;
   OtherLawyerLastName: string;
   OtherLawyerFirmName: string;
   DealScope: tblDealScope;
   tblDealFundsAllocations: tblDealFundsAllocation[];
}

