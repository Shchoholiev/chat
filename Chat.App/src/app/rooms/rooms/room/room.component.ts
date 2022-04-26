import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
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
export class RoomComponent implements OnInit, OnDestroy {

  public room: Room = new Room;

  public pageSize = 15;

  public pageNumber = 1;

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

      await this.delay(100);
      var element = document.getElementById('scroll');
      if (element) {
        element.scrollTop = element.scrollHeight;
      }
    });
  }

  public loadNext(){
    var messageToScroll = this.signalrService.messages[0];
    this.getMessages(this.pageNumber + 1);
    var element = document.getElementById(`message-${messageToScroll?.id}`);
    if (element) {
      element.scrollIntoView();          
    }
  }

  public getMessages(pageNumber: number){
    this._messagesService.getPage(this.pageSize, pageNumber, this.room.id).subscribe(
      response => {
        this.signalrService.messages = (response.body as Message[]).concat(this.signalrService.messages);

        var metadata = response.headers.get('x-pagination');
        if (metadata) {
          var object = JSON.parse(metadata);
          this.pageNumber = Number(object.PageNumber);
        }
      }
    )
  }

  public get roomName(){
    return this.room.users.find(u => u.email != this.authService.email)?.name;
  }

  private delay = (ms: number) => new Promise(res => setTimeout(res, ms));

  async ngOnDestroy(): Promise<void> {
    await this.signalrService.disconnect();
  }
}
