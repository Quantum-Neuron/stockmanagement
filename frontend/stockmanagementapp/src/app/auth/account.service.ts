import { inject, Injectable, signal } from "@angular/core";
import { environment } from "../../environments/environment";
import { HttpClient, HttpParams } from "@angular/common/http";
import { User } from "./user";
import { catchError, map, throwError } from "rxjs";
import { MatSnackBar } from "@angular/material/snack-bar";

@Injectable({
    providedIn: 'root'
})
export class AccountService {
    baseUrl = environment.apiUrl;
    private http = inject(HttpClient);
    currentUser = signal<User | null>(null);
    private snackBar = inject(MatSnackBar);

    login(values: any) {
        let params = new HttpParams();
        params = params.append('useCookies', true);

        return this.http.post<User>(this.baseUrl + '/login', values, { params, withCredentials: true });
    }

    register(values: any) {
        return this.http.post(this.baseUrl + '/user/register', values);
    }

    getUserInfo() {
        return this.http.get<User>(this.baseUrl + '/user/user-info').pipe(
            map(user => {
              this.currentUser.set(user);
              return user;  
            }),
            catchError(error => {
                this.snackBar.open('An error occurred while fetching user info', 'Close', {
                    duration: 3000,
                    verticalPosition: 'top',
                    horizontalPosition: 'right'
                });
                return throwError(error);
            })
        )
    }

    logout() {
        return this.http.post(this.baseUrl + '/user/logout', {});
    }

    getAuthState() {
        return this.http.get<{isAuthenticated: boolean}>(this.baseUrl + '/user/auth-status');
    }
}