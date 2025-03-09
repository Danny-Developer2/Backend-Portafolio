import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingPageComponent } from './componente/landing-page/landing-page.component';
import { RegisterProjectComponent } from './componente/registerProjects/registerProject.component';
import { LoginComponent } from './componente/login/login.component';
import { RegisterComponent } from './componente/register/register.component';
import { DashboardComponent } from './componente/dashboard/dashboard.component';
import { DetailProjectComponent } from './componente/detail-project/detail-project.component';

export const routes: Routes = [
    { path: '', component: LandingPageComponent },
    { path: 'register-projects', component: RegisterProjectComponent }, 
    { path: 'register', component: RegisterComponent },  // Ruta para registrar un nuevo proyecto
    { path: 'dashboard', component: DashboardComponent }, // Ruta para el dashboard del usuario logueado
    { path: 'detail/:id', component: DetailProjectComponent }, // Ruta para desloguearse del usuario logueado. Este componente se encarga de borrar el token en local storage y redirigir al login.
    { path: 'login', component: LoginComponent },
    { path: '**', redirectTo: '' }  // Si la ruta no existe, redirige al inicio
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],  // Configura las rutas en la aplicación
    exports: [RouterModule]  // Exporta RouterModule para que pueda ser utilizado en otros módulos
    
})
export class AppRoutingModule { }
