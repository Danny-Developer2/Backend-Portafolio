import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DetailComponent } from './detail.component';
import { DetailProjectComponent } from '../../componente/detail-project/detail-project.component';
import { AuthGuard } from '../../componente/auth/auth.component';

const routes: Routes = [
  { path: ':id', component: DetailProjectComponent,canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DetailRoutingModule { }
