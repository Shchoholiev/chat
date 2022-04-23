import { HttpClient, HttpHeaders, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { Tokens } from './tokens.model';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private _authService: AuthService, private _router: Router, 
              private _jwtHelper: JwtHelperService, private _http: HttpClient){}

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    var accessToken = localStorage.getItem("jwt");
    if (accessToken && !this._jwtHelper.isTokenExpired(accessToken)) {
      return of(true);
    }

    return this.refreshTokens(accessToken, state);
  }

  private refreshTokens(accessToken: string | null, state: RouterStateSnapshot): Observable<boolean> {
    var refreshToken = localStorage.getItem("refreshToken");
    if (!accessToken || !refreshToken) { 
      this._router.navigate(['/account/login'], { queryParams: { returnUrl: state.url }});
      return of(false);
    }
    
    var tokens = new Tokens(accessToken, refreshToken);
    return this._http.post("https://localhost:7083/api/token/refresh", tokens, { observe: 'response' }).pipe(
      map((response) => {
        this._authService.login((<any>response).body);
        return true;
      }),
      catchError(err => {
        return this._router.navigate(['/account/login'], { queryParams: { returnUrl: state.url }});
      })
    )
  }
}
