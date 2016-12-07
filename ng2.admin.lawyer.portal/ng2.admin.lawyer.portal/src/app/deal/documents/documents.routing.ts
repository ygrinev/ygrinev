import { Routes, RouterModule }  from '@angular/router';

import { Documents } from './documents.component';

// noinspection TypeScriptValidateTypes
const routes: Routes = [
  {
    path: '',
    component: Documents,
    children: [
    ]
  }
];

export const routing = RouterModule.forChild(routes);
