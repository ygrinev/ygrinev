import { tblProperty } from './tblProperty';
import { tblDealContact } from './tblDealContact';
import { tblLawyer } from './tblLawyer';
import { tblDealScope } from './tblDealScope';
import { tblDealHistory } from './tblDealHistory';
import { tblLender } from './tblLender';
import { tblMilestone } from './tblMilestone';
import { tblMortgage } from './tblMortgage';
import { tblMortgagor } from './tblMortgagor';
import { tblNote } from './tblNote';
/// <code-import> Place custom imports between <code-import> tags
import { core } from 'breeze-client';
import { EntityBase } from './EntityBase';
/// </code-import>

export class tblDeal extends EntityBase {

   /// <code> Place custom code between <code> tags
    addNote(noteText: string, dealId: number): tblNote {
        return <tblNote>this.entityAspect.entityManager.createEntity('tblNote', { notes: noteText, dealID: dealId });
    }  
   /// </code>

   // Generated code. Do not place code below this line.
   DealID: number;
   FCTRefNum: string;
   LenderRefNum: string;
   LenderID: number;
   RFIReceiveDate: Date;
   BranchID: number;
   ContactID: number;
   MortgageCentreID: number;
   MtgCentreContactID: number;
   LenderComment: string;
   Status: string;
   StatusDate: Date;
   StatusUserID: number;
   StatusUserType: string;
   StatusReason: string;
   StatusReasonID: number;
   LawyerID: number;
   LawyerDeclinedFlag: boolean;
   LawyerAcceptDeclinedDate: Date;
   LawyerAccountID: number;
   FinalDocsPostedDate: Date;
   UserNotification: boolean;
   LendersAttentionFlag: boolean;
   Promotion: boolean;
   FundStatusID: number;
   Sequence: number;
   FundsDisbursed: boolean;
   FundRequestDate: Date;
   LawyerMatterNumber: string;
   IsLLC: boolean;
   LenderNewNotes: boolean;
   LenderUpdated: boolean;
   CreditCardID: number;
   LastModified: any;
   BusinessModel: string;
   BCOnlineID: number;
   BillingAmountDetailID: number;
   LawyerApplication: string;
   ActionableNotesCompleted: number;
   FinalReportNotificationNo: number;
   ServiceAddressTypeID: number;
   MailingAddressTypeID: number;
   MortgageOwningBranchID: number;
   IsRFF: boolean;
   Encumbrances: string;
   DealTrustAccountID: number;
   MtgOwningBranchContactID: number;
   FundingPaymentMethodID: number;
   LenderDealRefNumber: string;
   LawyerAmendmentImmediate: boolean;
   LawyerAmendmentSROT: boolean;
   RFFComment: string;
   RFFClosingDate: Date;
   IsSubmissionPending: boolean;
   LawyerAppointmentDate: Date;
   LenderRepresentativeFirstName: string;
   LenderRepresentativeLastName: string;
   LenderRepresentativeTitle: string;
   DistrictName: string;
   DealClosingOptionID: number;
   SavedDocumentClosingOptionID: number;
   LenderSecurityType: string;
   RFFNotifiedDate: Date;
   LenderDealAlternateID: number;
   DealScopeID: number;
   PrimaryDealContactID: number;
   LawyerActingFor: string;
   DealType: string;
   LenderFINumber: string;
   IsLawyerConfirmedClosing: boolean;
   tblDealContacts: tblDealContact[];
   tblDealHistory: tblDealHistory[];
   tblDealScope: tblDealScope;
   tblLawyer: tblLawyer;
   tblLender: tblLender;
   tblMilestones: tblMilestone[];
   tblMortgages: tblMortgage[];
   tblMortgagors: tblMortgagor[];
   tblNotes: tblNote[];
   tblProperties: tblProperty[];
}

