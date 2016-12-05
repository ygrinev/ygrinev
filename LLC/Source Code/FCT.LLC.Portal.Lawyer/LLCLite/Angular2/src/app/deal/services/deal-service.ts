import { Injectable } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { tblDeal } from '../../core/entities/EntityModel';
import { DealDocument, DealDocuments } from '../../core/dto/DtoModel';
import { DealUnitOfWork } from '../deal-unit-of-work';

@Injectable()
export class DealService {

    currentDeal: tblDeal;
    currentDealDocuments: DealDocuments;

    constructor(private unitOfWork: DealUnitOfWork, private route: ActivatedRoute) { }

    getCurrentDeal(): tblDeal {
        if ((this.currentDeal == null) || (this.currentDeal === undefined)) {
            this.route.params.forEach(params => {
                let fctURN = params['id'];
            });
            let fctRefNum = this.route.snapshot.params['id'];
            this.getDealByFCTURN(fctRefNum);
        }
        return this.currentDeal;
    }

    setCurrentDeal(deal: tblDeal) {
        this.currentDeal = deal;
    }

    setDealDocuments(documents: DealDocuments) {
        this.currentDealDocuments = documents;
    }

    getDealByFCTURN(fctRefNum: string) {
        let self = this;
        this.unitOfWork.dealRepository.getDealByFCTURN(fctRefNum)
            .then(data => {
                if (data.length > 0) {
                    self.currentDeal = data[0];
                }
                return;
            });
    }
}