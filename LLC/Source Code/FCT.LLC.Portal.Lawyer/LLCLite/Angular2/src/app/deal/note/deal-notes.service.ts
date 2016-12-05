import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { INote} from './note';

@Injectable()
export class NoteService {
    private _dealSvcUrl = 'http://apppridevsg19.prefirstcdn.com/LLCDealService/DealService.svc/' //http://localhost/LLCDealService/DealService.svc/';
    public _notes: INote[] = null;
    constructor(private _http: Http) { 
        console.log('\n===================>  DealService Created!!!');
    }

    getNotes(fctUrn: string): Observable<INote[]> {
        // return this.getDeal(id)
        //     .map((deal: IDeal) => <INote[]>deal.notes);
        console.log('\n******************* Notes Number: ' + this._notes == null ? this._notes.length : '0');
        console.log('fctUrn: ' +  fctUrn);
        return this._http.get(this._dealSvcUrl + 'GetNotes/' + fctUrn)
        .map((response: Response) => <INote[]> response.json())
        .do(data => console.log('All: ' +  JSON.stringify(data)))
        .catch(this.handleError);
}

    private handleError(error: Response) {
        // in a real world app, we may send the server to some remote logging infrastructure
        // instead of just logging it to the console
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}
