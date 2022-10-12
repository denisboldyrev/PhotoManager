import { Injectable } from '@angular/core';
import { UserManager, User, UserManagerSettings } from 'oidc-client';
import { Subject } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private userManager: UserManager;
  private user: User;
  private loginChangedSubject = new Subject<boolean>();
  public loginChanged = this.loginChangedSubject.asObservable();
  private get idpSettings(): UserManagerSettings {
    return {
      authority: environment.authority,
      client_id: environment.clientId,
      redirect_uri: `${environment.clientUrl}/signin-callback`,
      scope: 'openid profile api-scope roles',
      response_type: 'code',
      post_logout_redirect_uri: `${environment.clientUrl}/signout-callback`
    };
  }

  constructor() {
    this.userManager = new UserManager(this.idpSettings);
  }
  public login() {
    return this.userManager.signinRedirect();
  }
  public isAuthenticated(): Promise<boolean> {
    return this.userManager.getUser()
    .then(user => {
      if (this.user !== user) {
        this.loginChangedSubject.next(this.checkUser(user));
      }
      this.user = user;
      return this.checkUser(user);
    });
  }
  public finishLogin(): Promise<User> {
    return this.userManager.signinRedirectCallback()
    .then(user => {
      this.user = user;
      this.loginChangedSubject.next(this.checkUser(user));
      return user;
    });
  }
  public logout() {
    this.userManager.signoutRedirect();
    return this.userManager.signinRedirect();
  }
  public finishLogout() {
    this.user = null;
    return this.userManager.signoutRedirectCallback();
  }
  public getIdToken(): Promise<string> {
    return this.userManager.getUser()
      .then(user => {
        return !!user && !user.expired ? user.id_token : null;
    });
  }
  public get userInfo(): User {
    return this.user;
  }
  private checkUser(user: User): boolean {
    return !!user && !user.expired;
  }
}
