import { tblLawyer } from './tblLawyer';
import { tblDealScope } from './tblDealScope';

/// <code-import> Place custom imports between <code-import> tags

/// </code-import>

export class vw_Deal {

   /// <code> Place custom code between <code> tags
   
   /// </code>

   // Generated code. Do not place code below this line.
   DealID: number;
   LLCRefNum: string;
   FCTRefNum: string;
   DealScopeID: number;
   LawyerID: number;
   LenderRefNum: string;
   Status: string;
   StatusUserType: string;
   StatusReason: string;
   Address: string;
   VendorLastName: string;
   LawyerMatterNumber: string;
   BusinessModel: string;
   ClientName: string;
   LawyerActingFor: string;
   ClosingDate: Date;
   MortgageNumber: string;
   LawyerName: string;
   WireDepositVerificationCode: string;
   tblDealScope: tblDealScope;
   tblLawyer: tblLawyer;
}

