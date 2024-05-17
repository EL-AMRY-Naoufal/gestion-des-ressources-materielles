import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FournisseurService {
  private readonly baseUrl: string = 'https://localhost:7159/api/Fournisseur/';
  constructor(private http: HttpClient, private router: Router) {}

  getCatalogs(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}catalogs`);
  }

  getAppelOffres(fournisseurId: number): Observable<any> {
    return this.http.get<any>(
      `${this.baseUrl}appelOffres?fournisseurId=${fournisseurId}`
    );
  }

  submitOffer(
    appelOffreId: number,
    fournisseurId: number,
    montant: number
  ): Observable<any> {
    const body = {
      appelOffreId: appelOffreId,
      fournisseurId: fournisseurId,
      montant: montant,
    };

    return this.http.post<any>(`${this.baseUrl}submitOffer`, body);
  }

  getOffreFournisseur(fournisseurId: number): Observable<any> {
    return this.http.get<any>(
      `${this.baseUrl}offresFournisseur?fournisseurId=${fournisseurId}`
    );
  }

  getAccpetedAppelOffre(fournisseurId: number): Observable<any> {
    return this.http.get<any>(
      `${this.baseUrl}acceptedOffresFournisseur?fournisseurId=${fournisseurId}`
    );
  }

  changeMontant(appelOffreId: number, montant: number): Observable<any> {
    const body = { appelOffreId: appelOffreId, montant: montant };
    console.log(body)
    return this.http.post<any>(`${this.baseUrl}changeMontant`, body);
  }

  createRessource(fournisseurID: number, appelOffreID: number, deliveryDate: Date): Observable<any> {
    const body = { fournisseurID: fournisseurID, appelOffreID: appelOffreID, deliveryDate: deliveryDate };
    console.log(body)
    return this.http.post<any>(`${this.baseUrl}createRessource`, body);
  }
}
