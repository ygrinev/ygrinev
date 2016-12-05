import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgaModule } from '../../theme/nga.module';
import { AccordionModule } from 'ng2-bootstrap/components/accordion';

import { DealSummary } from './summary.component';
import { routing } from './summary.routing';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        NgaModule,
        AccordionModule,
        routing
    ],
    declarations: [
        DealSummary
    ],
    providers: [
    ]
})
export class DealSummaryModule { }
