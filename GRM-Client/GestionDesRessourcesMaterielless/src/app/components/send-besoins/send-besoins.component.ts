import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { FournisseurService } from '../../services/fournisseur/fournisseur.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { PersonneDepartementService } from '../../services/personneDepartement/personne-departement.service';

export interface OrdinateurCatalog {
  RessourceCatalogID: string;
  Marque: string;
  Cpu: string;
  Ram: string;
  DisqueDur: string;
  Ecran: string;
}

export interface ImprimanteCatalog {
  RessourceCatalogID: string;
  Marque: string;
  Resolution: string;
  VitesseImpression: number;
}

@Component({
  selector: 'app-send-besoins',
  templateUrl: './send-besoins.component.html',
  styleUrl: './send-besoins.component.scss',
})
export class SendBesoinsComponent implements AfterViewInit {
  constructor(
    private fournisseurService: FournisseurService,
    private personneDepartementService: PersonneDepartementService
  ) {}

  displayedOrdinateurColumns: string[] = [
    'Marque',
    'Cpu',
    'Ram',
    'DisqueDur',
    'Ecran',
  ];
  displayedImprimanteColumns: string[] = [
    'VitesseImpression',
    'Resolution',
    'Marque',
  ];
  ordinateurDataSource = new MatTableDataSource<OrdinateurCatalog>();
  imprimanteDataSource = new MatTableDataSource<ImprimanteCatalog>();
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.ordinateurDataSource.paginator = this.paginator;
    this.fetchResources();
  }

  fetchResources() {
    this.fournisseurService.getCatalogs().subscribe((data: any) => {
      console.log(data);
      // Extracting Ordinateur catalogs
      this.ordinateurDataSource.data = data.ordinateurCatalogs.map(
        (ordinateur: any) => {
          return {
            RessourceCatalogID: ordinateur.ressourceCatalogID,
            Marque: ordinateur.marque,
            Cpu: ordinateur.cpu,
            Ram: ordinateur.ram,
            DisqueDur: ordinateur.disqueDur,
            Ecran: ordinateur.ecran,
          };
        }
      );

      // Extracting Imprimante catalogs
      this.imprimanteDataSource.data = data.imprimantCatalogs.map(
        (imprimante: any) => {
          return {
            RessourceCatalogID: imprimante.ressourceCatalogID,
            Marque: imprimante.marque,
            Resolution: imprimante.resolution,
            VitesseImpression: imprimante.vitesseimpression,
          };
        }
      );
    });
  }
  clickedRows = new Set<ImprimanteCatalog>();
  clickedRowsOrdi = new Set<OrdinateurCatalog>();

  imprimanteQuantities: { [key: string]: number } = {};
  ordinateurQuantities: { [key: string]: number } = {};

  sendResourceRequests() {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    const userId = user.userId;
  
    const imprimanteRequests: any = {};
    const ordinateurRequests: any = {};
  
    // Iterate over clicked ImprimanteCatalog rows and get quantities
    Array.from(this.clickedRows).forEach((row) => {
      const quantity = this.imprimanteQuantities[row.RessourceCatalogID] || 0; // Default to 0 if quantity is not set
      imprimanteRequests[row.RessourceCatalogID] = quantity;
    });
  
    // Iterate over clicked OrdinateurCatalog rows and get quantities
    Array.from(this.clickedRowsOrdi).forEach((row) => {
      const quantity = this.ordinateurQuantities[row.RessourceCatalogID] || 0; // Default to 0 if quantity is not set
      ordinateurRequests[row.RessourceCatalogID] = quantity;
    });
  
    const requestBody = {
      PersonneId: userId,
      ImprimanteRequests: imprimanteRequests,
      OrdinateurRequests: ordinateurRequests,
    };
    console.log(requestBody);
  
    this.personneDepartementService.sendResourceRequests(requestBody).subscribe(
      (response) => {
        console.log('Resource requests sent successfully:', response);
        this.clickedRows.clear();
        this.clickedRowsOrdi.clear();

        
      },
      (error) => {
        this.clickedRows.clear(); // Empty the clickedRows Set
        this.clickedRowsOrdi.clear();
     
        console.error('Error sending resource requests:', error);
      }
    );
  }
  
}
