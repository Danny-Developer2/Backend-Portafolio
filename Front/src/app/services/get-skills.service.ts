import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment.development';
import { map, Observable, tap } from 'rxjs';


export interface Skills {
  id: number;
  name: string;
  percentage: number;
  iconUrl: string;
  description: string;
  users: { $values: User[] };
  userSkills: any;  // Puedes definir una interfaz si conoces su estructura
  proyectSkills: any;  // Puedes definir una interfaz si conoces su estructura
}

export interface User {
  id: number;
  name: string;
  email: string;
}

@Injectable({
  providedIn: 'root'
})
export class GetSkillsService {

    private http = inject(HttpClient);
    private router: Router = new Router();
    skills: Skills[] = [];
    
  
    baseUrl = `${environment.apiUrl}Skills`;

  constructor() { }


   getSkills(): Observable<Skills[]> {
      const token = localStorage.getItem('token'); // Obtener el token del localStorage
  
      if (!token) {
        console.error('No se encontró el token en el localStorage.');
        return new Observable(); // Devuelve un observable vacío para evitar errores
      }
  
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      });
  
      return this.http.get<{$values:Skills[]}>(this.baseUrl, { headers }).pipe(
        map(response => response?.$values || []),
        tap((data) =>this.skills = data)
        
      )
    }
}


