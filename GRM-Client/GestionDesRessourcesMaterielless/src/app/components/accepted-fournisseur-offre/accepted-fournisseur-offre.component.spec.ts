import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AcceptedFournisseurOffreComponent } from './accepted-fournisseur-offre.component';

describe('AcceptedFournisseurOffreComponent', () => {
  let component: AcceptedFournisseurOffreComponent;
  let fixture: ComponentFixture<AcceptedFournisseurOffreComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AcceptedFournisseurOffreComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AcceptedFournisseurOffreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
