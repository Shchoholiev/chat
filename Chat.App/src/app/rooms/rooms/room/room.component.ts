import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../auth/auth.service';
import { Message } from '../../../shared/message.model';
import { Room } from '../../../shared/room.model';
import { SignalrService } from '../../signalr.service';
import { MessagesService } from '../../messages.service';
import { RoomsService } from '../../rooms.service';
import { ManagingMessagesService } from './send-message/managing-messages.service';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.css']
})
export class RoomComponent implements OnInit {

  public room: Room = new Room;

  public metadata: string | null;

  public pageSize = 9;

  constructor(public roomsService: RoomsService, private _route: ActivatedRoute, 
              public signalrService: SignalrService, private _messagesService: MessagesService,
              public authService: AuthService, public managingMessages: ManagingMessagesService) { }

  async ngOnInit(): Promise<void> {
    await this.signalrService.connect();

    this._route.params.subscribe(async params => {
      var oldRoomId = this.room?.id?.toString() || '0';
      var id = params['id'];
      await this.signalrService.chooseChat(id.toString(), oldRoomId);
      this.roomsService.getRoom(id).subscribe(
        response => {
          this.room = response;
          this.signalrService.messages = [];
          this.getMessages(1);
        } 
      );

      this.managingMessages.clearAll();
    });
  }

  getMessages(pageNumber: number){
    this._messagesService.getPage(this.pageSize, pageNumber, this.room.id).subscribe(
      response => {
        this.signalrService.messages = this.signalrService.messages.concat(response.body as Message[]);
        this.metadata = response.headers.get('x-pagination');
      }
    )
  }

  get roomName(){
    return this.room.users.find(u => u.email != this.authService.email)?.name;
  }
}
