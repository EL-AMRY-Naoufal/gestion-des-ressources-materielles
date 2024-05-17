import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginService } from '../../services/login/login.service';
import { NgToastService } from 'ng-angular-popup';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private loginService: LoginService,
    private toast: NgToastService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onLogin(): void {
    console.log(this.loginForm.value);
    this.loginService.login(this.loginForm.value).subscribe({
      next: (res) => {
        this.toast.success({
          detail: 'SUCCESS',
          summary: res.message,
          duration: 5000,
        });
        this.loginForm.reset();
        this.loginService.storeToken(res.token);
        this.loginService.storeUser(res.user);
        this.loginService.storeRole(res.role)
        this.router.navigate(["/mainLayout/listeRessources"])
      },
      error: (err) => {
        this.toast.error({
          detail: 'FAILED',
          summary: 'Incorrect email or password!',
          duration: 5000,
        });
        console.log(err);
      },
    });
  }
}
