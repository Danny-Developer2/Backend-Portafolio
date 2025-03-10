import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaSkillsComponent } from './tabla-skills.component';

describe('TablaSkillsComponent', () => {
  let component: TablaSkillsComponent;
  let fixture: ComponentFixture<TablaSkillsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TablaSkillsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TablaSkillsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
