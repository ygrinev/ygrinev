import { Component, ContentChildren, QueryList, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'dashboard',
    template: `
<div class="content-wrapper">
    <!-- Content Header (Page header) -->
    <!-- Main content -->
    <section class="content">
        <ng-content></ng-content>
    </section>
    <!-- /.content -->
</div>
    `
})
export class DashboardComponent {

    // add next line to any component that hosts a TabContainer in order to make the tabContainer available internally
    // only needed if you actually need to access that tabContainer programatically.
    //    @ViewChild(TabContainer) tabContainer: TabContainer;

}

