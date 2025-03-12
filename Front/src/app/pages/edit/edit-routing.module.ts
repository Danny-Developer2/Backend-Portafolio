import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EditComponent } from './edit.component';
import { EditarProjectComponent } from '../../componente/editar-project/editar-project.component';
import { AuthGuard } from '../../componente/auth/auth.component';

const routes: Routes = [
   { path: ':id', component: EditarProjectComponent,canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EditRoutingModule { }
