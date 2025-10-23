import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-test-errors',
  imports: [],
  templateUrl: './test-errors.html',
  styleUrl: './test-errors.css'
})
export class TestErrors {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  validationErrors = signal<string[]>([]);

  get400ValidationError(): void {
    this.http.post(this.baseUrl + "account/register", {}).subscribe({
      next: response => console.log(response),
      error: error => {
        console.log(error);
        this.validationErrors.set(error);
      }
    })
  }

  get400Error(): void {
    this.http.get(this.baseUrl + "error/bad-request").subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get401Error(): void {
    this.http.get(this.baseUrl + "error/auth").subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get404Error(): void {
    this.http.get(this.baseUrl + "error/not-found").subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get500Error(): void {
    this.http.get(this.baseUrl + "error/server-error").subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }
}
