import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
// TO DO: import components here

import { DealSearchComponent } from './deal-search.component';
import { DealComponent } from './deal.component';
import { DealDocumentComponent } from './document/deal-document.component';
import { DealNoteListComponent } from './note/deal-note-list.component';
import { DealHeaderComponent } from './shared/deal-header.component';
import { DealLeftNavComponent } from './shared/deal-left-nav.component';
import { DealDocGridComponent } from './shared/deal-doc-grid';

import { DealSummaryComponent } from './summary/deal-summary.component';

import { PropertyQuestionsComponent } from '../deal/pif/property-questions.component';
import { QuestionComponent } from '../deal/pif/question.component';
import { AnswerTypeComponent } from '../deal/pif/answertype.component';
import { PifQuestionResolver } from '../deal/pif/pif-question.resolver';
import { QuestionService } from '../deal/pif/question.service';
import { QuestionControlService } from '../deal/pif/question-control.service';

import { HistoryComponent } from '../deal/history/history.component';
import { DealUnitOfWork } from './deal-unit-of-work';
import { DealRepository, DealHistoryRepository } from './deal-repository';
import { DealService } from './services/deal-service';

// TO DO:  add routes
import { routing } from './deal.routes';
import { Ng2SmartTableModule } from 'ng2-smart-table';

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        FormsModule,
        ReactiveFormsModule,
        Ng2SmartTableModule,
        routing
    ],
    declarations: [
        DealSearchComponent,
        DealComponent,
        DealNoteListComponent,
        DealDocumentComponent,
        DealHeaderComponent,
        DealLeftNavComponent,
        DealDocGridComponent,
        HistoryComponent,
        DealSummaryComponent,
        PropertyQuestionsComponent,
        QuestionComponent,
        AnswerTypeComponent
    ],
    providers: [
        DealUnitOfWork,
        DealRepository,
        DealHistoryRepository,
        DealService,  // The UoW used for the module
        PifQuestionResolver,
        QuestionService,
        QuestionControlService,
    ]
})
export class DealModule { }
