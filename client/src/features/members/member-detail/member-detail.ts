import { Component, inject, OnInit, signal } from '@angular/core';
import { MembersService } from '../../../core/services/members-service';
import { ActivatedRoute, NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AsyncPipe } from '@angular/common';
import { Member } from '../../../types/member';
import { filter, Observable } from 'rxjs';

@Component({
  selector: 'app-member-detail',
  imports: [AsyncPipe, RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './member-detail.html',
  styleUrl: './member-detail.css'
})
export class MemberDetail implements OnInit {
  private membersService = inject(MembersService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  protected member$?: Observable<Member>;
  protected title = signal<string | undefined>("Profile");

  ngOnInit(): void {
    this.member$ = this.loadMember();
    this.title.set(this.route.firstChild?.snapshot?.title);

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe({
      next: () => {
        this.title.set(this.route.firstChild?.snapshot?.title);
      }
    });
  }

  loadMember(): Observable<Member> | undefined {
    const id = this.route.snapshot.paramMap.get("id");

    if (id) {
      return this.membersService.getMember(id);
    }

    return;
  }
}
