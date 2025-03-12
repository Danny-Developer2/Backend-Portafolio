import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProjectsComponent } from '../../componente/projects/projects.component';
import { TablaSkillsComponent } from '../../componente/tabla-skills/tabla-skills.component';
import { DetailProjectComponent } from '../../componente/detail-project/detail-project.component';
import { DashboardComponent } from '../../componente/dashboard/dashboard.component';
import { AuthGuard } from '../../componente/auth/auth.component';

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    canActivate: [AuthGuard],  // Protege la ruta
    children: [
      { path: 'projects', component: ProjectsComponent },
      { path: 'skills', component: TablaSkillsComponent },
      { path: 'detail/:id', component: DetailProjectComponent },
      { path: '', redirectTo: 'projects', pathMatch: 'full' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
