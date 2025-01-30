import { Routes } from '@angular/router';
import { MainFeedComponent } from './main-feed/main-feed.component';
import { RegisterUserComponent } from './register-user/register-user.component';
import { LoginUserComponent } from './login-user/login-user.component';
import { AccountComponent } from './account/account.component';
import { authGuard } from './auth/auth.guard';

export const routes: Routes = [
    { path: 'main-feed', component: MainFeedComponent, canActivate: [authGuard] },
    { path: 'account/register-user', component: RegisterUserComponent },
    { path: 'account/login-user', component: LoginUserComponent },
    { path: 'account', component: AccountComponent },
    { path: '', redirectTo: '/account/login-user', pathMatch: 'full' }
];
