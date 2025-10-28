import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MemberPhotos } from './member-photos';

describe('MemberPhotos', () => {
  let component: MemberPhotos;
  let fixture: ComponentFixture<MemberPhotos>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MemberPhotos]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MemberPhotos);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
