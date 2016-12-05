import { Entity, FetchStrategySymbol, EntityManager, FetchStrategy, EntityType, EntityQuery, Predicate} from 'breeze-client';

import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

// TO DO: Add Deal Unit of work
import { DealUnitOfWork, IDeal } from '../deal-unit-of-work';
import { BusyService, CanComponentDeactivate } from '../../core/services/common';
import { tblDeal, tblNote} from '../../core/entities/EntityModel';
import { OnChanges, Input, Output, EventEmitter} from "@angular/core";

@Component({
    selector: "llc-notes",
    templateUrl: './deal-note-list.component.html'
})
export class DealNoteListComponent implements OnInit, OnDestroy {
    @Input() deal: tblDeal;
    dealId: number;
    pageTitle: string;
    notes: tblNote[];
    selectedNote: number;
    newNoteText: string;
    model: tblNote;

    private savedOrRejectedSub: Subscription;

    constructor(private unitOfWork: DealUnitOfWork, private busyService: BusyService, private router: Router, private route: ActivatedRoute) { }
    selectNote() {

    }
    ngOnInit() {
        let self = this;
        self.notes = self.deal.tblNotes;
        self.dealId = self.deal.DealID;

        self.pageTitle = "LLC Notes";

        self.savedOrRejectedSub = DealUnitOfWork.savedOrRejected.subscribe(args => {

        });
    }
    public setDeal (newModelValue:tblDeal) {
        this.deal = newModelValue;
    }
    
    addNote() {
        let self = this;
        let note = self.deal.addNote(self.newNoteText, self.dealId);
        note.NoteType = "0";
        this.unitOfWork.commit();

    }
    ngOnDestroy() {
        let self = this;
        self.savedOrRejectedSub.unsubscribe();
    }

    get canSave(): boolean {
        let self = this;
        return self.unitOfWork.hasChanges();
    }

    save(suppressConfirmation: boolean) {
        let self = this;
        return self.busyService.busy(self.unitOfWork.commit()).then(() => {
            if (suppressConfirmation) return;
        });
    }

    cancel() {
        let self = this;
        self.unitOfWork.rollback();
    }
}