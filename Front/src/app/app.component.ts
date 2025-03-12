
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {Router, RouterLink, RouterModule, RouterOutlet } from '@angular/router';
import {jwtDecode} from 'jwt-decode';
import { FooterComponent } from "./componente/footer/footer.component"; 
import { HttpClient } from '@angular/common/http';


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


  constructor(private router: Router, private http:HttpClient) { }


  getToken(): string {
    this.token = localStorage.getItem('data') // Retorna el token de sesión o localStorage, según sea el primero disponible
    if (this.token) {
      const decodedToken: any = jwtDecode(this.token);
      this.role = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
  
      // console.log('Rol del usuario:', this.role);
    }
    return this.role;
  }


  usuarioLogeado(): boolean {
    return !!localStorage.getItem('data'); // Retorna true si hay un usuario, false si no
  }



  logout(){
    sessionStorage.removeItem('data'); // Elimina el token de sesión
    localStorage.removeItem('data'); // Elimina el token de localStorage
    localStorage.removeItem('expirationTime');
    this.router.navigate(['/']); // Redirige al login
  }

  prueba() {
    // Hacer la solicitud GET a la ruta protegida (Prueba)
    return this.http.get('http://localhost:7600/api/Auth', { withCredentials: true }).subscribe(data => {
      console.log('Respuesta de la prueba:', data);
    });
  }


  
}
