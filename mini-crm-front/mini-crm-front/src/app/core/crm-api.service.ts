import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class CrmApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/api/v1`;

  list<T>(resource: string, params: Record<string, string | number | boolean | undefined> = {}): Observable<T> {
    let httpParams = new HttpParams();
    Object.entries(params).forEach(([key, value]) => {
      if (value !== undefined) httpParams = httpParams.set(key, String(value));
    });
    return this.http.get<T>(`${this.baseUrl}/${resource}`, { params: httpParams });
  }

  get<T>(resource: string, id: string | number): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/${resource}/${id}`);
  }

  create<T>(resource: string, body: unknown): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}/${resource}`, body);
  }

  update<T>(resource: string, id: string | number, body: unknown): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/${resource}/${id}`, body);
  }

  patch<T>(resource: string, path: string, body: unknown = {}): Observable<T> {
    return this.http.patch<T>(`${this.baseUrl}/${resource}/${path}`, body);
  }

  post<T>(resource: string, path: string, body: unknown = {}): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}/${resource}/${path}`, body);
  }
}
