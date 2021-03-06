import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../account.service';
import { Login } from './login.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  public user: Login = new Login();

  private returnUrl: string = '';

  constructor(private _route: ActivatedRoute, private _router: Router, 
              private _accountService: AccountService) { }

  onSubmit(){
    this._accountService.login(this.user);
  }

  ngOnInit(): void {
    this.user.returnUrl = this._route.snapshot.queryParams['returnUrl'] || '/';
  }

}
