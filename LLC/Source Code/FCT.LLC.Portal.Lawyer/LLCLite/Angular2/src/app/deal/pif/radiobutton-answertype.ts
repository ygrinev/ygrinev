import { AnswerTypeBase } from './answertype-base';

export class RadiobuttonAnswerType extends AnswerTypeBase<string> {
    checked: boolean;

    DisplayControlType = "RadioButton";

    public get AnswerDataType(): string {
        return 'bit';
    }

    constructor(options: {} = {}) {
        super(options);
        this.checked = options['checked'] || false;
        this.required = true;
    }
}
