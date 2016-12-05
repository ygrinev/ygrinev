import { AnswerTypeBase } from './answertype-base';

export class TextboxAnswerType extends AnswerTypeBase<string> {
    DisplayControlType = "TextBox";

    constructor(options: {} = {}) {
        super(options);

        if ((this.ValidationExpression !== null) && (this.ValidationExpression !== undefined) && (this.ValidationExpression.length > 0)) {
            this.required = true;
        }
    }
}
