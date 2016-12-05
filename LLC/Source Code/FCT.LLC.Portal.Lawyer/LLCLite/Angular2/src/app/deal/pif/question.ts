import { AnswerTypeBase } from './answertype-base';
import { tblQuestion } from "../../core/entities/EntityModel"

export class Question extends tblQuestion{
    private _isFraudQuestion:boolean;

    AnswerTypes: AnswerTypeBase<any>[];

    public get label(): string {
        return this.EnglishText;
    }

    public get IsValid(): boolean {
        // TODO: Evaluate whether or not the answer types for this question are valid
        return true;
    }

    public get IsFraudQuestion(): boolean {
        return (this._isFraudQuestion && !this.IsFinalQuestion );
    }

    public get IsUnderwritingQuestion(): boolean {
        return (!this.IsFraudQuestion && !this.IsFinalQuestion);
    }

    constructor(questionEntity?: tblQuestion) {
        super();

        if (questionEntity !== null) {
            this.QuestionID = questionEntity.QuestionID || 0;
            this.DisplaySequence = questionEntity.DisplaySequence || 0;
            this.IsForOrderingTitleInsurance = questionEntity.IsForOrderingTitleInsurance || false;
            this.IsFinalQuestion = questionEntity.IsFinalQuestion || false;
            this.IsNationalExcludeQC = questionEntity.IsNationalExcludeQC || false;
            this.EnglishText = questionEntity.EnglishText || "";
            this.FrenchText = questionEntity.FrenchText || "";
            this.Province = questionEntity.Province || "";

            if (questionEntity.AnswerTypes !== null) {
                
                this.AnswerTypes = new Array();
                for (let answertypeEntity of questionEntity.AnswerTypes) {
                    var answerType = new AnswerTypeBase(answertypeEntity);
                    this.AnswerTypes.push(answerType);
                }
            }
        }
    }
}