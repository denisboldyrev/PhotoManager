import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../core/services/auth.service';

@Component({
  selector: 'app-signin-redirect-callback',
  templateUrl: './signin-redirect-callback.component.html',
  styleUrls: ['./signin-redirect-callback.component.css']
})
export class SigninRedirectCallbackComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private router: Router) { }
  ngOnInit(): void {
    this.authService.finishLogin()
    .then(() => {
      this.router.navigate(['/'], { replaceUrl: true });
    });
  }
}
