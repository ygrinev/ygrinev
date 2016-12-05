import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../shared/shared.module';

import {
    CanDeactivateGuard, PrepareGuard,
    ErrorHandler, EntityManagerProvider, customExceptionHandlerProvider,
    BusyService, AuthService
} from './services/common';

// ATTENTION: Never import this module into a lazy loaded module. Only import into app module.
@NgModule({
    imports: [CommonModule, SharedModule],
    providers: [
        CanDeactivateGuard,
        PrepareGuard,
        ErrorHandler,
        EntityManagerProvider,
        customExceptionHandlerProvider,
        BusyService,
        AuthService
    ]
})
export class CoreModule { }