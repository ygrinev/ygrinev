import { Injectable } from '@angular/core';

import { EntityManagerProvider } from '../core/services/entity-manager-provider';
import { UnitOfWork, IRepository } from '../shared/services/common';
import { tblMortgage, tblProperty, tblDeal } from '../core/entities/EntityModel';
import { DealDocument, DealDocuments } from '../core/dto/DtoModel';
import { DealRepository, DealHistoryRepository, DealDocumentsRepository } from './deal-repository';

export interface IDeal {
    id: string;
    FCTRefNum: string;
    ClosedDate: Date;
    Created: Date;
    CreatedUser: string;
    Modified: Date;
    ModifyUser: string;
    RowVersion: number;
    Mortgages: tblMortgage[];
    Properties: tblProperty[];
}

@Injectable()
export class DealUnitOfWork extends UnitOfWork {

    dealItems: IRepository<IDeal>;
    deals: IRepository<tblDeal>;
    dealRepository: DealRepository;
    dealHistoryRepository: DealHistoryRepository;
    dealDocumentsRepository: DealDocumentsRepository;


    protected createDealRepository(entityTypeName: string, resourceName: string, isCached: boolean = false) {
        return new DealRepository(this.manager, entityTypeName, resourceName, isCached);
    }

    protected createDealHistoryRepository(entityTypeName: string, resourceName: string, isCached: boolean = false) {
        return new DealHistoryRepository(this.manager, entityTypeName, resourceName, isCached);
    }

    protected createDealDocumentsRepository(entityTypeName: string, resourceName: string, isCached: boolean = false) {
        return new DealDocumentsRepository(this.manager, entityTypeName, resourceName, isCached);
    }

    constructor(emProvider: EntityManagerProvider) {
        super(emProvider);
        let self = this;
        self.dealRepository = self.createDealRepository('tblDeal', 'deal/findDealByFCTURN');
        self.dealHistoryRepository = self.createDealHistoryRepository('tblDealHistory', '');
        self.dealDocumentsRepository = self.createDealDocumentsRepository('DealDocuments', 'document/GetDealDocuments');
    }

}