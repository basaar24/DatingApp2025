import { ResolveFn, Router } from '@angular/router';
import { MembersService } from '../../core/services/members-service';
import { inject } from '@angular/core';
import { Member } from '../../types/member';
import { EMPTY } from 'rxjs';

export const memberResolver: ResolveFn<Member> = (route, state) => {
  const membersService = inject(MembersService);
  const router = inject(Router);
  const memberId = route.paramMap.get("id");

  if (memberId) {
    return membersService.getMember(memberId);
  }

  router.navigateByUrl("/not-found");
  return EMPTY;
};
