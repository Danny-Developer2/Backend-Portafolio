import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from '../../componente/login/login.component';
import { RegisterComponent } from '../../componente/register/register.component';
import { DetailProjectComponent } from '../../componente/detail-project/detail-project.component';
import { EditarProjectComponent } from '../../componente/editar-project/editar-project.component';
import { AuthGuard } from '../../componente/auth/auth.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthRoutingModule {}
