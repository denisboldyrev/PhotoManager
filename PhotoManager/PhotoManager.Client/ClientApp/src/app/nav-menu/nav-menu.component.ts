import { Component, OnInit } from '@angular/core';
import { AuthService } from '../core/services/auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  public isUserAuthenticated = false;
  constructor(private authService: AuthService) { }
  ngOnInit(): void {
    this.authService.loginChanged
    .subscribe(res => {
      this.isUserAuthenticated = res;
    });
  }
  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  public login = () => {
    this.authService.login();
  }
  public logout = () => {
    this.authService.logout();
  }
}
