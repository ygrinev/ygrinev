import { Component, ViewEncapsulation } from '@angular/core';
import { AccordionModule } from "ng2-accordion";

@Component({
    selector: 'deal-summary',
    encapsulation: ViewEncapsulation.None,
    styles: [require('./summary.scss')],
    template: require('./summary.html'),
    providers: [AccordionModule]
})
export class DealSummary {

    constructor() {
    }
}
