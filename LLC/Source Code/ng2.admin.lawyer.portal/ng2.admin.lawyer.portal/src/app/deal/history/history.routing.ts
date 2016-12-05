import { Routes, RouterModule }  from '@angular/router';

import { DealHistory } from './history.component';

// noinspection TypeScriptValidateTypes
const routes: Routes = [
  {
    path: '',
    component: DealHistory,
    children: [
    ]
  }
];

export const routing = RouterModule.forChild(routes);
