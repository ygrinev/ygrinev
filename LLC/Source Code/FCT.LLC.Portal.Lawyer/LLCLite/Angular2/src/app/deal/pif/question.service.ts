import { Injectable, ReflectiveInjector } from '@angular/core';
import { Headers, Http, Response } from '@angular/http';
import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs/Observable';

import { QuestionControlService } from './question-control.service';

import { Question } from './question';
import { AnswerTypeBase } from './answertype-base';
import { TextboxAnswerType } from './textbox-answertype';
import { MultiLineTextBoxAnswerType } from './multiline-textbox-answertype';
import { RadiobuttonAnswerType } from './radiobutton-answertype';
import { MoneyAnswerType } from './money-answertype';
import { tblQuestion } from "../../core/entities/EntityModel"
import { GetPifQuestionRequest } from "../../core/dto/DtoModel"
//

@Injectable()
export class QuestionService {
    pifQuestions: Question[];
    constructor(private http: Http, private questionControlService: QuestionControlService) {
    }
    
    getPifQuestions(dealId: number, dealStatus: string): Observable<any[]> {
        let self = this;
        let recalculateQuestions = true;

        if (dealStatus.toLowerCase().indexOf("accepted") <= 0) {
            recalculateQuestions = false;
        }
        var request = new GetPifQuestionRequest();
        request.dealId = dealId;
        request.recalculateQuestions = recalculateQuestions;

        var body = JSON.stringify(request);
        var headers = new Headers();
        headers.append('Content-Type', 'application/json; charset=utf-8');

        return self.http
            .post('http://localhost/LLCWebApi/breeze/Deal/GetPifQuestionsByDealId', body, { headers: headers })
            .map(self.extractData)
            .catch(self.handleError);
    }
    
    private extractData(res: Response) {
        
        var pifQuestions = new Array();
        let body = res.json();

        if ((body !== null) && (body !== undefined)) {
            var questionEntities = body["Questions"];

            if ((questionEntities !== null) && (questionEntities !== undefined) && (questionEntities.length > 0)) {
                
                for (let questionEntity of questionEntities) {
                    var pifQuestion = new Question(questionEntity);

                    var index = 0;
                    for (let answerType of pifQuestion.AnswerTypes) {

                        if (index > 0) {
                            if ((pifQuestion.AnswerTypes[index - 1] !== null) && (pifQuestion.AnswerTypes[index - 1] !== undefined)) {
                                let previousControlType = pifQuestion.AnswerTypes[index - 1].DisplayControlType;
                                let currentControlType = answerType.DisplayControlType;
                                answerType.AddControlOnNewLine = ((previousControlType === "RadioButton") ||
                                    (previousControlType === "TextBox")) &&
                                    ((currentControlType === "TextBox") ||
                                        (currentControlType === "MultiLineTextBox") ||
                                        (currentControlType === "MoneyTextBox"));
                            }
                        }
                        index++;
                    }
                    pifQuestions.push(pifQuestion);
                    
                }
                
            }
        }
        
        return pifQuestions;
    }
    
    private handleError(error: Response | any) {
        // In a real world app, we might use a remote logging infrastructure
        let errMsg: string;
        if (error instanceof Response) {
            const body = error.json() || '';
            const err = body.error || JSON.stringify(body);
            errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
        } else {
            errMsg = error.message ? error.message : error.toString();
        }
        console.error(errMsg);
        return Observable.throw(errMsg);
    }
}
