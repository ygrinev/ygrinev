import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { QuestionService } from './question.service';
import { DealService } from '../../deal/services/deal-service';

import { tblDeal } from '../../core/entities/EntityModel';

import { Question } from './question';

@Injectable()
export class PifQuestionResolver implements Resolve<Question[]> {
    model: tblDeal;
    constructor(private dealService: DealService, private questionService: QuestionService) {}

    resolve(route: ActivatedRouteSnapshot) {
        let self = this;

        self.model = this.dealService.getCurrentDeal();

        var pifQuestions = self.questionService.getPifQuestions(self.model.DealID, self.model.Status);

        return pifQuestions;
    }
}