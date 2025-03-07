
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {Router, RouterLink, RouterModule, RouterOutlet } from '@angular/router';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet,RouterLink,RouterModule,FormsModule,CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Front';


  constructor(private router: Router) { }

  usuarioLogeado(): boolean {
    return !!sessionStorage.getItem('token'); // Retorna true si hay un usuario, false si no
  }


  logout(){
    sessionStorage.removeItem('token'); // Elimina el token de sesión
    localStorage.removeItem('token'); // Elimina el token de localStorage
    this.router.navigate(['/']); // Redirige al login
  }
}
