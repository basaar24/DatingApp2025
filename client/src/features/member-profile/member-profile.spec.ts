import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MemberProfile } from './member-profile';

describe('MemberProfile', () => {
  let component: MemberProfile;
  let fixture: ComponentFixture<MemberProfile>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MemberProfile]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MemberProfile);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
