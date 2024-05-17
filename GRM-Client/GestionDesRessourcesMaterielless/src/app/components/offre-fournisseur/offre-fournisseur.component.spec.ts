import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OffreFournisseurComponent } from './offre-fournisseur.component';

describe('OffreFournisseurComponent', () => {
  let component: OffreFournisseurComponent;
  let fixture: ComponentFixture<OffreFournisseurComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [OffreFournisseurComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(OffreFournisseurComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
