import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class MainLayoutService {
  private readonly baseUrl:string = "https://localhost:7159/api/ChefDepartement/"
  constructor(private http:HttpClient, private router:Router) { }

  sendDemandeBesoinToEnseignants(departementId: number) {
    return this.http.post<any>(`${this.baseUrl}sendDemandeBesoinToEnseignants?departementId=${departementId}`, {});
  }
}
