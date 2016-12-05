import { Injectable } from '@angular/core'
import { Routes, RouterModule, Resolve, ActivatedRouteSnapshot } from '@angular/router';

import { DealSearchComponent } from './deal-search.component';
import { DealComponent } from './deal.component';
import { HistoryComponent } from './history/history.component';

import { DealNoteListComponent } from './note/deal-note-list.component';
import { PropertyQuestionsComponent } from '../deal/pif/property-questions.component';

import { DealDocumentComponent } from './document/deal-document.component';
import { PrepareGuard, CanDeactivateGuard } from '../core/services/common';

import { PifQuestionResolver } from '../deal/pif/pif-question.resolver';

export const dealRoutes: Routes = [
    {
        path: 'deal',
        component: DealSearchComponent,
        canActivate: [PrepareGuard]
    },
    {
        path: 'deal/:id',
        component: DealComponent,
        canActivate: [PrepareGuard]
    },
    {
        path: 'deal-history/:id',
        component:  HistoryComponent,
        canActivate: [PrepareGuard]
    },
    {
        path: 'deal-documents/:id',
        component: DealDocumentComponent,
        canActivate: [PrepareGuard]
    },
    {
        path: 'dealnotes',
        component: DealNoteListComponent,
        canActivate: [PrepareGuard],
        children: [
            {
                path: ':id',
                component: DealNoteListComponent,
                canActivate: [CanDeactivateGuard]
            }
        ]
    },
    {
        path: 'deal-prefunding/:id',
        component: PropertyQuestionsComponent,
        canActivate: [PrepareGuard],
        resolve: {
            pifQuestions: PifQuestionResolver,
        }

    }
];

export const routing = RouterModule.forChild(dealRoutes);