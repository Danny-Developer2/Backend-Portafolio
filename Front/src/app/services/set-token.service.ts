import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class SetExpToken {
  tokeTimeValidator: string = '';
  Role: string = '';

  baseUrl = `${environment.apiUrl}Auth/read-token-data`;

  constructor(private http: HttpClient) {}

  // Método para validar el token
  setExpToken(token: string): any {
    this.http
      .post(this.baseUrl, JSON.stringify(token), {
        headers: { 'Content-Type': 'application/json' },
        withCredentials: true,
      })
      .subscribe((response: any) => {
        if (response.dencryptedToken) {
          try {
            const decodedToken: any = jwtDecode(response.dencryptedToken);
            this.tokeTimeValidator = decodedToken['exp'];
            this.Role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

            if (this.Role === 'Admin') {
              sessionStorage.setItem('role', 'Admin');
            } else if (this.Role === 'User') {
              sessionStorage.setItem('role', 'User');
            }

            if (this.tokeTimeValidator) {
              localStorage.setItem(
                'expirationTime',
                this.tokeTimeValidator.toString()
              );
              sessionStorage.setItem(
                'expirationTime',
                this.tokeTimeValidator.toString()
              );
            } else {
              console.error(
                'Error: El token no contiene la fecha de expiración.'
              );
            }
          } catch (error) {
            console.error('Error al decodificar el token:', error);
          }
        } else {
          console.error(
            'Error: No se recibió `dencryptedToken` en la respuesta.'
          );
        }
      });

    // Calcular tiempo restante del token
    if (this.tokeTimeValidator) {
      const now = new Date().getTime();
      const expiration = parseInt(this.tokeTimeValidator, 10) * 1000;
      return expiration - now;
    }

    return null; // Retorna null si no hay un tiempo de expiración válido
  }
}
