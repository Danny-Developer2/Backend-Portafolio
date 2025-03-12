import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingPageComponent } from './componente/landing-page/landing-page.component';
import { RegisterProjectComponent } from './componente/registerProjects/registerProject.component';
import { AuthGuard } from './componente/auth/auth.component';
import { DetailProjectComponent } from './componente/detail-project/detail-project.component';
import { EditarProjectComponent } from './componente/editar-project/editar-project.component';


export const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'register-projects', component: RegisterProjectComponent,
    canActivate: [AuthGuard]  // Protege la ruta
    
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('./pages/auth/auth.module').then((m) => m.AuthModule)
  },
  {
    path: 'dashboard',
    loadChildren: () =>
      import('./pages/dashboard/dashboard.module').then(
        (m) => m.DashboardModule
      )
  },
  // Aquí irán las rutas
  {
    path: 'edit',
    loadChildren: () => import('./pages/edit/edit.module').then(m => m.EditModule),
    canActivate: [AuthGuard]  // Protege la ruta
  },
  {
    path: 'detail',
    loadChildren: () => import('./pages/detail/detail.module').then(m => m.DetailModule),
    canActivate: [AuthGuard]  // Protege la ruta
  },
 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)], // Para rutas globales
  exports: [RouterModule],
})
export class AppRoutingModule {}
