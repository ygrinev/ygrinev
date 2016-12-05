
/// <code-import> Place custom imports between <code-import> tags
import { tblAnswer } from './tblAnswer';
/// </code-import>

export class tblAnswerType {

   /// <code> Place custom code between <code> tags
   
   /// </code>

   // Generated code. Do not place code below this line.
   AnswerTypeID: number;
   QuestionID: number;
   DisplaySequence: number;
   DisplayControlType: string;
   ControlMaxSize: number;
   ValidationExpression: string;
   AnswerDataType: string;
   EnglishText: string;
   FrenchText: string;
   Answer: tblAnswer;
}

