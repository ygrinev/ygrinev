import { tblBuilderLegalDescription } from './tblBuilderLegalDescription';
import { tblDeal } from './tblDeal';
import { vw_Deal } from './vw_Deal';
import { tblPIN } from './tblPIN';

/// <code-import> Place custom imports between <code-import> tags
import { EntityBase } from './EntityBase';

/// </code-import>

export class tblProperty extends EntityBase {

   /// <code> Place custom code between <code> tags
   
   /// </code>

   // Generated code. Do not place code below this line.
   PropertyID: number;
   DealID: number;
   Address: string;
   City: string;
   Province: string;
   PostalCode: string;
   HomePhone: string;
   BusinessPhone: string;
   LegalDescription: string;
   ARN: string;
   IsCondo: boolean;
   EstateType: string;
   InstrumentNumber: string;
   RegistrationDate: Date;
   AmountOfTaxesPaid: number;
   TaxesPaidOnClosing: boolean;
   PropertyType: string;
   NumberOfUnits: number;
   OccupancyType: string;
   IsNewHome: boolean;
   AnnualTaxAmount: number;
   IsPrimaryProperty: boolean;
   IsLenderToCollectPropertyTaxes: boolean;
   RegistryOffice: string;
   CondoLevel: string;
   CondoUnitNumber: string;
   CondoCorporationNumber: string;
   LastModified: any;
   UnitNumber: string;
   StreetNumber: string;
   Address2: string;
   Country: string;
   BookFolioRoll: string;
   PageFrame: string;
   CondoDeclarationRegistrationNumber: string;
   CondoDeclarationRegistrationDate: Date;
   CondoBookNoOfDeclaration: string;
   CondoPageNumberOfDeclaration: string;
   CondoDeclarationAcceptedDate: Date;
   CondoPlanRegistrationDate: Date;
   CondoDeclarationDate: Date;
   CondoPlanNumber: string;
   AssignmentOfRentsRegistrationNumber: string;
   RentAssignment: boolean;
   LenderPropertyID: number;
   MortgagePriority: string;
   OtherEstateTypeDescription: string;
   Municipality: string;
   CondoDeclarationModificationNumber: string;
   JudicialDistrict: string;
   CondoDeclarationModificationDate: Date;
   IsCondominium: boolean;
   AssignmentOfRentsRegistrationDate: Date;
   NewHomeWarranty: boolean;
   TaxesPaidToDate: Date;
   tblBuilderLegalDescriptions: tblBuilderLegalDescription[];
   tblDeal: tblDeal;
   tblPINs: tblPIN[];
   vw_Deal: vw_Deal;
}

