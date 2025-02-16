import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatCard } from '@angular/material/card';
import { MatButton } from '@angular/material/button';
import { MatInput } from '@angular/material/input';
import { AccountService } from '../auth/account.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormField,
    MatLabel,
    MatButton,
    MatCard,
    MatInput,
    RouterModule,
  ],
  templateUrl: './login-user.component.html',
  styleUrls: ['./login-user.component.css']
})
export class LoginUserComponent {
  formBuilder = inject(FormBuilder);
  accountService = inject(AccountService);
  router = inject(Router);
  private activatedRoute = inject(ActivatedRoute);
  returnUrl = "/account/login-user";
  matSnackBar = inject(MatSnackBar);

  constructor() {
    const url = this.activatedRoute.snapshot.queryParams['returnUrl'];
    if (url) {
      this.returnUrl = url;
    }
  }

  loginForm = this.formBuilder.group({
    email: [''],
    password: ['']
  });

  onSubmit(): void {
    this.accountService.login(this.loginForm.value).subscribe({
      next: () => {
        this.accountService.getUserInfo().subscribe();
        this.router.navigateByUrl('/main-feed');
      }, error: () => {
        this.matSnackBar.open('Wrong Credentials, Please Try Again.', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          horizontalPosition: 'right',
        })
      }
    })
  }

  validateForm(): void {
    Object.keys(this.loginForm.controls).forEach(field => {
      const control = this.loginForm.get(field);
      control?.markAsTouched({ onlySelf: true });
    });
  }

  navigateToRegister(): void {
    this.router.navigate(['/register-user']);
  }
}