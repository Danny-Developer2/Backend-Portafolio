import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditarProjectComponent } from './editar-project.component';

describe('EditarProjectComponent', () => {
  let component: EditarProjectComponent;
  let fixture: ComponentFixture<EditarProjectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditarProjectComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditarProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
