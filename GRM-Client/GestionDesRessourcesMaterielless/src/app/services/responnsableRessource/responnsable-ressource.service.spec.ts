import { TestBed } from '@angular/core/testing';

import { ResponnsableRessourceService } from './responnsable-ressource.service';

describe('ResponnsableRessourceService', () => {
  let service: ResponnsableRessourceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ResponnsableRessourceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
