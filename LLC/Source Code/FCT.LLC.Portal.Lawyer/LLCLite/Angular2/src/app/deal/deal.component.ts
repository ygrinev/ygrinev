import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { DealNoteListComponent } from './note/deal-note-list.component';

import { PropertyQuestionsComponent } from './pif/property-questions.component';

import { DealSummaryComponent } from './summary/deal-summary.component';
import { BusyService, CanComponentDeactivate } from '../core/services/common';
import { CollapsableBoxComponent } from '../shared/controls/collapsable-box.component';
import { DashboardComponent } from '../shared/controls/dashboard.component';

import { tblDeal } from '../core/entities/EntityModel';
import { DealUnitOfWork } from './deal-unit-of-work';
import { DealService } from './services/deal-service';

import { Entity, FetchStrategySymbol, EntityManager, FetchStrategy, EntityType, EntityQuery, Predicate} from 'breeze-client';
import { DealRepository } from './deal-repository';

@Component({
    templateUrl: './deal.component.html',
    providers: [
        DealUnitOfWork
    ]
})
export class DealComponent implements OnInit{

    model: tblDeal;
    dealUnitOfWork: DealUnitOfWork;

    constructor(private unitOfWork: DealUnitOfWork, private dealService: DealService, private busyService: BusyService, private route: ActivatedRoute) { }

    ngOnInit() {
        let self = this;

        self.route.params.forEach(params => {
            let fctURN = params['id'];
            self.getDealByFCTURN(fctURN);
        });
    }

    getDealByFCTURN(fctRefNum: string) {
        console.log(fctRefNum);
        let self = this;
        return this.busyService.busy(this.unitOfWork.dealRepository.getDealByFCTURN(fctRefNum)
            .then(data => {
                if (data.length > 0) {
                    self.model = data[0];
                    this.dealService.setCurrentDeal(data[0]);
                }
                return data;
            }));
    }

    getDealByDealID(dealId: number) {
        console.log(dealId);
        let self = this;
        return this.busyService.busy(this.unitOfWork.dealRepository.getDealByDealID(dealId)
            .then(data => {
                if (data.length > 0) {
                    self.model = data[0];
                    this.dealService.setCurrentDeal(data[0]);
                }
                return data;
            }));
    }

    canDeactivate(): boolean | Observable<boolean> | Promise<boolean> {
        let self = this;
        if (!self.unitOfWork.hasChanges()) return true;

        let title = 'Confirm';
        let message = 'You have unsaved changes. Would you like to save?';
        let buttons = ['Yes', 'No', 'Cancel'];

        return true;
    }

    get canSave(): boolean {
        let self = this;
        return self.unitOfWork.hasChanges();
    }

    save(suppressConfirmation: boolean) {
        let self = this;
        return self.busyService.busy(this.unitOfWork.commit()).then(() => {
            if (suppressConfirmation) return;

            return true;
        });
    }

    cancel() {
        let self = this;
        self.unitOfWork.rollback();
    }

}