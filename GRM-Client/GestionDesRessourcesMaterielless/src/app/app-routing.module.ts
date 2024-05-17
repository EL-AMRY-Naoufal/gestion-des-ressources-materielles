import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { MainLayoutComponent } from './components/main-layout/main-layout.component';
import { AuthGuard } from './guards/auth.guard';
import { ListRessourcesComponent } from './components/list-ressources/list-ressources.component';
import { ListeBesoinComponent } from './components/liste-besoin/liste-besoin.component';
import { SendBesoinsComponent } from './components/send-besoins/send-besoins.component';
import { AppelOffresComponent } from './components/appel-offres/appel-offres.component';
import { OffreFournisseurComponent } from './components/offre-fournisseur/offre-fournisseur.component';
import { AcceptedFournisseurOffreComponent } from './components/accepted-fournisseur-offre/accepted-fournisseur-offre.component';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  {
    path: 'mainLayout',
    component: MainLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'listeRessources', component:  ListRessourcesComponent}, 
      {path : 'listBesoins', component : ListeBesoinComponent},
      {path : 'sendBesoins', component : SendBesoinsComponent},
      {path : 'appelOffres', component : AppelOffresComponent},
      {path : 'offreFournisseur', component : OffreFournisseurComponent},
      {path : 'acceptedOffreFournisseur', component : AcceptedFournisseurOffreComponent}
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
