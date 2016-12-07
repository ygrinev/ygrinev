import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgaModule } from '../../theme/nga.module';

import { Ng2SmartTableModule } from 'ng2-smart-table';
import { DealHistory } from './history.component';
import { routing } from './history.routing';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        Ng2SmartTableModule,
        NgaModule,
        routing
    ],
    declarations: [
        DealHistory
    ],
    providers: [
        DatePipe
    ]
})
export default class DealHistoryModule { }
