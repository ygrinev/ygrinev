import { Component, ContentChildren, QueryList, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'box',
    template: `
            <div class="row" style="min-height:80%">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-header with-border">
                            <h3 class="box-title">{{title}}</h3>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <ng-content></ng-content>
                                </div>
                                <!-- /.col -->
                            </div>
                            <!-- /.row -->
                        </div>
                        <!-- ./box-body -->
                        <div class="box-footer">
                            <div class="row">
                            </div>
                            <!-- /.row -->
                        </div>
                        <!-- /.box-footer -->
                    </div>
                    <!-- /.box -->
                </div>
                <!-- /.col -->
            </div>
    `
})
export class BoxComponent {

    // add next line to any component that hosts a TabContainer in order to make the tabContainer available internally
    // only needed if you actually need to access that tabContainer programatically.
    //    @ViewChild(TabContainer) tabContainer: TabContainer;

    @Input() title: string;
}

