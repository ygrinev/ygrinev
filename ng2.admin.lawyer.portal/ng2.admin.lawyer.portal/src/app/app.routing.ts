import { Routes, RouterModule } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'deal', pathMatch: 'full' },
  { path: '**', redirectTo: 'deal/history' }
];

export const routing = RouterModule.forRoot(routes, { useHash: true });
