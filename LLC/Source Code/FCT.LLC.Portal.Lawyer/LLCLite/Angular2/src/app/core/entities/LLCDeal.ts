import { EntityBase } from './EntityBase';
import { LLCMortgage } from './LLCMortgage';
import { LLCNote } from './LLCNote';
import { LLCProperty } from './LLCProperty';
/// <code-import> Place custom imports between <code-import> tags
import { core } from 'breeze-client';

/// </code-import>

export class LLCDeal extends EntityBase {

   /// <code> Place custom code between <code> tags
    addNote(noteText: string, dealId: number): LLCNote {
        return <LLCNote>this.entityAspect.entityManager.createEntity('LLCNote', { notes: noteText, dealID: dealId });
    }   
   /// </code>

   // Generated code. Do not place code below this line.
   dealID: number;
   fCTRefNum: string;
   closedDate: Date;
   mortgages: LLCMortgage[];
   notes: LLCNote[];
   properties: LLCProperty[];
}

