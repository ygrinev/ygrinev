import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { LoadingAnimateService } from 'ng2-loading-animate';
import { BusyService, CanComponentDeactivate } from '../../core/services/common';
import { CollapsableBoxComponent } from '../../shared/controls/collapsable-box.component';
import { DashboardComponent } from '../../shared/controls/dashboard.component';

import { tblDeal, tblDealHistory } from '../../core/entities/EntityModel';
import { DealUnitOfWork } from '../deal-unit-of-work';
import { DealService } from '../services/deal-service';
import { Entity, FetchStrategySymbol, EntityManager, FetchStrategy, EntityType, EntityQuery, Predicate } from 'breeze-client';
import { DealHistoryRepository } from '../deal-repository';

@Component({
    templateUrl: './history.html',
    providers: [
        DealUnitOfWork,
    ]
})
export class HistoryComponent implements OnInit {

    model: tblDeal;
    dealId: number;
    //lenderCode: string;
    dealHistories: tblDealHistory[];
    dealUnitOfWork: DealUnitOfWork;
    ascendingUser: boolean = true;
    ascendingActivity: boolean = true;
    ascendingDate: boolean = true;
    rowCount: number = 0;

    constructor(private unitOfWork: DealUnitOfWork,
                private dealService: DealService,
                private _loadingSvc: LoadingAnimateService,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        let self = this;
        self.start();

        self.route.params.forEach(params => {
            let id = params['id'];
            this.dealId = id;
            self.getDealById(id);
        });
    }

    setRowBackground(lastCol: boolean) {
        let self = this;

        if (this.rowCount % 2 == 0) {
            if (lastCol == true) {
                self.rowCount++;
            }
            return "none";
        } else {
            if (lastCol == true) {
                self.rowCount++;
            }
            return "#c0c0c0";
        }
    }

    getDealById(dealId: number) {
        console.log(dealId);
        let self = this;
        this.unitOfWork.dealRepository.getDealByDealID(dealId)
            .then(data => {
                if (data.length > 0) {
                    self.model = data[0];
                    self.dealService.setCurrentDeal(data[0]);
                    self.dealHistories = data[0].tblDealHistory.sort(function (history2, history1) {
                        if (history1.LogDate < history2.LogDate) {
                            return -1;
                        } else if (history1.LogDate > history2.LogDate) {
                            return 1;
                        } else {
                            return 0;
                        }
                    });
                    this.ascendingDate = true;
                    //self.lenderCode = data[0].tblLender.LenderCode;
                    self.stop();
                }
                return data;
            });
    }

    sortDealHistoryByUser() {
        this.rowCount = 0;
        if (this.ascendingUser == true) {
            this.dealHistories.sort(function (history1, history2) {
                if (history1.UserType.toLowerCase() < history2.UserType.toLowerCase()) {
                    return -1;
                } else if (history1.UserType.toLowerCase() > history2.UserType.toLowerCase()) {
                    return 1;
                } else {
                    return 0;
                }
            });
            this.ascendingUser = false;
        } else {
            this.dealHistories.sort(function (history2, history1) {
                if (history1.UserType.toLowerCase() < history2.UserType.toLowerCase()) {
                    return -1;
                } else if (history1.UserType.toLowerCase() > history2.UserType.toLowerCase()) {
                    return 1;
                } else {
                    return 0;
                }
            });
            this.ascendingUser = true;
        }
    }

    sortDealHistoryByActivity() {
        this.rowCount = 0;
        if (this.ascendingActivity == true) {
            this.dealHistories.sort(function (history1, history2) {
                if (history1.Activity.toLowerCase() < history2.Activity.toLowerCase()) {
                    return -1;
                } else if (history1.Activity.toLowerCase() > history2.Activity.toLowerCase()) {
                    return 1;
                } else {
                    return 0;
                }
            });
            this.ascendingActivity = false;
        } else {
            this.dealHistories.sort(function (history2, history1) {
                if (history1.Activity.toLowerCase() < history2.Activity.toLowerCase()) {
                    return -1;
                } else if (history1.Activity.toLowerCase() > history2.Activity.toLowerCase()) {
                    return 1;
                } else {
                    return 0;
                }
            });
            this.ascendingActivity = true;
        }
    }

    sortDealHistoryByDate() {
        this.rowCount = 0;
        if (this.ascendingDate == true) {
            this.dealHistories.sort(function (history1, history2) {
                if (history1.LogDate < history2.LogDate) {
                    return -1;
                } else if (history1.LogDate > history2.LogDate) {
                    return 1;
                } else {
                    return 0;
                }
            });
            this.ascendingDate = false;
        } else {
            this.dealHistories.sort(function (history2, history1) {
                if (history1.LogDate < history2.LogDate) {
                    return -1;
                } else if (history1.LogDate > history2.LogDate) {
                    return 1;
                } else {
                    return 0;
                }
            });
            this.ascendingDate = true;
        }
    }

    setUserName(entity: tblDealHistory) {
        if (entity.UserID.toLowerCase().trim() == 'system' && entity.UserType.toLowerCase().trim() == 'lender') {
            return this.model.tblLender.LenderCode + " User";
        }
        else {
            return entity.UserID;
        }
    }

    start() {
        this._loadingSvc.setValue(true);
    }

    stop() {
        this._loadingSvc.setValue(false);
    }
}