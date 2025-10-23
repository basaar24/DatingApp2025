import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MemberCard } from './member-card';

describe('MemberCard', () => {
  let component: MemberCard;
  let fixture: ComponentFixture<MemberCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MemberCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MemberCard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
