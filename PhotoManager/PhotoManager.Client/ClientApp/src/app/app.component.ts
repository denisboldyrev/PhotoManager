import { Component, OnInit } from '@angular/core';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent  implements OnInit {
  title = 'app';
  public userAuthenticated = false;
  constructor(private authService: AuthService) {
    this.authService.loginChanged.subscribe(userAuthenticated => {
      this.userAuthenticated = userAuthenticated;
    });
  }
  ngOnInit(): void {
    this.authService.isAuthenticated()
    .then(userAuthenticated => {
      this.userAuthenticated = userAuthenticated;
    });
  }
}
