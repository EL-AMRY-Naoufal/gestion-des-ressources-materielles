import { Component } from '@angular/core';
import { LoginService } from '../../services/login/login.service';
import { FournisseurService } from '../../services/fournisseur/fournisseur.service';
import { FormControl } from '@angular/forms';
import { TooltipPosition } from '@angular/material/tooltip';
import { ResponnsableRessourceService } from '../../services/responnsableRessource/responnsable-ressource.service';

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

  role : any;

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

  constructor(private fournisseurService : FournisseurService, private loginService: LoginService, private ResponsableRessurces : ResponnsableRessourceService){}

  ngOnInit(): void {
    this.currentUser = this.loginService.getUser();
    this.role = this.loginService.getRole();
    if(this.role == "responsableRessources"){
      this.getOffreFournisseurs();
    }else{
      this.getOffreFournisseur();
    }
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

  getOffreFournisseurs() {
    console.log("dsd")
    this.ResponsableRessurces.getOffreFournisseurs().subscribe(
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

  handleOffer(offreFournisseurId:any, accept: boolean) {
    console.log(offreFournisseurId)
    this.ResponsableRessurces.handleOffreFournisseur(offreFournisseurId, accept)
      .subscribe({
        next: (response) => {
          console.log(response);
        },
        error: (error) => {
          console.error(error);
          // Handle the error as needed
        }
      });
  }
}
