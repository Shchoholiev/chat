import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';
import { Login } from './login/login.model';
import { Register } from './register/register.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private readonly baseURL = '/api/account';

  constructor(private _http: HttpClient, private _authService: AuthService, private _router: Router) { }

  public register(form: Register){
    this._http.post<any>(this.baseURL + '/register', form).subscribe(
      response => {
        this._authService.login(response);
        this._router.navigate(['rooms']);
      }
    );
  }

  public login(form: Login){
    this._http.post<any>(this.baseURL + '/login', form).subscribe(
      response => {
        this._authService.login(response);
        this._router.navigate([form.returnUrl]);
      }
    );
  }

  public getUser(email: string){
    return this._http.get(`${this.baseURL}/${email}`);
  }
}
