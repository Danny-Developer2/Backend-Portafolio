
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {Router, RouterLink, RouterModule, RouterOutlet } from '@angular/router';
import {jwtDecode} from 'jwt-decode';
import { FooterComponent } from "./componente/footer/footer.component"; 
import { HttpClient } from '@angular/common/http';
import { environment } from '../environments/environment.development';
import { LoginService } from './services/login.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, RouterModule, FormsModule, CommonModule, FooterComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Front';

  token: string | null = null // Obtiene el token actual

  role: string  | null = null;
  

  //  baseUrl = `${environment.apiUrl}Auth/read-token-data`;


  constructor(private router: Router, private http:HttpClient,private logoutService: LoginService,private toastr:ToastrService) { }


  getToken(): string {
    this.role = sessionStorage.getItem('role') 
    
    return this.role!;
  }


  usuarioLogeado(): boolean {
    return !!localStorage.getItem('token'); // Retorna true si hay un usuario, false si no
  }



  logout(){
    this.token = sessionStorage.getItem('token') || localStorage.getItem('token'); 
    this.logoutService.logaut(this.token!)
    sessionStorage.removeItem('role'); // Elimina el token de sesión
    localStorage.removeItem('token'); // Elimina el token de localStorage
    sessionStorage.removeItem('expirationTime');
    localStorage.removeItem('expirationTime');
    this.toastr.success('Sesión finalizada correctamente.', '', {
      timeOut: 4000,
      positionClass: 'toast-top-right',
    });  // Muestra un mensaje de éxito al cerrar la sesión
    this.router.navigate(['/']); // Redirige al login
  }

  prueba() {
    // Hacer la solicitud GET a la ruta protegida (Prueba)
    return this.http.get('http://localhost:7600/api/Auth/test', { withCredentials: true }).subscribe(data => {
      console.log('Respuesta de la prueba:', data);
    });
  }


  
}
