import { Component, ViewEncapsulation } from '@angular/core';

@Component({
    selector: 'deal-notes',
    encapsulation: ViewEncapsulation.None,
    styles: [require('./notes.scss')],
    template: require('./notes.html')
})
export class Notes {

    constructor() {
    }
}
