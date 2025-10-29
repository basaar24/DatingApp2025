import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { memberResolver } from './member-resolver';

describe('memberResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => memberResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
