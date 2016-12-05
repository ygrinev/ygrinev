import { Routes, RouterModule }  from '@angular/router';

import { Notes } from './notes.component';

// noinspection TypeScriptValidateTypes
const routes: Routes = [
  {
    path: '',
    component: Notes,
    children: [
    ]
  }
];

export const routing = RouterModule.forChild(routes);
