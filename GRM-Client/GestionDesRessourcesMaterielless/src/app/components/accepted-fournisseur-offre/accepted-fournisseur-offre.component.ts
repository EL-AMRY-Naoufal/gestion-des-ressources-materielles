import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';
import { TooltipPosition } from '@angular/material/tooltip';
import { FournisseurService } from '../../services/fournisseur/fournisseur.service';
import { LoginService } from '../../services/login/login.service';

@Component({
  selector: 'app-accepted-fournisseur-offre',
  templateUrl: './accepted-fournisseur-offre.component.html',
  styleUrl: './accepted-fournisseur-offre.component.scss'
})
export class AcceptedFournisseurOffreComponent {
  currentUser: any;

  deliveryDate: Date | null = new Date();

  appelOffres: any[] = [];
  panelOpenState = false;

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
    this.fetchAccpetedAppelOffres();
  }

  fetchAccpetedAppelOffres() {
    this.fournisseurService.getAccpetedAppelOffre(this.currentUser.userId).subscribe(
      (data: any[]) => {
      this.appelOffres = data;
      },
      (error) => {
        console.error('Error fetching appel offres:', error);
        // Handle error
      }
    );
  }

  createRessource(appelOffreID: number) {
    const deliveryDateToSend = this.deliveryDate || new Date();

    this.fournisseurService.createRessource(this.currentUser.userId, appelOffreID, deliveryDateToSend).subscribe(
      response => {
        console.log('Ressource created successfully', response);
      },
      error => {
        console.error('Error creating ressource', error);
      }
    );
  }
}
