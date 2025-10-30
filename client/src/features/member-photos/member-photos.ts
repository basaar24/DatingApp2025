import { Component, inject } from '@angular/core';
import { MembersService } from '../../core/services/members-service';
import { ActivatedRoute } from '@angular/router';
import { Photo } from '../../types/member';
import { Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-member-photos',
  imports: [AsyncPipe],
  templateUrl: './member-photos.html',
  styleUrl: './member-photos.css'
})
export class MemberPhotos {
  private membersService = inject(MembersService);
  private route = inject(ActivatedRoute);
  protected photos$?: Observable<Photo[]>;

  constructor() {
    const memberId = this.route.parent?.snapshot.paramMap.get("id");
    if (memberId) {
      this.photos$ = this.membersService.getPhotos(memberId);
    }
  }

  get photoMocks() {
    return Array.from({ length: 20 }, (_, i) => ({
      url: "./user.jpg"
    }));
  }
}
