import { TestBed } from '@angular/core/testing';

import { UserDetails } from './user-details.service';

describe('UserDetails', () => {
  let service: UserDetails;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserDetails);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
