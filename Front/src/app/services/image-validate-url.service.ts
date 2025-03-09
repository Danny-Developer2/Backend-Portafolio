import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ImageValidateUrlService {
  constructor(private http: HttpClient) {}

  validateImageUrl(url: string) {
    console.log('Validando URL:', url);
    return this.http.get(url, { responseType: 'blob', observe: 'response' }).pipe(
      map(response => {
        console.log('Response:', response);
        const contentType = response.headers.get('Content-Type');
        console.log('Content-Type:', contentType);
        return contentType ? contentType.startsWith('image/') : false;
      }),
      catchError(err => {
        console.error('Error al validar la imagen:', err);
        return of(false);
      })
    );
  }
}