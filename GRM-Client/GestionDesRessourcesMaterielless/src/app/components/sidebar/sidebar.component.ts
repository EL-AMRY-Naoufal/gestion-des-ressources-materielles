import {
  Component,
  ElementRef,
  EventEmitter,
  OnInit,
  Output,
} from '@angular/core';
import { LoginService } from '../../services/login/login.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
})
export class SidebarComponent implements OnInit {
  @Output() sidebarStatusChanged = new EventEmitter<{
    isOpen: boolean;
    width: number;
  }>();

  isSidebarOpen = false;

  currentUser: any;

  constructor(private elementRef: ElementRef, private loginService: LoginService) {}

  ngOnInit(): void {
    this.currentUser = this.loginService.getUser();
    console.log(this.currentUser)
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
}
