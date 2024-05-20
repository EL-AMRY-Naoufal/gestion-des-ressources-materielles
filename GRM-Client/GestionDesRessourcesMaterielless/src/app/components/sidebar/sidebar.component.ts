import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { LoginService } from '../../services/login/login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
})
export class SidebarComponent implements OnInit {
  
  sidebarContent: any;
  @Output() sidebarStatusChanged = new EventEmitter<{
    isOpen: boolean;
    width: number;
  }>();

  isSidebarOpen = false;

  currentUser: any;


  constructor(private elementRef: ElementRef, private loginService: LoginService, private router: Router) {}

  ngOnInit(): void {
    this.currentUser = this.loginService.getUser();
   
    this.sidebarContent = this.loginService.getSidebar()
    console.log(this.sidebarContent);
    this.sidebarStatusChanged.emit({
      isOpen: this.isSidebarOpen,
      width: this.getBodyWidth(),
    });
    
  }

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
    setTimeout(() => {
      this.emitSidebarStatusAndWidth();
    }, 200);
  }

  private emitSidebarStatusAndWidth(): void {
    const width = this.getBodyWidth();
    this.sidebarStatusChanged.emit({ isOpen: this.isSidebarOpen, width });
  }

  private getBodyWidth(): number {
    const bodyElement = this.elementRef.nativeElement.querySelector('#nav-bar');
    return bodyElement.offsetWidth + 30;
  }

  redirectToPage(url: string) {
    window.location.href = url;
  }
  logout() {
      localStorage.clear();
      this.router.navigate(["login"]);
  }
}
