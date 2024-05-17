import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ChefDeparetementService } from '../../services/chefDepartement/chef-deparetement.service';
import { MatPaginator } from '@angular/material/paginator';
import { TooltipPosition } from '@angular/material/tooltip';
import { FormControl } from '@angular/forms';
import { ResponnsableRessourceService } from '../../services/responnsableRessource/responnsable-ressource.service';
import { LoginService } from '../../services/login/login.service';

export interface Besoin {
  besoinId: number;
  personneDepartementId: any; 
  ressourceCatalogteId: any; 
  numberOfRessource: number;
  dateRequested: Date;
}

@Component({
  selector: 'app-liste-besoin',
  templateUrl: './liste-besoin.component.html',
  styleUrls: ['./liste-besoin.component.scss']
})
export class ListeBesoinComponent implements OnInit, AfterViewInit  {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  displayedBesoinColumns: string[] = ['BesoinId', 'PersonneDepartement', 'RessourceCatalogteId', 'NumberOfRessource', 'DateRequested'];
  besoinDataSource = new MatTableDataSource<Besoin>();

  positionOptions: TooltipPosition[] = ['below', 'above', 'left', 'right'];
  position = new FormControl(this.positionOptions[0]);

  role : string = "";

  constructor(private loginService : LoginService, private chefDeparetementService: ChefDeparetementService, private ressponsableRessourceService : ResponnsableRessourceService) { }

  ngOnInit(): void {
    this.role = this.loginService.getRole();
    if(this.role == "chefDepartement"){
      console.log('here iamù');
      this.fetchBesoins();
    }
    else{
      console.log('here iamù resss');

      this.fetchAllBesoinsForRess();
    }
  }

  ngAfterViewInit() {
    this.besoinDataSource.paginator = this.paginator;
  }

  fetchBesoins() {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    const userId = user.userId; 
    this.chefDeparetementService.getBesoinsForChefDepartement(userId).subscribe((besoins: Besoin[]) => {
      this.besoinDataSource.data = besoins;
    });
  }

  fetchAllBesoinsForRess() {
    this.ressponsableRessourceService.getBesoinsForResponsableRessources().subscribe((besoins: Besoin[]) => {
      this.besoinDataSource.data = besoins;
    }, error => {
      console.error('Error occurred while fetching besoins for responsible resources:', error);
    });
  }

  createAppelOffre(){
    this.ressponsableRessourceService.createAppelOffre().subscribe(
      () => {
        // Success callback
        console.log('Appel d\'offre créé avec succès');
        // Optionally, you can refresh the data or show a success message
      },
      error => {
        // Error callback
        console.error('Une erreur est survenue lors de la création de l\'appel d\'offre:', error);
        // Optionally, you can handle the error or show an error message
      }
    );
  }

  approveBesoins() {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    const userId = user.userId; 
    const besoinIds = Array.from(this.clickedRows).map(row => row.besoinId);
    console.log(besoinIds);
    console.log(typeof besoinIds);
    this.chefDeparetementService.approveBesoins(userId, besoinIds).subscribe(() => {
      // Success callback
    }, error => {
      // Error callback
      console.error('Error occurred while approving besoins:', error);
      // Optionally handle the error here, e.g., display an error message to the user
    });
    this.fetchBesoins();
  }

  clickedRows = new Set<Besoin>();
}