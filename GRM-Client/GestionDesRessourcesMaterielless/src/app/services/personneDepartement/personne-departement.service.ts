import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PersonneDepartementService {
  private readonly baseUrl: string = 'https://localhost:7159/api/PersonneDepartement/';

  constructor(private http: HttpClient) {}

  sendResourceRequests(requestBody: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}sendRequestForResources`, requestBody)
      .pipe(
        catchError(error => {
          return throwError('Error sending resource requests: ' + error);
        })
      );
  }
}
