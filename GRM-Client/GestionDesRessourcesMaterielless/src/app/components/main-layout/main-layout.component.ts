import { AfterViewInit, Component, ElementRef, OnInit, QueryList, Renderer2, ViewChild, ViewChildren } from '@angular/core';
import { SidebarComponent } from '../sidebar/sidebar.component';

@Component({
  selector: 'app-main-layout',
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.scss'
})
export class MainLayoutComponent implements AfterViewInit, OnInit {
  @ViewChild(SidebarComponent, { static: true }) sidebarComponent!: SidebarComponent;

  constructor(private elementRef: ElementRef) {}

  ngOnInit(): void {
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
}