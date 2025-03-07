import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class RegisterProjectsService {



  private apiUrl =`${environment.apiUrl}Projects`; // URL del backend

  constructor(private http: HttpClient) { }

  // Método para registrar un nuevo proyecto
  registerProject(project: any,token: string): Observable<any> {

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`, // Agregar el token en el encabezado
      'Content-Type': 'application/json'
    });
    return this.http.post<any>(this.apiUrl, project,{headers});
  }
}
