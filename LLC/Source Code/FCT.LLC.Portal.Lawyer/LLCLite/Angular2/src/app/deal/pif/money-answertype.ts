import { AnswerTypeBase } from './answertype-base';
import { TextboxAnswerType } from './textbox-answertype';

export class MoneyAnswerType extends TextboxAnswerType {
    DisplayControlType = "MoneyTextBox";

    public get AnswerDataType(): string {
        return 'money';
    }

    constructor(options: {} = {}) {
        super(options);
    }
}