import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MemberMessages } from './member-messages';

describe('MemberMessages', () => {
  let component: MemberMessages;
  let fixture: ComponentFixture<MemberMessages>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MemberMessages]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MemberMessages);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
