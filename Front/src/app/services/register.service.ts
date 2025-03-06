import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {



  private apiUrl =`${environment.apiUrl}Projects`; // URL del backend

  constructor(private http: HttpClient) { }

  // Método para registrar un nuevo proyecto
  registerProject(project: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, project);
  }
}
