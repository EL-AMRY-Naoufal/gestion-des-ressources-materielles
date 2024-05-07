import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Route } from '@angular/router';
import { Router } from 'express';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private readonly baseUrl:string = "https://localhost:7159/api/User"
  constructor(private http:HttpClient, private router:Router) { }
  
}
