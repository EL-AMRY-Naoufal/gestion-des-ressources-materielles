import { AfterViewInit, Component, ElementRef, OnInit, QueryList, Renderer2, ViewChild, ViewChildren } from '@angular/core';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { LoginService } from '../../services/login/login.service';
import { TooltipPosition } from '@angular/material/tooltip';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MainLayoutService } from '../../services/mainLayout/main-layout.service';

@Component({
  selector: 'app-main-layout',
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.scss'
})
export class MainLayoutComponent implements AfterViewInit, OnInit {
  @ViewChild(SidebarComponent, { static: true }) sidebarComponent!: SidebarComponent;

  positionOptions: TooltipPosition[] = ['below', 'above', 'left', 'right'];
  position = new FormControl(this.positionOptions[0]);

  role : string = ""

  constructor(private elementRef: ElementRef, private loginService: LoginService, private mainLayoutService: MainLayoutService) {}

  ngOnInit(): void {
    this.role = this.loginService.getRole();
    this.sidebarComponent.sidebarStatusChanged.subscribe(({ isOpen, width }) => {
      this.updateNavBarWidth(isOpen, width);
    });
  }

  ngAfterViewInit(): void {
    this.sidebarComponent.sidebarStatusChanged.subscribe(({ isOpen, width }) => {
      this.updateNavBarWidth(isOpen, width);
    });
  }

  private updateNavBarWidth(isSidebarOpen: boolean, navBarWidth: number): void {
    const navBarElement = this.elementRef.nativeElement.querySelector('#nav-bar');
    if (navBarElement) {
      navBarElement.style.width = `${navBarWidth}px`;
    }
  }

  sendDemandeBesoinToEnseignants() {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    const departementId = user.departement.departmentId; 
    this.mainLayoutService.sendDemandeBesoinToEnseignants(departementId).subscribe({
      next: (response) => {
        console.log(response); 
      },
      error: (error) => {
        console.error(error); 
    }});
   
  }
}