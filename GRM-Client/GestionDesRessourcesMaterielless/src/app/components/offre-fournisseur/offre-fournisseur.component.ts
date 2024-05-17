import { Component } from '@angular/core';
import { LoginService } from '../../services/login/login.service';
import { FournisseurService } from '../../services/fournisseur/fournisseur.service';
import { FormControl } from '@angular/forms';
import { TooltipPosition } from '@angular/material/tooltip';

@Component({
  selector: 'app-offre-fournisseur',
  templateUrl: './offre-fournisseur.component.html',
  styleUrl: './offre-fournisseur.component.scss'
})
export class OffreFournisseurComponent {
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
    this.getOffreFournisseur();
  }
  getOffreFournisseur() {
    this.fournisseurService.getOffreFournisseur(this.currentUser.userId).subscribe(
      (response) => {
        this.appelOffres = response;
        console.log(this.appelOffres);
        // Reset the montant
        this.montant = 0;
      },
      (error) => {
        console.error('Error submitting offer:', error);
      }
    );
  }

  changeMontant(appelOffreId : number):void{
    this.fournisseurService.changeMontant(appelOffreId, this.montant).subscribe(
      (response) => {
        console.log('Montant chnaged');
      },
      (error) => {
        console.error('Error submitting offer:', error);
      }
    );
  }
}
