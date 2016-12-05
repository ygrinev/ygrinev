import { Entity, FetchStrategySymbol, EntityManager, FetchStrategy, EntityType, EntityQuery, Predicate } from 'breeze-client';
import { Component, ElementRef, HostListener, ViewEncapsulation, AfterViewInit, OnInit, Input, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { GlobalState } from '../../global.state';
import { layoutSizes } from './theme.constants';

// TO DO: Add Deal Unit of work
import { DealUnitOfWork, IDeal } from '../deal-unit-of-work';
import { tblDeal, tblNote } from '../../core/entities/EntityModel';


@Component({
    selector: "deal-left-nav",
    encapsulation: ViewEncapsulation.None,
    templateUrl: './deal-left-nav.component.html'
})

export class DealLeftNavComponent implements OnInit, OnDestroy, AfterViewInit {

    public menuHeight: number;
    public isMenuCollapsed: boolean = false;
    public isMenuShouldCollapsed: boolean = false;

    @Input() deal: tblDeal;

    private savedOrRejectedSub: Subscription;

    constructor(private unitOfWork: DealUnitOfWork,
                private router: Router,
                private route: ActivatedRoute,
                private _elementRef: ElementRef,
                private _state: GlobalState) {
        this._state.subscribe('menu.isCollapsed', (isCollapsed) => {
            this.isMenuCollapsed = isCollapsed;
        });
    }

    ngOnInit() {
        if (this._shouldMenuCollapse()) {
            this.menuCollapse();
        }
    }

    ngAfterViewInit() {
        setTimeout(() => this.updateSidebarHeight());
    }

    ngOnDestroy() {
    }

    clearMessage() {
    }

    onSelect(deal: IDeal) {
    }

    public searchDeal(ref: string) {

    }

    public renderPifMenuItem(businessModel: string): boolean {
        var renderMenuItem = false;

        if ((businessModel.length > 0) && (businessModel.toLowerCase().indexOf("mms") >= 0)) {
            renderMenuItem = true;
        }
        return renderMenuItem;
    }

    @HostListener('window:resize')
    public onWindowResize(): void {

        var isMenuShouldCollapsed = this._shouldMenuCollapse();

        if (this.isMenuShouldCollapsed !== isMenuShouldCollapsed) {
            this.menuCollapseStateChange(isMenuShouldCollapsed);
        }
        this.isMenuShouldCollapsed = isMenuShouldCollapsed;
        this.updateSidebarHeight();
    }

    public menuExpand(): void {
        this.menuCollapseStateChange(false);
    }

    public menuCollapse(): void {
        this.menuCollapseStateChange(true);
    }

    public menuCollapseStateChange(isCollapsed: boolean): void {
        this.isMenuCollapsed = isCollapsed;
        this._state.notifyDataChanged('menu.isCollapsed', this.isMenuCollapsed);
    }

    public updateSidebarHeight(): void {
        this.menuHeight = this._elementRef.nativeElement.childNodes[0].clientHeight - 84;
    }

    private _shouldMenuCollapse(): boolean {
        return window.innerWidth <= layoutSizes.resWidthCollapseSidebar;
    }
}
