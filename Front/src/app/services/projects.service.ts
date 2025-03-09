import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, Observable, of, tap } from 'rxjs';




export interface updateProject {
  id: number;
  name: string;
  description: string;
  technology: string;
  url: string;
  imgUrl: string;
}
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

  getProjectById(id: number): Observable<Project> {
    const token = localStorage.getItem('token'); // Obtener el token del localStorage
  
    if (!token) {
      console.error('No se encontró el token en el localStorage.');
      return new Observable(); // Devuelve un observable vacío para evitar errores
    }
  
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  
    // Corregir la URL para interpolar correctamente el id
    const url = `${this.baseUrl}/${id}`;
    return this.http.get<Project>(url, { headers }).pipe(
      tap((data) => console.log('Respuesta de la API:', data)) // Verifica la respuesta
    );
    
  }

  // deleteProject(id:number): Observable<Project> {
  //   const token = localStorage.getItem('token'); // Obtener el token del localStorage
  //   if (!token) {
  //     console.error('No se encontró el token en el localStorage.');
  //     return new Observable(); // Devuelve un observable vacío para evitar errores
  //   }
  //   const headers = new HttpHeaders({
  //     'Authorization': `Bearer ${token}`,
  //     'Content-Type': 'application/json'
  //   });
  //   const url = `${this.baseUrl}/${id}`;
  //   console.log('Eliminando');
  //   return this.http.delete<Project>(url, { headers }).pipe(
  //     tap((data) => console.log('Proyecto eliminado:', data)) // Verifica la respuesta
  //   );
  // }

  deleteProject(id: number): any {
    if (id !== null && id !== undefined) {
      this.http
        .delete<string>(`${this.baseUrl}/${id}`, { responseType: 'text' as 'json' })
        .subscribe(
          (response) => {
            console.log('Respuesta del servidor:', response); // Respuesta es el texto del backend
            if (response) {
              this.getProjects(); // Actualiza la lista de proyectos si la eliminación fue exitosa
            }
            return true;
          },
          (error) => {
            console.error('Error al eliminar el proyecto:', error);
            console.error('Estado del error:', error.status);
            return false;
          }
        );
    } else {
      console.error('ID de proyecto no válido');
    }
  }


  updateProject(id: number, project: any): Observable<boolean> {
    return this.http
      .put<string>(`${this.baseUrl}/${id}`, project, { responseType: 'text' as 'json' })
      .pipe(
        map((response) => {
          console.log('Respuesta del servidor:', response);
          return true; // Devuelve true si la actualización fue exitosa
        }),
        catchError((error) => {
          console.error('Error al actualizar el proyecto:', error.message);
          return of (false); // Devuelve false en caso de error
        })
      );
  }
  
  
  
  

  
}
