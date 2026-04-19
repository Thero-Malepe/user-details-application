import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeniedAccess } from './denied-access';

describe('DeniedAccess', () => {
  let component: DeniedAccess;
  let fixture: ComponentFixture<DeniedAccess>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DeniedAccess]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeniedAccess);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
