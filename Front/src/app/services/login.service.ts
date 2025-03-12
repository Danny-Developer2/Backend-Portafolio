import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment.development';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private http = inject(HttpClient);
  private router: Router = new Router();

  baseUrl = `${environment.apiUrl}Auth/login`;


  constructor() { }

  // Método para hacer login
  login(email: string, password: string): Observable<any> {
    // Crea el cuerpo de la solicitud
    const body = { email, password };

    // Realiza la solicitud POST
    return this.http.post(this.baseUrl, body,{ withCredentials: true });
  }

  isTokenExpired(): boolean {
    const expiration = localStorage.getItem('expirationTime');
    if (!expiration) return true; // Si no hay expiración, se considera expirado
  
    const now = new Date().getTime();
    return now > parseInt(expiration) * 1000; // Retorna true si ha expirado
  }

  logout(){
    sessionStorage.removeItem('expirationTime'); // Elimina el token de sesión
    localStorage.removeItem('data'); // Elimina el token de localStorage
    localStorage.removeItem('expirationTime');
    this.router.navigate(['/']); // Redirige al login
  }

  checkSession() {
    return this.isTokenExpired();
    
  }

  usuarioLogeado(): boolean {
    return !!localStorage.getItem('data'); // Retorna true si hay un usuario, false si no
  }

  
}
