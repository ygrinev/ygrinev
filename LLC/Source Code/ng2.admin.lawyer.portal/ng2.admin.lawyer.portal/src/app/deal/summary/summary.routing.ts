import { Routes, RouterModule }  from '@angular/router';

import { DealSummary } from './summary.component';

// noinspection TypeScriptValidateTypes
const routes: Routes = [
  {
    path: '',
    component: DealSummary,
    children: [
    ]
  }
];

export const routing = RouterModule.forChild(routes);
