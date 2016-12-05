import { Routes, RouterModule } from '@angular/router';
import { Deal } from './deal.component';

// noinspection TypeScriptValidateTypes
const routes: Routes = [
    {
        path: 'deal',
        component: Deal,
        children: [
            { path: '', redirectTo: 'history', pathMatch: 'full' },
            { path: 'documents', loadChildren: () => System.import('./documents/documents.module') },
            { path: 'pif', loadChildren: () => System.import('./pif/pif.module') },
            { path: 'history', loadChildren: () => System.import('./history/history.module') },
            { path: 'notes', loadChildren: () => System.import('./notes/notes.module') },
            { path: 'srot', loadChildren: () => System.import('./srot/srot.module') }
        ]
    }
];

export const routing = RouterModule.forChild(routes);
