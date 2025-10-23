import { Component, inject } from '@angular/core';
import { MembersService } from '../../../core/services/members-service';
import { Member } from '../../../types/member';
import { Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-member-list',
  imports: [AsyncPipe],
  templateUrl: './member-list.html',
  styleUrl: './member-list.css'
})
export class MemberList {
  private membersService = inject(MembersService);
  protected members$: Observable<Member[]>;

  constructor() {
    this.members$ = this.membersService.getMembers();
  }
}
