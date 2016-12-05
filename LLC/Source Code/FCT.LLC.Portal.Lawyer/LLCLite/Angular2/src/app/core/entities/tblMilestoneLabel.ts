import { tblLender } from './tblLender';
import { tblMilestoneCode } from './tblMilestoneCode';

/// <code-import> Place custom imports between <code-import> tags

/// </code-import>

export class tblMilestoneLabel {

   /// <code> Place custom code between <code> tags
   
   /// </code>

   // Generated code. Do not place code below this line.
   MilestoneLabelID: number;
   MilestoneCodeID: number;
   LenderID: number;
   SequenceNumber: number;
   LabelEnglish: string;
   LabelFrench: string;
   EffectiveDate: Date;
   TerminationDate: Date;
   tblLender: tblLender;
   tblMilestoneCode: tblMilestoneCode;
}

