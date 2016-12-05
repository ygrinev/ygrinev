import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { Question } from './question';
import { AnswerTypeBase } from './answertype-base';

@Injectable()

// Adds all of the answertypes for a given question to a single form group
export class QuestionControlService {

    questionsToFormGroup(questions: Question[]) {
        let group: any = {};
        for (let question of questions) {
            question.AnswerTypes.forEach(answertype => {
                var validators = new Array();
                
                var formControl = new FormControl();
                var validationExpression = this.getValidationExpression(answertype);

                if (answertype.required) {
                    validators.push(Validators.required);
                }

                if (answertype.ControlMaxSizeSpecified) {
                    validators.push(Validators.maxLength(answertype.ControlMaxSize));
                }
                if ((validationExpression !== null) && (validationExpression !== undefined) && (validationExpression.length > 0)) {
                    validators.push(Validators.pattern(validationExpression));
                } 

                formControl = new FormControl(answertype.value || '', Validators.compose(validators));

                group[answertype.id] = formControl;
            });
        }

        return new FormGroup(group);
    }

    getValidationExpression(answerType: any): string {
        var validationExpression = "";
        if ((answerType !== null) && (answerType !== undefined)) {
            if ((answerType.ValidationExpression !== null) && (answerType.ValidationExpression !== undefined) && (answerType.ValidationExpression.length > 0)) {
                validationExpression = answerType.ValidationExpression;
            }            
        }

        return validationExpression;
    }
}