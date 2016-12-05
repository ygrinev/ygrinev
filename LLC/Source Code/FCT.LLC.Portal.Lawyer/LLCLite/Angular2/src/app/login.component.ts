import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { DashboardComponent } from './shared/controls/dashboard.component';
import { AuthService} from './core/services/common';

@Component({
    templateUrl: './login.html'
})
export class LoginComponent {

    username: string = 'Admin';
    password: string = 'password';

    constructor(private authService: AuthService, private router: Router) { }

    loginUser() {
        if (!this.isValid) return;

        this.authService.loginUser(this.username, this.password)
            .then(() => {
                this.router.navigateByUrl(this.authService.redirectUrl || '/');
            }).catch(e => {
                if (e.status == 401) {
                    // Do something
                }

                // Something else happened
                throw e;
            });
    }

    get isValid(): boolean {
        return !!this.username && !!this.password;
    }
}