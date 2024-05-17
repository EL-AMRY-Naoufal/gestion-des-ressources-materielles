import { Component, OnInit } from '@angular/core';
import { FournisseurService } from '../../services/fournisseur/fournisseur.service';
import { TooltipPosition } from '@angular/material/tooltip';
import { FormControl } from '@angular/forms';
import { LoginService } from '../../services/login/login.service';


@Component({
  selector: 'app-appel-offres',
  templateUrl: './appel-offres.component.html',
  styleUrl: './appel-offres.component.scss'
})
export class AppelOffresComponent implements OnInit {
  currentUser: any;

  appelOffres: any[] = [];
  panelOpenState = false;
  montant: number = 0;

  positionOptions: TooltipPosition[] = ['below', 'above', 'left', 'right'];
  position = new FormControl(this.positionOptions[0]);

  displayedOrdinateurColumns: string[] = [
    'marque',
    'cpu',
    'ram',
    'disqueDur',
    'ecran',
    'numberOfRessource'
  ];
  displayedImprimanteColumns: string[] = [
    'vitesseimpression',
    'resolution',
    'marque',
    'numberOfRessource'
  ];

  constructor(private fournisseurService : FournisseurService, private loginService: LoginService){}

  ngOnInit(): void {
    this.currentUser = this.loginService.getUser();
    this.fetchAppelOffres();
    this.getOffreFournisseur();
  }

  fetchAppelOffres() {
    this.fournisseurService.getAppelOffres(this.currentUser.userId).subscribe(
      (data: any[]) => {
      this.appelOffres = data;
      },
      (error) => {
        console.error('Error fetching appel offres:', error);
        // Handle error
      }
    );
  }

  submitOffer(appelOffreId: number) {
    this.fournisseurService.submitOffer(appelOffreId, this.currentUser.userId, this.montant).subscribe(
      (response) => {
        console.log('Offer submitted successfully:', response);
        this.fetchAppelOffres();
        // Reset the montant
        this.montant = 0;
      },
      (error) => {
        console.error('Error submitting offer:', error);
      }
    );
  }

  getOffreFournisseur() {
    this.fournisseurService.getOffreFournisseur(this.currentUser.userId).subscribe(
      (response) => {
        console.log('Offer submitted successfully:', response);
        // Reset the montant
        this.montant = 0;
      },
      (error) => {
        console.error('Error submitting offer:', error);
      }
    );
  }
}
