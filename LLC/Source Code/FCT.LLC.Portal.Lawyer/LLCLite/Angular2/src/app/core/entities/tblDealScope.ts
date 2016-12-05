import { tblDeal } from './tblDeal';
import { tblFundingDeal } from './tblFundingDeal';
import { tblVendor } from './tblVendor';

/// <code-import> Place custom imports between <code-import> tags

/// </code-import>

export class tblDealScope {

   /// <code> Place custom code between <code> tags
   
   /// </code>

   // Generated code. Do not place code below this line.
   DealScopeID: number;
   FCTRefNumber: string;
   ShortFCTRefNumber: string;
   WireDepositVerificationCode: string;
   WireDepositDetails: string;
   tblDeals: tblDeal[];
   tblFundingDeals: tblFundingDeal[];
   tblVendors: tblVendor[];
}

