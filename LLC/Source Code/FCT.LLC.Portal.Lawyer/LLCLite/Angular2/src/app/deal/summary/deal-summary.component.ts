import { Entity, FetchStrategySymbol, EntityManager, FetchStrategy, EntityType, EntityQuery, Predicate} from 'breeze-client';

import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

// TO DO: Add Deal Unit of work
import { DealUnitOfWork, IDeal } from '../deal-unit-of-work';
import { BusyService, CanComponentDeactivate } from '../../core/services/common';
import { tblDeal, tblMortgage, tblProperty } from '../../core/entities/EntityModel';
import { OnChanges, Input, Output, EventEmitter} from "@angular/core";

@Component({
    selector: "llc-deal-summary",
    templateUrl: './deal-summary.component.html'
})
export class DealSummaryComponent implements OnInit, OnDestroy, OnChanges {
    @Input() deal: tblDeal;
    dealId: number;
    pageTitle: string;
    mortgages: tblMortgage[];
    properties:  tblProperty[];
    model: tblDeal;

    constructor(private unitOfWork: DealUnitOfWork, private busyService: BusyService, private router: Router, private route: ActivatedRoute) { }

    ngOnInit() {

        let self = this;
        self.pageTitle = "LLC Summary";
    }
    ngOnChanges() {
        let self = this;

    }

    ngOnDestroy() {
        let self = this;
    }

}