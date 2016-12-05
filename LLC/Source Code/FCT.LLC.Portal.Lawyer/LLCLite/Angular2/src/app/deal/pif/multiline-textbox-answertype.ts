import { AnswerTypeBase } from './answertype-base';
import { TextboxAnswerType } from './textbox-answertype';

export class MultiLineTextBoxAnswerType extends TextboxAnswerType {
    
    DisplayControlType = "MultiLineTextBox";

    public get AnswerDataType(): string {
        return 'string';
    }

    constructor(options: {} = {}) {
        super(options);
    }
}