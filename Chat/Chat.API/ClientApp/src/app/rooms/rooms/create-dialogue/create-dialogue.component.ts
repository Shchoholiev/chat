import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/account/account.service';
import { AuthService } from 'src/app/auth/auth.service';
import { Room } from 'src/app/shared/room.model';
import { User } from 'src/app/shared/user.model';
import { RoomsService } from '../../rooms.service';

@Component({
  selector: 'app-create-dialogue',
  templateUrl: './create-dialogue.component.html',
  styleUrls: ['./create-dialogue.component.css']
})
export class CreateDialogueComponent implements OnInit {

  public email: string = "";

  public error: string = "";

  constructor(private _roomsService: RoomsService, private _usersService: AccountService,
              private _authService: AuthService) { }

  ngOnInit(): void { }

  public onSubmit(){
    if (this.email == this._authService.email) {
      this.error = "You cant create dialogue with yourself."
      return;
    }
    this._usersService.getUser(this.email).subscribe(
      response => {
        if (response) {
          var room = new Room;
          room.displayName = null;
          room.users.push(response as User);
          this._usersService.getUser(this._authService.email).subscribe(
            response => {
              room.users.push(response as User);
              this._roomsService.create(room);
            }
          );
          this.email = "";
        }
        else{
          this.error = "User with this email doesn't exist."
        }
      }
    );
  }
}
