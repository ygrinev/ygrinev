import { tblAnswerType } from './tblAnswerType';

/// <code-import> Place custom imports between <code-import> tags

/// </code-import>

export class tblQuestion {

   /// <code> Place custom code between <code> tags
   
   /// </code>

   // Generated code. Do not place code below this line.
   QuestionID: number;
   DisplaySequence: number;
   IsFraudQuestion: boolean;
   IsForOrderingTitleInsurance: boolean;
   IsFinalQuestion: boolean;
   IsNationalExcludeQC: boolean;
   EnglishText: string;
   FrenchText: string;
   Province: string;
   AnswerTypes: tblAnswerType[];
}

