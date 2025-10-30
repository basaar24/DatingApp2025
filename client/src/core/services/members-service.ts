import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member, Photo } from '../../types/member';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;

  getMember(id: string): Observable<Member> {
    return this.http.get<Member>(this.baseUrl + "members/" + id);
  }

  getMembers(): Observable<Member[]> {
    return this.http.get<Member[]>(this.baseUrl + "members");
  }

  getPhotos(id: string) {
    return this.http.get<Photo[]>(`${this.baseUrl}members/${id}/photos`);
  }
}
