import { RouterModule, Routes } from '@angular/router';

import { PrepareGuard, CanDeactivateGuard } from './core/services/common';

import { LoginComponent } from './login.component';
import { DealSearchComponent } from './deal/deal-search.component';
import { DealComponent } from './deal/deal.component';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'deal-search',
        pathMatch: 'full'
    },
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'deal-search',
        component: DealSearchComponent,
        canActivate: [PrepareGuard]
    },
    {
        path: 'deal/:id',
        component: DealComponent,
        canActivate: [PrepareGuard]
    }

];

export const routing = RouterModule.forRoot(routes, { enableTracing: true, useHash: true });