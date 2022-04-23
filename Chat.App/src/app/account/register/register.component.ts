import { Component, OnInit } from '@angular/core';
import { AccountService } from '../account.service';
import { Register } from './register.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  public user: Register = new Register();

  constructor(private _accountService: AccountService) { }

  onSubmit(){
    this._accountService.register(this.user);
  }

  ngOnInit(): void {
  }

}
