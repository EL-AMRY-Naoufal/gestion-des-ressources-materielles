import { Component } from '@angular/core';
import { LoginService } from '../services/login/login.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  loginForm!: FormGroup;

  constructor(private fb: FormBuilder, private loginService: LoginService) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onLogin(){
    if(this.loginForm.valid){
      this.loginService.login(this.loginForm.value).subscribe({
        next: (res)=> {
          console.log("niiiiiiiice");
        },
        error: (err) => {
          console.log(err);
        }
      })
    }
  }
}
