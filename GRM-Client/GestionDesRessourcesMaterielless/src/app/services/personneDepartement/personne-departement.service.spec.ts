import { TestBed } from '@angular/core/testing';

import { PersonneDepartementService } from './personne-departement.service';

describe('PersonneDepartementService', () => {
  let service: PersonneDepartementService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PersonneDepartementService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
