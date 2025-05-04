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

  private readonly _baseURL = '/api/rooms';

  public rooms: Room[] = [];

  public pageSize = 15;

  public pageNumber = 1;

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
        var metadata = response.headers.get('x-pagination');
        if (metadata) {
          var object = JSON.parse(metadata);
          this.pageNumber = Number(object.PageNumber);
        }
      }
    )
  }
  
  public create(room: Room){
    this._http.post(this._baseURL, room).subscribe(
      () => {
        this.refresh();
        this._dialog.closeAll();
      }
    );
  }

  public openGroupDialog(){
    this._dialog.open(CreateGroupComponent);
  }

  public openDialogueDialog(){
    this._dialog.open(CreateDialogueComponent);
  }

  public openUsersInGroupDialog(users: User[], roomId: number){
    this._dialog.open(UsersInGroupComponent, {
      data: { users: users, roomId: roomId },
    });
  }

  public addMember(email: string, roomId: number){
    this._http.put(this._baseURL + '/add-member', { email: email, roomId: roomId }).subscribe();
  }

  public loadNext(){
    this.getPage(this.pageNumber + 1);
  }
}
