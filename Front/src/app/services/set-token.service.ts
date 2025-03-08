import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class SetExpToken {
  tokeTimeValidator: string = '';

  constructor() {}

  // Método para validar el token
  setExpToken(token: string): any {

    console.log('soy el token',token);
    if (token) {
      const decodedToken: any = jwtDecode(token!);
      this.tokeTimeValidator = decodedToken['exp'];
      
      
    }
    return this.tokeTimeValidator

    
  }
}
