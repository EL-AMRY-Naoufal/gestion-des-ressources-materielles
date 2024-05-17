import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppelOffresComponent } from './appel-offres.component';

describe('AppelOffresComponent', () => {
  let component: AppelOffresComponent;
  let fixture: ComponentFixture<AppelOffresComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AppelOffresComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AppelOffresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
