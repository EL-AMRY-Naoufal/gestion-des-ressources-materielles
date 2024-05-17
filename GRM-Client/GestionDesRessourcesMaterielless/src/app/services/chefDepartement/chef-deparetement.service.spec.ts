import { TestBed } from '@angular/core/testing';

import { ChefDeparetementService } from './chef-deparetement.service';

describe('ChefDeparetementService', () => {
  let service: ChefDeparetementService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChefDeparetementService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
