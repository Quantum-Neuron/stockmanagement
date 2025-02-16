import { CommonModule, JsonPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { AccountService } from '../auth/account.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-register-user',
  imports: [
    CommonModule, 
    ReactiveFormsModule,
    MatCard,
    MatFormField,
    MatLabel,
    MatInput,
    MatButton,
    MatError,
  ],
  templateUrl: './register-user.component.html',
  styleUrl: './register-user.component.css'
})
export class RegisterUserComponent {
  registerForm: FormGroup;
  validationErrors: string[] = [];
  matSnackBar = inject(MatSnackBar);

  constructor(private fb: FormBuilder, private accountService: AccountService, private router: Router ) {
    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    });
  }

  onSubmit(): void {
    this.accountService.register(this.registerForm.value).subscribe({
      next: () => {
        this.router.navigateByUrl('account/login-user');
      },
      error: () => {
        this.matSnackBar.open('An error occurred while registering', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          horizontalPosition: 'right'
        });
      }
    })
  }

  passwordsMatch(): boolean {
    const password = this.registerForm.controls['password'].value;
    const confirmPassword = this.registerForm.controls['confirmPassword'].value;
    return password === confirmPassword;
  }
}
