import { Component, ViewEncapsulation } from '@angular/core';

@Component({
    selector: 'deal-pif',
    encapsulation: ViewEncapsulation.None,
    styles: [require('./pif.scss')],
    template: require('./pif.html')
})
export class Pif {

    constructor() {
    }
}
