import { Component, input } from '@angular/core';
import { Member } from '../../../types/member';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-member-card',
  imports: [RouterLink],
  templateUrl: './member-card.html',
  styleUrl: './member-card.css'
})
export class MemberCard {
  member = input.required<Member>();
}
