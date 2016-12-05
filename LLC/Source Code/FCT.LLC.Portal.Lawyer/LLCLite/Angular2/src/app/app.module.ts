import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';
import { BreezeBridgeAngular2Module } from 'breeze-bridge-angular2';
import { LoadingAnimateModule, LoadingAnimateService } from 'ng2-loading-animate';

import { routing } from './app.routes';
import { GlobalState } from './global.state';
import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';
import { DealModule } from './deal/deal.module';

import { AppComponent } from './app.component';
import { LoginComponent } from './login.component';
import { DatePipe } from '@angular/common';

@NgModule({
    imports: [
        BreezeBridgeAngular2Module,
        BrowserModule,
        HttpModule,
        FormsModule,
        routing,
        LoadingAnimateModule.forRoot(),
        CoreModule,
        SharedModule,
        DealModule
    ],
    declarations: [
        AppComponent,
        LoginComponent
    ],
    bootstrap: [
        AppComponent
    ],
    providers: [
        GlobalState,
        LoadingAnimateService,
        DatePipe 
    ]
})
export class AppModule { }
