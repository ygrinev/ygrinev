import { Component, ViewEncapsulation, Input } from '@angular/core';
import { DatePipe } from '@angular/common'; 

import { DealHistoryService } from './history.service';
import { LocalDataSource } from 'ng2-smart-table';

@Component({
    selector: 'deal-history',
    encapsulation: ViewEncapsulation.None,
    styles: [require('./history.scss')],
    template: require('./history.html')
})
export class DealHistory {

    query: string = '';

    settings = {
        hideSubHeader: true,
        actions: {
            add: false,
            edit: false,
            delete: false
        },
        columns: {
            Activity: {
                title: 'Activity',
                type: 'string'
            },
            UserID: {
                title: 'Username',
                type: 'string'
            },
            LogDate: {
                title: 'Date',
                valuePrepareFunction: (date) => {
                    var raw = new Date(date);
                    var formatted = this.datePipe.transform(raw, 'MMM dd, yyyy hh:mm a');
                    return formatted;
                }
            }
        }
    };

    source: LocalDataSource = new LocalDataSource();

    constructor(private service: DealHistoryService, private datePipe: DatePipe) {
        this.service.getData().then((data) => {
            this.source.load(data);
        });
    }
}
