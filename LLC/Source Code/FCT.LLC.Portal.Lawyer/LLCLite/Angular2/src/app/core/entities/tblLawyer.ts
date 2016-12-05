import { tblDeal } from './tblDeal';
import { tblDealContact } from './tblDealContact';

/// <code-import> Place custom imports between <code-import> tags

/// </code-import>

export class tblLawyer {

   /// <code> Place custom code between <code> tags
   
   /// </code>

   // Generated code. Do not place code below this line.
   LawyerID: number;
   LastName: string;
   FirstName: string;
   MiddleName: string;
   LawFirm: string;
   UnitNo: string;
   Address: string;
   Address2: string;
   City: string;
   Province: string;
   PostalCode: string;
   Phone: string;
   MobilePhone: string;
   Fax: string;
   EMail: string;
   UserID: string;
   Password: any;
   PasswordReset: boolean;
   Active: boolean;
   LastModified: any;
   Comments: string;
   IsAssistant: boolean;
   LawyerSoftwareUsed: string;
   Profession: string;
   RegistrationDate: Date;
   AgreementReceivedDate: Date;
   PasswordSetDate: Date;
   StreetNumber: string;
   UserLanguage: string;
   Country: string;
   BillingAddressID: number;
   LawyerCode: number;
   ProfileCreateDateTime: Date;
   ProfileActiveDateTime: Date;
   ProfileDeactiveDateTime: Date;
   ProfileModifyDateTime: Date;
   ValidatedByFCT: boolean;
   RequestSource: string;
   UserStatusID: number;
   InternetBrowserUsed: string;
   LawSocietyFirstName: string;
   LawSocietyMiddleName: string;
   LawSocietyLastName: string;
   SolicitorSyncLawSocietyStatus: string;
   TestAccount: boolean;
   tblDealContacts: tblDealContact[];
   tblDeals: tblDeal[];
}

