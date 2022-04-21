import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { Login } from './login/login.model';
import { Register } from './register/register.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private readonly baseURL = 'https://localhost:7083/api/account';

  constructor(private http: HttpClient, public authService: AuthService) { }

  register(form: Register){
    this.http.post<any>(this.baseURL + '/register', form).subscribe(
      response => {
        this.authService.login(response.token);
      }
    );
  }

  login(form: Login){
    this.http.post<any>(this.baseURL + '/login', form).subscribe(
      response => {
        this.authService.login(response.token);
      }
    );
  }
}
