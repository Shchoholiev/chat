import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/account/account.service';
import { AuthService } from 'src/app/auth/auth.service';
import { Room } from 'src/app/shared/room.model';
import { User } from 'src/app/shared/user.model';
import { RoomsService } from '../../rooms.service';

@Component({
  selector: 'app-create-group',
  templateUrl: './create-group.component.html',
  styleUrls: ['./create-group.component.css'],
})
export class CreateGroupComponent implements OnInit {

  public room: Room = new Room;

  public email: string = "";

  public error: string = "";

  constructor(private _roomsService: RoomsService, private _usersService: AccountService,
              private _authService: AuthService) { }

  ngOnInit(): void {
    this._usersService.getUser(this._authService.email).subscribe(
      response => this.room.users.push(response as User)
    );
  } 

  public onSubmit(){
    this._roomsService.create(this.room);
  }

  public addUser(){
    if (this.room.users.find(u => u.email == this.email)) {
      this.error = "This user is already added!";
      this.email = "";
      return;
    }
    this._usersService.getUser(this.email).subscribe(
      response => {
        if (response) {
          this.room.users.push(response as User);
          this.email = "";
        }
        else{
          this.error = "User with this email doesn't exist."
        }
      }
    );
  }
}
