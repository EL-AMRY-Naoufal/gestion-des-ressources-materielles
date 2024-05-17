import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ChefDeparetementService } from '../../services/chefDepartement/chef-deparetement.service';

export interface Ordinateur {
  Marque: string;
  Cpu: string;
  Ram: string;
  DisqueDur: string;
  Ecran: string;
}

export interface Imprimante {
  Marque: string;
  Resolution: string;
  VitesseImpression: number;
}

@Component({
  selector: 'app-list-ressources',
  templateUrl: './list-ressources.component.html',
  styleUrl: './list-ressources.component.scss',
})
export class ListRessourcesComponent implements AfterViewInit {
  constructor(private chefDeparetementService: ChefDeparetementService) {}

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
  ordinateurDataSource = new MatTableDataSource<Ordinateur>();
  imprimanteDataSource = new MatTableDataSource<Imprimante>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.ordinateurDataSource.paginator = this.paginator;
    this.fetchResources();
  }

  fetchResources() {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    const departementId = user.departement.departmentId;

    this.chefDeparetementService
      .getResourcesForDepartement(departementId)
      .subscribe((response: any) => {
        response.imprimantes.forEach((imprimante: any) => {
          const newImprimante: Imprimante = {
            Marque: imprimante.marque,
            Resolution: imprimante.resolution,
            VitesseImpression: imprimante.vitesseimpression,
          };
          this.imprimanteDataSource.data.push(newImprimante);
        });
        this.imprimanteDataSource._updateChangeSubscription();

        response.ordinateurs.forEach((ordinateur: any) => {
          const newOrdinateur: Ordinateur = {
            Marque: ordinateur.marque,
            Cpu: ordinateur.cpu,
            Ram: ordinateur.ram,
            DisqueDur: ordinateur.disqueDur,
            Ecran: ordinateur.ecran,
          };
          this.ordinateurDataSource.data.push(newOrdinateur);
        });
        this.ordinateurDataSource._updateChangeSubscription();
      });
  }
  
}
