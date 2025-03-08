import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class RoleUser {
  role: string = '';

  constructor() {}

  // Método para validar el token
  roleUser(token: string): any {
    if (token) {
      const decodedToken: any = jwtDecode(token);
      this.role = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    }
    
    return this.role

    
  }
}
