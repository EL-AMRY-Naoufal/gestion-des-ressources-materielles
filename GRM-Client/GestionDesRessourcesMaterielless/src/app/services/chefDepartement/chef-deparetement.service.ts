import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, catchError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ChefDeparetementService {
  private readonly baseUrl: string =
    'https://localhost:7159/api/ChefDepartement/';
  constructor(private http: HttpClient, private router: Router) {}

  getResourcesForDepartement(departementId: number) {
    return this.http.get<any>(
      `${this.baseUrl}GetResourcesForDepartement?departementId=${departementId}`
    );
  }

  getBesoinsForChefDepartement(userId: number): Observable<any> {
    return this.http
      .get<any>(`${this.baseUrl}GetBesoinsForChefDepartement?userId=${userId}`)
      .pipe(
        catchError((error) => {
          throw 'Error in getting besoins for chef departement: ' + error;
        })
      );
  }

  approveBesoins(userId: number, besoinIds: number[]): Observable<any> {
    return this.http.post(`${this.baseUrl}approveBesoins`, {
      userId,
      besoinIds,
    });
  }
}
