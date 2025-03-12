import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment.development';
import { map, Observable, tap } from 'rxjs';

export interface Experiences {
  id: number;
  companyName: string;
  position: string;
  startDate: string;
  endDate: string;
  user: any;
  description: string;  
  userId: number;  
}


@Injectable({
  providedIn: 'root',
})
export class GetExperiencesService {
  private http = inject(HttpClient);
  private router: Router = new Router();
  Experiences: Experiences[] = [];

  baseUrl = `${environment.apiUrl}Experience`;

  constructor() {}

  getExperiences(): Observable<Experiences[]> {
    const token = localStorage.getItem('data'); // Obtener el token del localStorage

    if (!token) {
      console.error('No se encontró el token en el localStorage.');
      return new Observable(); // Devuelve un observable vacío para evitar errores
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });

    return this.http.get<{ $values: Experiences[] }>(this.baseUrl, { headers }).pipe(
      map((response) => response?.$values || []),
      tap((data) => (this.Experiences = data))
    );
  }
}
