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
}
