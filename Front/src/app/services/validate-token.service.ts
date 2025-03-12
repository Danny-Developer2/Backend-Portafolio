import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment.development';
import { map, Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ValidateTokenService {

    private http = inject(HttpClient);
    private router: Router = new Router();
  
    baseUrl = `${environment.apiUrl}Auth/verify-token`;
  

  constructor() { }


  verifyToken(token: string): Observable<{ message: string; isValid: boolean }> {
    return this.http.post<{ message: string; isValid: boolean }>(
      this.baseUrl,
      { token }
    ).pipe(
      // tap(response => console.log("Respuesta del backend:", response)) // Depurar
    );
  }
  
  
}
