import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class GlobalState {

    private _data = new Subject<Object>();
    private _dataStream$ = this._data.asObservable();

    private _subscriptions: Map<string, Array<Function>> = new Map<string, Array<Function>>();

    constructor() {
        this._dataStream$.subscribe((data) => this._onEvent(data));
    }

    notifyDataChanged(event, value) {

        let current = this._data[event];
        if (current !== value) {
            this._data[event] = value;

            this._data.next({
                event: event,
                data: this._data[event]
            });
        }
    }

    subscribe(event: string, callback: Function) {
        let subscribers = this._subscriptions.get(event) || [];
        subscribers.push(callback);

        this._subscriptions.set(event, subscribers);
    }

    _onEvent(data: any) {
        let subscribers = this._subscriptions.get(data['event']) || [];

        subscribers.forEach((callback) => {
            callback.call(null, data['data']);
        });
    }
}


//How To Use It:
//1- Adding import {GlobalState} from '[PATH]/global.state';
//2- Injecting to constructor
//             constructor(private _state:GlobalState [,...]) {
//              .....
//             }
//3- Using _state table based on your requirements
//this._state.subscribe(event , callback);
//this._state.notifyDataChanged(event, value);
//this._state._onEvent(data);