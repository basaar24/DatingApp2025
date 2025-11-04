import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastService } from '../../core/services/toast-service';
import { themes } from '../theme';

@Component({
  selector: 'app-nav',
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './nav.html',
  styleUrl: './nav.css'
})
export class Nav implements OnInit {
  private router = inject(Router);
  private toast = inject(ToastService);
  protected accountService = inject(AccountService);
  protected creds: any = {};
  protected selectedTheme = signal<string>(localStorage.getItem("theme") || "light");
  protected themes = themes;

  ngOnInit(): void {
    document.documentElement.setAttribute("data-theme", this.selectedTheme());
  }

  handleSelectedTheme(theme: string) {
    this.selectedTheme.set(theme);
    localStorage.setItem("theme", theme);
    document.documentElement.setAttribute("data-theme", theme);
    const elem = document.activeElement as HTMLDivElement;
    if (elem) {
      elem.blur();
    }
  }

  login(): void {
    this.accountService.login(this.creds).subscribe({
      next: response => {
        this.router.navigateByUrl("/members");
        this.creds = {};
        this.toast.success("Logged in!")
      },
      error: error => {
        this.toast.error(error.error);
      }
    });
  }

  logout(): void {
    this.accountService.logout();
    this.router.navigateByUrl("/");
  }
}
