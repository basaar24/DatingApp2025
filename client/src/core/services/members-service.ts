import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../../types/member';
import { Observable } from 'rxjs';
import { AccountService } from './account-service';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;
  private accountService = inject(AccountService);

  getMember(id: string): Observable<Member> {
    return this.http.get<Member>(this.baseUrl + "members/" + id, this.getHttpOptions())
  }

  getMembers(): Observable<Member[]> {
    return this.http.get<Member[]>(this.baseUrl + "members", this.getHttpOptions());
  }

  private getHttpOptions() {
    return {
      headers: new HttpHeaders({
        Authorization: "Bearer " + this.accountService.currentUser()?.token
      })
    };
  }
}
