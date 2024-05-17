import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './components/login/login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MainLayoutComponent } from './components/main-layout/main-layout.component';
import { ListRessourcesComponent } from './components/list-ressources/list-ressources.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import {MatTableModule} from '@angular/material/table';
import {MatPaginator, MatPaginatorModule} from '@angular/material/paginator';
import {MatButtonModule} from '@angular/material/button';
import {TooltipPosition, MatTooltipModule} from '@angular/material/tooltip';
import {MatTabsModule} from '@angular/material/tabs';
import { ListeBesoinComponent } from './components/liste-besoin/liste-besoin.component';
import { SendBesoinsComponent } from './components/send-besoins/send-besoins.component';

import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {FormsModule} from '@angular/forms';
import { AppelOffresComponent } from './components/appel-offres/appel-offres.component';
import {MatExpansionModule} from '@angular/material/expansion';
import { OffreFournisseurComponent } from './components/offre-fournisseur/offre-fournisseur.component';
import { AcceptedFournisseurOffreComponent } from './components/accepted-fournisseur-offre/accepted-fournisseur-offre.component';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {provideNativeDateAdapter} from '@angular/material/core';
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    MainLayoutComponent,
    ListRessourcesComponent,
    SidebarComponent,
    ListeBesoinComponent,
    SendBesoinsComponent,
    AppelOffresComponent,
    OffreFournisseurComponent,
    AcceptedFournisseurOffreComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatTooltipModule,
    MatTabsModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatFormFieldModule
  ],
  providers: [
    provideAnimationsAsync(),
    provideNativeDateAdapter()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
