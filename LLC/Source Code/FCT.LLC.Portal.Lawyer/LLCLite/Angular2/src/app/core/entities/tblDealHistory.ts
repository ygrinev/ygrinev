
/// <code-import> Place custom imports between <code-import> tags
import { core } from 'breeze-client';
import { EntityBase } from './EntityBase';
/// </code-import>

export class tblDealHistory extends EntityBase {

   /// <code> Place custom code between <code> tags
   
   /// </code>

   // Generated code. Do not place code below this line.
   DealHistoryID: number;
   DealID: number;
   Activity: string;
   ActivityFrench: string;
   LogDate: Date;
   UserID: string;
   UserType: string;
   IsShowOnLender: boolean;
}

