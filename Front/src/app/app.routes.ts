// import { NgModule } from '@angular/core';
// import { RouterModule, Routes } from '@angular/router';
// import { LandingPageComponent } from './componente/landing-page/landing-page.component';
// import { RegisterProjectComponent } from './componente/registerProjects/registerProject.component';
// import { LoginComponent } from './componente/login/login.component';
// import { RegisterComponent } from './componente/register/register.component';
// import { DashboardComponent } from './componente/dashboard/dashboard.component';
// import { DetailProjectComponent } from './componente/detail-project/detail-project.component';
// import { EditarProjectComponent } from './componente/editar-project/editar-project.component';
// import { ProjectsComponent } from './componente/projects/projects.component';
// import { TablaSkillsComponent } from './componente/tabla-skills/tabla-skills.component';

// export const routes: Routes = [
//     { path: '', component: LandingPageComponent },
//     { path: 'register-projects', component: RegisterProjectComponent },
//     { path: 'register', component: RegisterComponent },
//     { path: 'login', component: LoginComponent },
//     { path: 'detail/:id', component: DetailProjectComponent },
//     { path: 'edit/:id', component: EditarProjectComponent },

//     // **Dashboard con rutas anidadas**
//     {
//         path: 'dashboard',
//         component: DashboardComponent,
//         children: [
//             { path: 'projects', component: ProjectsComponent }, // Página de proyectos
//             { path: 'skills', component: TablaSkillsComponent }, // Editar proyecto dentro del dashboard
//             { path: 'detail/:id', component: DetailProjectComponent }, // Detalle de proyecto dentro del dashboard
//             // { path: '', redirectTo: 'projects', pathMatch: 'full' }  // Redirección por defecto a 'projects'
//         ]
//     },

//     { path: '**', redirectTo: '' }  // Redirección si la ruta no existe
// ];

// @NgModule({
//     imports: [RouterModule.forRoot(routes)], 
//     exports: [RouterModule] 
// })
// export class AppRoutingModule { }
