import { tblMilestoneLabel } from './tblMilestoneLabel';
import { tblMilestone } from './tblMilestone';

/// <code-import> Place custom imports between <code-import> tags

/// </code-import>

export class tblMilestoneCode {

   /// <code> Place custom code between <code> tags
   
   /// </code>

   // Generated code. Do not place code below this line.
   MilestoneCodeID: number;
   Name: string;
   Description: string;
   BusinessModel: string;
   IsMajor: boolean;
   IsOptional: boolean;
   IsPreClosing: boolean;
   IsPostClosing: boolean;
   IsExternalActivated: boolean;
   tblMilestoneLabels: tblMilestoneLabel[];
   tblMilestones: tblMilestone[];
}

