import { Component, inject } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AccountService } from '../../auth/account.service';
import { MatButton } from '@angular/material/button';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatMenu, MatMenuItem, MatMenuTrigger } from '@angular/material/menu';
import { MatDivider } from '@angular/material/divider';

@Component({
  selector: 'app-header',
  imports: [
    MatIcon,
    CommonModule,
    MatButton,
    MatMenuTrigger,
    MatMenu,
    MatMenuItem
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  accountService = inject(AccountService);

  constructor(private router: Router) {}

  navigateToProfile(): void {
    this.router.navigate(['/profile']);
  }

  navigateToLoginUser(): void {
    this.router.navigate(['/account/login-user']);
  }

  navigateToRegisterUser(): void {
    this.router.navigate(['/account/register-user']);
  }

  logout() {
    this.accountService.logout().subscribe({
      next: () => {
        this.accountService.currentUser.set(null);
        this.router.navigateByUrl('/account/login-user');
      }
    });
  }
}
