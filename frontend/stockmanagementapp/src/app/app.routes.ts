import { Routes } from '@angular/router';
import { AuthComponent } from './auth/auth.component';
import { MainFeedComponent } from './main-feed/main-feed.component';

export const routes: Routes = [
    { path: 'login', component: AuthComponent },
    { path: 'main-feed', component: MainFeedComponent }
];
