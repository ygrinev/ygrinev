import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NewLineDirective, ScrollToTopDirective } from './directives/common';
import { TabContainer } from './controls/tab-container.component';
import { TabPane } from './controls/tab-pane.directive';
import { CollapsableBoxComponent } from './controls/collapsable-box.component';
import { BoxComponent } from './controls/box.component';

import { DashboardComponent } from './controls/dashboard.component';
import { UnitOfWork } from './services/common';

@NgModule({
    imports: [CommonModule],
    declarations: [
        NewLineDirective,
        TabContainer,
        TabPane,
        ScrollToTopDirective,
        CollapsableBoxComponent,
        BoxComponent,
        DashboardComponent
    ],
    exports: [
        NewLineDirective,
        TabContainer,
        TabPane,
        ScrollToTopDirective,
        CollapsableBoxComponent,
        DashboardComponent
    ],
    providers: [UnitOfWork]
})
export class SharedModule { }