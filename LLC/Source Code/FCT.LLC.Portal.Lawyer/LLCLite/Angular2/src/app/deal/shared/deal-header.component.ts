import { Entity, FetchStrategySymbol, EntityManager, FetchStrategy, EntityType, EntityQuery, Predicate } from 'breeze-client';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

// TO DO: Add Deal Unit of work
import { DealUnitOfWork, IDeal } from '../deal-unit-of-work';
import { BusyService, CanComponentDeactivate } from '../../core/services/common';
import { tblDeal, tblNote } from '../../core/entities/EntityModel';
@Component({
    selector: "deal-header",
    templateUrl: './deal-header.component.html'
})
export class DealHeaderComponent implements OnInit, OnDestroy {
    deal: tblDeal;
    deals: tblDeal[];
    pageTitle: string;

    private savedOrRejectedSub: Subscription;

    constructor(private unitOfWork: DealUnitOfWork, private busyService: BusyService, private router: Router, private route: ActivatedRoute) {
        let self = this;
    }

    ngOnInit() {
        let self = this;
    }


    ngOnDestroy() {
    }

    clearMessage() {
    }

    onSelect(deal: IDeal) {
    }

    public searchDeal(ref: string) {

    }
}
