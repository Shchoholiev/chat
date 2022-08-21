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

  constructor(private _authService: AuthService,  
              private _jwtHelper: JwtHelperService, private _http: HttpClient){}

  ngOnInit(): void {
  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    var accessToken = localStorage.getItem("jwt");
    if (accessToken && !this._jwtHelper.isTokenExpired(accessToken)) {
      return of(true);
    }

    return this._authService.refreshTokens(state);
  }
}
