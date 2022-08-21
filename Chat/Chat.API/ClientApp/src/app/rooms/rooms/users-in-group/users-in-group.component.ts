import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AccountService } from 'src/app/account/account.service';
import { User } from 'src/app/shared/user.model';
import { RoomsService } from '../../rooms.service';

@Component({
  selector: 'app-users-in-group',
  templateUrl: './users-in-group.component.html',
  styleUrls: ['./users-in-group.component.css']
})
export class UsersInGroupComponent implements OnInit {

  public error: string = "";

  public email: string = "";

  constructor(@Inject(MAT_DIALOG_DATA) public data: { users: User[], roomId: number }, 
              private _usersService: AccountService, private _roomsService: RoomsService) { }

  ngOnInit(): void {
  }

  public onSubmit(){
    if (this.data.users.find(u => u.email == this.email)) {
      this.error = "This user is already added!";
      this.email = "";
      return;
    }
    this._usersService.getUser(this.email).subscribe(
      response => {
        if (response) {
          this._roomsService.addMember(this.email, this.data.roomId);
          this.data.users.push(response as User);
          this.email = "";
        }
        else{
          this.error = "User with this email doesn't exist."
        }
      }
    );
  }
}
