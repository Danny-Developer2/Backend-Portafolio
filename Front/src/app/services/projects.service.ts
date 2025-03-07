import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable, tap } from 'rxjs';

export interface Project {
  id: number;
  name: string;
  description: string;
  technology: string;
  url: string;
  imgUrl: string;
  skills: { $values: Skill[] };
  experience: { $values: Experience[] };
}

export interface Skill {
  id: number;
  skillName: string;
  name: string;
  percentage: number;
  iconUrl: string;
  description: string;
}

export interface Experience {
  id: number;
  companyName: string;
  position: string;
  startDate: string;
  endDate: string;
  description: string;
}

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {
  baseUrl = `${environment.apiUrl}Projects`;
  private http = inject(HttpClient);
  private router: Router = new Router();
  projects: Project[] = [];

  constructor() { }
  

  getProjects(): Observable<Project[]> {
    const token = localStorage.getItem('token'); // Obtener el token del localStorage

    if (!token) {
      console.error('No se encontró el token en el localStorage.');
      return new Observable(); // Devuelve un observable vacío para evitar errores
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.http.get<{$values:Project[]}>(this.baseUrl, { headers }).pipe(
      map(response => response?.$values || []),
      tap((data) =>this.projects = data)
    )
  }
  

  
}
