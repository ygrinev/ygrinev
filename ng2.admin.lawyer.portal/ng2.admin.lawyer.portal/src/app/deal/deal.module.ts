import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AccordionModule } from "ng2-accordion";

import { routing } from './deal.routing';
import { NgaModule } from '../theme/nga.module';

import { Deal } from './deal.component';
import { DealSummary } from './summary/summary.component';
import { DealHistoryService } from './history/history.service';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        NgaModule,
        AccordionModule,
        routing
    ],
    declarations: [
        Deal,
        DealSummary
    ],
    providers: [
        DealHistoryService
    ]
})
export class DealModule {
}
