import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {





  
    private apiUrl =`${environment.apiUrl}Auth/register`; // URL del backend
  
    constructor(private http: HttpClient) { }
  
    // Método para registrar un nuevo proyecto
    registerUser(project: any): Observable<any> {
      return this.http.post<any>(this.apiUrl, project);
}
}
