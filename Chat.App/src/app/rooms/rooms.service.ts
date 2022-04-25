import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { Room } from '../shared/room.model';
import { CreateGroupComponent } from './rooms/create-group/create-group.component';
import { MatDialog } from '@angular/material/dialog';
import { UsersInGroupComponent } from './rooms/users-in-group/users-in-group.component';
import { User } from '../shared/user.model';
import { CreateDialogueComponent } from './rooms/create-dialogue/create-dialogue.component';

@Injectable({
  providedIn: 'root'
})
export class RoomsService {

  private readonly _baseURL = 'https://localhost:7083/api/rooms';

  public rooms: Room[] = [];
  
  public metadata: string | null;

  public pageSize = 15;

  constructor(private _http: HttpClient, private _dialog: MatDialog) { }

  public refresh(){
    this.rooms = [];
    this.getPage(1);
  }

  public getRoom(id: number){
    return this._http.get<Room>(`${this._baseURL}/${id}`);
  }

  public getPage(pageNumber: number){
    this._http.get(this._baseURL, { params: { 
                                              pageSize: this.pageSize, 
                                              pageNumber: pageNumber
                                            }, observe: 'response' }
    ).subscribe(
      response => {
        this.rooms = this.rooms.concat(response.body as Room[]);
        this.metadata = response.headers.get('x-pagination');
      }
    )
  }

  public openGroupDialog(){
    this._dialog.open(CreateGroupComponent);
  }

  public openUsersInGroupDialog(users: User[]){
    this._dialog.open(UsersInGroupComponent, {
      data: { users: users },
    });
  }

  public openDialogueDialog(){
    this._dialog.open(CreateDialogueComponent);
  }

  public create(room: Room){
    this._http.post(this._baseURL, room).subscribe(
      () => {
        this.refresh();
        this._dialog.closeAll();
      }
    );
  }
}
