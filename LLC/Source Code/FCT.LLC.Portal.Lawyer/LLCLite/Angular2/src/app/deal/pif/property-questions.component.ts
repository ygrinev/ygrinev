import { Component, OnInit, OnDestroy, OnChanges, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormArray, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { BusyService, CanComponentDeactivate } from '../../core/services/common';

import { tblDeal } from '../../core/entities/EntityModel';

import { QuestionService } from './question.service';
import { QuestionControlService } from './question-control.service';
import { DealService } from '../../deal/services/deal-service';
import { Question } from './question';

@Component({
    selector: "property-questions",
    templateUrl: "./property-questions.html",
    providers: [QuestionService, QuestionControlService]
})
export class PropertyQuestionsComponent implements OnInit {
    pifQuestions: Question[];
    pifFormGroup: FormGroup;
    test: FormGroup;
    isDataAvailable: boolean;
    payLoad = '';
    pageTitle: string;
    model: tblDeal;

    constructor(private busyService: BusyService,
        private router: Router,
        private route: ActivatedRoute,
        private questionService: QuestionService,
        private questionControlService: QuestionControlService,
        private dealService: DealService,
        private formBuilder: FormBuilder,
        private cd: ChangeDetectorRef) {
        this.isDataAvailable = false;
    }

    ngOnInit() {
        let self = this;
        this.model = self.dealService.currentDeal;
        self.pageTitle = "MMS Pre-Funding Information";

        self.pifQuestions = this.route.snapshot.data['pifQuestions'];
        if ((self.pifQuestions !== null) && (self.pifQuestions !== undefined)) {
            self.isDataAvailable = true;
            self.pifFormGroup = self.questionControlService.questionsToFormGroup(self.pifQuestions);
        }
    }
    
    onSubmit() {
        var isValid = this.pifFormGroup.valid;
        this.payLoad = JSON.stringify(this.pifFormGroup.value);
    }
}
