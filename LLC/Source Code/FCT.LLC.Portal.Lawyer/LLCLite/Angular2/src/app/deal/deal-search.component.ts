
import { Entity, FetchStrategySymbol, EntityManager, FetchStrategy, EntityType, EntityQuery, Predicate} from 'breeze-client';

import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { DashboardComponent } from '../shared/controls/dashboard.component';
import { CollapsableBoxComponent } from '../shared/controls/collapsable-box.component';
// TO DO: Add Deal Unit of work
import { DealUnitOfWork, IDeal } from './deal-unit-of-work';
import { BusyService, CanComponentDeactivate } from '../core/services/common';
import { tblDeal, tblNote } from '../core/entities/EntityModel';
@Component({
    templateUrl: './deal-search.component.html'
})
export class DealSearchComponent implements OnInit, OnDestroy {
    activeDeal: IDeal;
    dealId: string;
    deal: tblDeal;
    deals: tblDeal[];
    pageTitle: string;
    searchText: string;
    selectedNote: tblNote;
    noDeals: boolean;

    private savedOrRejectedSub: Subscription;

    constructor(private unitOfWork: DealUnitOfWork, private busyService: BusyService, private router: Router, private route: ActivatedRoute) {
        let self = this;
    }

    ngOnInit() {
        let self = this;


        self.pageTitle = "LLC Deal Search";

        if (self.route.firstChild) {
            self.route.firstChild.params.forEach(params => {
                self.dealId = params['id'];
            });

        }
        self.savedOrRejectedSub = DealUnitOfWork.savedOrRejected.subscribe(args => {
        });
    }

    isInputValid(test: string) : boolean {
        return test != null && test != '' && !isNaN(Number(test))&&test.length==11 ;
    }

    numeric(test: string) : boolean {
        return test == null || test == '' || !isNaN(Number(test));
    }


    ngOnDestroy() {
        let self = this;
        self.savedOrRejectedSub.unsubscribe();
    }

    clearMessage() {
        let self = this;
        self.noDeals = false;
    }

    onSelect(deal: IDeal) {
       let self = this;
       self.router.navigate(['/deal', deal.id]);
    }

    public searchDeal(ref: string) {
        let self = this;
        self.router.navigate(['/deal', ref])

    }
}
