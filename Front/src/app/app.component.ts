
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {Router, RouterLink, RouterModule, RouterOutlet } from '@angular/router';
import {jwtDecode} from 'jwt-decode';
import { FooterComponent } from "./componente/footer/footer.component"; 


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, RouterModule, FormsModule, CommonModule, FooterComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Front';

  token: string | null = null // Obtiene el token actual

  role: string = '';


  constructor(private router: Router) { }


  getToken(): string {
    this.token = sessionStorage.getItem('token') || localStorage.getItem('token'); // Retorna el token de sesión o localStorage, según sea el primero disponible
    if (this.token) {
      const decodedToken: any = jwtDecode(this.token);
      this.role = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
  
      // console.log('Rol del usuario:', this.role);
    }
    return this.role;
  }


  usuarioLogeado(): boolean {
    return !!localStorage.getItem('token'); // Retorna true si hay un usuario, false si no
  }


  logout(){
    sessionStorage.removeItem('token'); // Elimina el token de sesión
    localStorage.removeItem('token'); // Elimina el token de localStorage
    localStorage.removeItem('expirationTime');
    this.router.navigate(['/']); // Redirige al login
  }

  
}
