import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SendBesoinsComponent } from './send-besoins.component';

describe('SendBesoinsComponent', () => {
  let component: SendBesoinsComponent;
  let fixture: ComponentFixture<SendBesoinsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SendBesoinsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SendBesoinsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
