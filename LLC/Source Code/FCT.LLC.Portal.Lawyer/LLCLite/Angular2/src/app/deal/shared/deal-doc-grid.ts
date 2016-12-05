import { Component, ContentChildren, QueryList, EventEmitter, Input, Output } from '@angular/core';
import { DealDocument } from '../../core/dto/DtoModel';
import { BoxComponent } from '../../shared/controls/box.component';
@Component({
    selector: 'deal-doc-grid',
    templateUrl: './deal-doc-grid.html',
    styles: [`
        a.disabled {
            color: lightgray;
            cursor: not-allowed;
        }
        tr.selected > td {
            color: white;
            background-color: rgb(120,200,240) !important;
            font-weight: bold;
        }
    `],
})
export class DealDocGridComponent {

    // add next line to any component that hosts a TabContainer in order to make the tabContainer available internally
    // only needed if you actually need to access that tabContainer programatically.
    //    @ViewChild(TabContainer) tabContainer: TabContainer;

    @Input() title: string;
    @Input() documents: DealDocument[];
    @Input() showRegeneration: boolean = false;
    @Input() showSubmit: boolean = false;
    @Input() showUpload: boolean = false;
    @Input() showDownload: boolean = false;
    @Input() showDateModified: boolean = false;
    @Input() showDatePublished: boolean = false;
    @Input() showPublishToOther: boolean = false;
    @Input() showStatus: boolean = false;

    @Input() DescriptionColumnWidth: number = 55;
    @Input() TypeColumnWidth: number = 6;
    @Input() DateModifiedColumnWidth: number = 15;
    @Input() DatePublishedColumnWidth: number = 15;
    @Input() StatusColumnWidth: number = 9;

    getTotalColumnWidth(): number {
        let totalWidth = 0;
        totalWidth = this.DescriptionColumnWidth + this.TypeColumnWidth;
        if (this.showDateModified == true) {
            totalWidth += this.DateModifiedColumnWidth;
        }
        if (this.showStatus == true) {
            totalWidth += this.StatusColumnWidth;
        }
        if (this.showDatePublished == true) {
            totalWidth += this.DatePublishedColumnWidth;
        }
        return totalWidth;

    }
    
    selectedDocument: DealDocument;

    onClickDocument(doc: DealDocument): void {
        this.selectedDocument = doc;
    }
}

