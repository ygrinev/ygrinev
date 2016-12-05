import { tblFinancialInstitutionNumber } from './tblFinancialInstitutionNumber';
import { tblMilestoneLabel } from './tblMilestoneLabel';

/// <code-import> Place custom imports between <code-import> tags

/// </code-import>

export class tblLender {

   /// <code> Place custom code between <code> tags
   
   /// </code>

   // Generated code. Do not place code below this line.
   LenderID: number;
   LenderCode: string;
   Name: string;
   Address: string;
   City: string;
   Province: string;
   PostalCode: string;
   Phone: string;
   Fax: string;
   Active: boolean;
   LogoName: string;
   BillingID: string;
   LastModified: any;
   DepositToFCTAccountID: number;
   ShortName: string;
   Is2WayLender: boolean;
   IsRealLender: boolean;
   TestLender: boolean;
   tblFinancialInstitutionNumbers: tblFinancialInstitutionNumber[];
   tblMilestoneLabels: tblMilestoneLabel[];
}

