import { tblAnswer, tblAnswerType } from "../../core/entities/EntityModel"

export class AnswerTypeBase<T> extends tblAnswerType{
    value: T;
    required: boolean;
    AddControlOnNewLine: boolean;

    public get ControlMaxSizeSpecified(): boolean {
        var self = this;
        var specified = false;

        if (self.ControlMaxSize > 0) {
            specified = true;
        }

        return specified;
    }

    public get id(): string {
        return this.QuestionID + "__" + this.AnswerTypeID;
    }

    public get label(): string {
        return this.EnglishText;
    }

    constructor(options: {
        Answer?: tblAnswer,
        value?: T,
        AnswerTypeID?: number,
        QuestionID?:number,
        EnglishText?: string,
        FrenchText?: string,
        required?: boolean,
        DisplaySequence?: number,
        DisplayControlType?: string,
        AddControlOnNewLine?: boolean,
        ValidationExpression?: string,
        AnswerDataType?: string,
        ControlMaxSize?: number,
        answerTypeEntity?: tblAnswerType,
    } = {}) {
        super();

        if ((options.answerTypeEntity !== null) && (options.answerTypeEntity !== undefined)) {
            this.Answer = options.answerTypeEntity.Answer || null;
            this.AnswerTypeID = options.answerTypeEntity.AnswerTypeID || 0;
            this.QuestionID = options.answerTypeEntity.QuestionID || 0;
            this.EnglishText = options.answerTypeEntity.EnglishText || '';
            this.FrenchText = options.answerTypeEntity.FrenchText || '';
            this.DisplaySequence = options.answerTypeEntity.DisplaySequence === undefined ? 1 : options.answerTypeEntity.DisplaySequence;
            this.DisplayControlType = options.answerTypeEntity.DisplayControlType || 'NotSet';
            this.AddControlOnNewLine = false;
            this.ValidationExpression = options.answerTypeEntity.ValidationExpression || '';
            this.AnswerDataType = options.answerTypeEntity.AnswerDataType || '';
            this.ControlMaxSize = options.answerTypeEntity.ControlMaxSize || 0;
        } else {
            this.Answer = options.Answer || null;
            this.value = options.value;
            this.AnswerTypeID = options.AnswerTypeID || 0;
            this.QuestionID = options.QuestionID || 0;
            this.EnglishText = options.EnglishText || '';
            this.FrenchText = options.FrenchText || '';
            this.required = !!options.required;
            this.DisplaySequence = options.DisplaySequence === undefined ? 1 : options.DisplaySequence;
            this.DisplayControlType = options.DisplayControlType || 'NotSet';
            this.AddControlOnNewLine = options.AddControlOnNewLine || false;
            this.ValidationExpression = options.ValidationExpression || '';
            this.AnswerDataType = options.AnswerDataType || '';
            this.ControlMaxSize = options.ControlMaxSize || 0;   
        }
    }
    
}