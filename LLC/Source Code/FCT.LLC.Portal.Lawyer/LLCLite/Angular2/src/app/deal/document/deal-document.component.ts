import { Entity, FetchStrategySymbol, EntityManager, FetchStrategy, EntityType, EntityQuery, Predicate} from 'breeze-client';

import { OnChanges, Input, Output, EventEmitter, Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { Headers, Http } from '@angular/http';

import { DealUnitOfWork, IDeal } from '../deal-unit-of-work';
import { BusyService, CanComponentDeactivate } from '../../core/services/common';
import { DealService } from '../../deal/services/deal-service';
import { BoxComponent } from '../../shared/controls/box.component';
import { DealDocGridComponent } from '../shared/deal-doc-grid';
import { DashboardComponent } from '../../shared/controls/dashboard.component';
import { tblDeal, tblNote } from '../../core/entities/EntityModel';
import { DealDocuments, DealDocument, DealDocList } from '../../core/dto/DtoModel';

@Component({
    selector: "deal-document",
    templateUrl: './deal-document.component.html',
    styles: [``],
    providers: [DealUnitOfWork]
})
export class DealDocumentComponent implements OnInit, OnDestroy {
    model: tblDeal;
    documents: DealDocuments;
    docLists: DealDocList[];

    constructor(private http: Http,
        private unitOfWork: DealUnitOfWork,
        private dealService: DealService,
        private busyService: BusyService,
        private router: Router,
        private route: ActivatedRoute) { }

    getDealDocuments(fctRefNum: string, langID: number) {
        console.log("FCTURN: " + fctRefNum + "langID: " + langID);
        let self = this;
        var body = "{ \"FCTURN\": " + fctRefNum + ", \"langID\": 0 }";
        var headers = new Headers();
        headers.append('Content-Type', 'application/json; charset=utf-8');

        self.http
            .post('http://localhost/LLCWebApi/breeze/Document/GetDealDocuments', body, { headers: headers })
            .map(response => response.json())   
            .subscribe(
            response => { this.documents = response['dealDocuments']; this.initDocLists(this.documents); },
            self.logError,
            () => console.log("SUCCESS! \n\nData:" + JSON.stringify(this.documents))
            );        

        this.unitOfWork.dealRepository.getDealByFCTURN(fctRefNum)
            .then(data => {
                if (data.length > 0) {
                    self.model = data[0];
                }
            });
    }
    initDocLists(documents: DealDocuments): void {
        this.docLists = [
            {
                title: "Documents Posted by Lender:",
                docs: documents.lenderDocs,
                showDateModified: true,
                showDatePublished: false,
                showPublishToOther: false,
                showDownload: true,
                showRegenerate: false,
                showStatus: false,
                showSubmit: false,
                showUpload: false
            },
            {
                title: "Documents Created/Uploaded by Lawyer/Notary:",
                docs: documents.lawyerDocs,
                showDateModified: true,
                showDatePublished: true,
                showPublishToOther: true,
                showDownload: true,
                showRegenerate: true,
                showStatus: true,
                showSubmit: true,
                showUpload: true
            },

            {
                title: "Documents Submitted by Lawyer/Notary:",
                docs: documents.lawyerSubmittedDocs,
                showDateModified: true,
                showDatePublished: false,
                showPublishToOther: false,
                showDownload: false,
                showRegenerate: false,
                showStatus: false,
                showSubmit: false,
                showUpload: false
            },
            {
                title: "Documents Published by Other Lawyer/Notary:",
                docs: documents.lawyerPublishedDocs,
                showDateModified: false,
                showDatePublished: false,
                showPublishToOther: false,
                showDownload: false,
                showRegenerate: false,
                showStatus: false,
                showSubmit: false,
                showUpload: false
            },
            {
                title: "Documents Published by FCT:",
                docs: documents.fctDocs,
                showDateModified: false,
                showDatePublished: false,
                showPublishToOther: false,
                showDownload: false,
                showRegenerate: false,
                showStatus: false,
                showSubmit: false,
                showUpload: false
            }
        ];
    }

    logError(fctRefNum) {
        console.log(`ERROR: Failed to get documents for FCTURN=${fctRefNum}`);
    }
    ngOnInit() {
        let self = this;
        this.model = self.dealService.currentDeal;
        self.route.params.forEach(params => {
            let fctURN = params['id'];
            self.getDealDocuments(fctURN, 0);
        });
    }
    private savedOrRejectedSub: Subscription;

    ngOnDestroy() {
    }

    cancel() {
    }
}