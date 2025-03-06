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
    return this.http.post(this.baseUrl, body);
  }
}
