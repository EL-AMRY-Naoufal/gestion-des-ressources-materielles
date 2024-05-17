import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ObservableInput, catchError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ResponnsableRessourceService {
  private readonly baseUrl: string = 'https://localhost:7159/api/RessponsableRessources/';

  constructor(private http: HttpClient) {}

  getBesoinsForResponsableRessources(): Observable<any> {
    return this.http
      .get<any>(`${this.baseUrl}besoins`) // Assuming this endpoint is correct for fetching needs for responsible resources
      .pipe(
        catchError((error) => {
          throw 'Error in getting besoins for responsible resources: ' + error;
        })
      );
  }

  createAppelOffre(): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}createAppelOffre`, {}).pipe(
      catchError((error) => {
        throw 'Error creating appel d\'offre: ' + error;
      })
    );
  }
}
