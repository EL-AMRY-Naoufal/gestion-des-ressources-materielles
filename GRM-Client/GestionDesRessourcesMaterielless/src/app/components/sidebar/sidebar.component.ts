import { Component, ElementRef, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent implements OnInit {
  @Output() sidebarStatusChanged = new EventEmitter<{ isOpen: boolean, width: number }>();
  isSidebarOpen = false; // Assuming sidebar is initially open

  constructor(private elementRef: ElementRef) {}

  ngOnInit(): void {
    this.sidebarStatusChanged.emit({ isOpen: this.isSidebarOpen, width: this.getBodyWidth() });
  }

  toggleSidebar() {
    // Toggle the sidebar status
    this.isSidebarOpen = !this.isSidebarOpen;
    
    // Emit the updated sidebar status and width after a short delay
    setTimeout(() => {
      this.emitSidebarStatusAndWidth();
    }, 200); // Adjust the delay time as needed based on your sidebar animation duration
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