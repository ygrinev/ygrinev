import { Entity, FetchStrategySymbol, EntityManager, FetchStrategy, EntityType, EntityQuery, Predicate } from 'breeze-client';
import { Repository } from '../shared/services/repository';
import { tblDeal, tblDealHistory } from '../core/entities/EntityModel';
import { DealDocuments } from '../core/dto/DtoModel';

export class DealRepository extends Repository<tblDeal> {

    getDealByFCTURN(fcturn: string): Promise<tblDeal[]> {
        var query = EntityQuery.from(`deal/GetDealByFCTURN/?fcturn=${fcturn}`);
        return this.executeQuery(query);
    }

    getDealByDealID(dealId: number): Promise<tblDeal[]> {
        var query = EntityQuery.from(`deal/GetDealByDealID/?DealID=${dealId}`);
        return this.executeQuery(query);
    }
}

export class DealHistoryRepository extends Repository<tblDealHistory> {
    getDealHistory(dealId: number): Promise<tblDealHistory[]> {
        var query = EntityQuery.from(`dealHistory/GetDealHistories?DealID=${dealId}`);
        return this.executeQuery(query);
    }
}

export class DealDocumentsRepository extends Repository<DealDocuments> {
       //getDealDocuments(fctUrn: string, langID: number): Promise<DealDocuments[]> {
        //var query = EntityQuery.from('document/GetDealDocuments');
        //query.parameters = {
            //$method: 'POST',
            //$encoding: 'JSON',
            //$data: { FCTURN: fctUrn, langID: langID }
        //};
      //  return this.executeQuery(query);
   // }
}
