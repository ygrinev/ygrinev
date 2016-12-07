import { Routes, RouterModule }  from '@angular/router';

import { Pif } from './pif.component';

// noinspection TypeScriptValidateTypes
const routes: Routes = [
  {
    path: '',
    component: Pif,
    children: [
    ]
  }
];

export const routing = RouterModule.forChild(routes);
