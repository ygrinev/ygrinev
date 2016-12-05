import { Component, OnInit, OnDestroy, OnChanges, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormArray, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { BusyService, CanComponentDeactivate } from '../../core/services/common';
import { tblQuestion } from "../../core/entities/EntityModel";
import { QuestionControlService } from './question-control.service';
import { Question } from './question';
import { AnswerTypeBase } from './answertype-base';

@Component({
    selector: 'pif-question',
    templateUrl: 'question.html',
    providers: [QuestionControlService]
})

// Responsible for a single question & and its answertypes
export class QuestionComponent implements OnInit {

    @Input() question: Question;
    @Input() form: FormGroup;

    constructor(private qcs: QuestionControlService) { }

    ngOnInit() {
    }
    
}