import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private readonly baseUrl:string = "https://localhost:7159/api/User/"

  constructor(private http:HttpClient, private router:Router) { }
  
  login(loginObj:any){
    console.log("look here")
    return this.http.post<any>(`${this.baseUrl}authenticate`,loginObj);
  }

  storeToken(tokenValue: string){
    localStorage.setItem('token', tokenValue);
  }

  getToken(){
    return localStorage.getItem('token');
  }

  isLoggedIn() : boolean{
    return !!localStorage.getItem('token')
  }

  storeUser(user: any){
    localStorage.setItem('user', JSON.stringify(user));
  }

  getUser(){
    const userJSON = localStorage.getItem('user');
    return userJSON ? JSON.parse(userJSON) : null;
  }

  storeRole(role: any){
    localStorage.setItem('role', JSON.stringify(role));
  }

  getRole(){
    const userJSON = localStorage.getItem('role');
    return userJSON ? JSON.parse(userJSON) : null;
  }

  storeSidebar(sidebar: any[]) {
    const localSidebarData = [...sidebar]; // Make a copy of the sidebar data
    // You can perform any additional processing here if needed
    localStorage.setItem('sidebarData', JSON.stringify(localSidebarData)); // Store in local storage
  }

  // Method to retrieve the sidebar data from local storage
  getSidebar() {
    const storedData = localStorage.getItem('sidebarData');
    try {
      return storedData ? JSON.parse(storedData) : null; // Parse the stored data from local storage
    } catch (error) {
      console.error('Error parsing sidebar data:', error);
      return null;
    }
  }
}
