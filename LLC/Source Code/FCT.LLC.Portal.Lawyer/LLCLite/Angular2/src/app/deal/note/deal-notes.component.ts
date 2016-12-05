import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { Subscription }       from 'rxjs/Subscription';

import { INote } from './note';
import { NoteService } from './deal-notes.service';

@Component({
    templateUrl: 'app/deal/note/deal-notes.component.html'
})
export class DealNotesComponent implements OnInit, OnDestroy {
    pageTitle: string = 'Deal Notes';
    fCTRefNum: string = '...';
    notes: INote[];
    errorMessage: string;
    private sub: Subscription;

    constructor(private _route: ActivatedRoute,
                private _router: Router,
                private _noteService: NoteService) {
    }

    ngOnInit(): void {
        this.sub = this._route.params.subscribe(
            params => {
                this.fCTRefNum = params['fCTRefNum'];
                this.getNotes(this.fCTRefNum);
        });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    getNotes(fctUrn: string) {
        this._noteService.getNotes(fctUrn).subscribe(
            notes => this.notes = notes,
            error => this.errorMessage = <any>error);
    }

    onBack(): void {
        this._router.navigate(['/deals']);
    }

    onCheck(note: INote): void {
        note.isActivated = !note.isActivated;
    }

}
