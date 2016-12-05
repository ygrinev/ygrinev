import { Routes, RouterModule }  from '@angular/router';

import { Srot } from './srot.component';

// noinspection TypeScriptValidateTypes
const routes: Routes = [
  {
    path: '',
    component: Srot,
    children: [
    ]
  }
];

export const routing = RouterModule.forChild(routes);
