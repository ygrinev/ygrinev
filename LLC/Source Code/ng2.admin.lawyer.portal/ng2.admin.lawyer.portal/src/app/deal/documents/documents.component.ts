import { Component, ViewEncapsulation } from '@angular/core';

@Component({
    selector: 'deal-documents',
    encapsulation: ViewEncapsulation.None,
    styles: [require('./documents.scss')],
    template: require('./documents.html')
})
export class Documents {

    constructor() {
    }
}
