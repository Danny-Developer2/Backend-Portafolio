import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingPageComponent } from './componente/landing-page/landing-page.component';
import { RegisterComponent } from './componente/register/register.component';
import { LoginComponent } from './componente/login/login.component';

export const routes: Routes = [
    { path: '', component: LandingPageComponent },
    { path: 'register', component: RegisterComponent }, 
    { path: 'login', component: LoginComponent },
    { path: '**', redirectTo: '' }  // Si la ruta no existe, redirige al inicio
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],  // Configura las rutas en la aplicación
    exports: [RouterModule]  // Exporta RouterModule para que pueda ser utilizado en otros módulos
})
export class AppRoutingModule { }
