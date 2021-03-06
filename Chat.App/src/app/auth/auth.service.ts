import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { RoomsService } from '../rooms/rooms.service';
import { Tokens } from './tokens.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _jwtHelper: JwtHelperService, private _router: Router,
              private _roomsService: RoomsService) { }

  get name(){
    var token = localStorage.getItem("jwt");
    if (token != null) {
      var decodedToken = this._jwtHelper.decodeToken(token);
      return decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
    }
  }

  get email(){
    var token = localStorage.getItem("jwt");
    if (token != null) {
      var decodedToken = this._jwtHelper.decodeToken(token);
      return decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'];
    }
  }

  get isAuthenticated(){
    var token = localStorage.getItem("jwt");
    return token && !this._jwtHelper.isTokenExpired(token);
  }

  login(token: Tokens){
    localStorage.setItem('jwt', token.accessToken);
    localStorage.setItem('refreshToken', token.refreshToken);
    this._roomsService.rooms = [];
  }

  logout(){
    localStorage.removeItem('jwt');
    localStorage.removeItem('refreshToken');
    this._router.navigate(['account/login']);
  }
}
