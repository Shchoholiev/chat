import { Component, OnInit } from '@angular/core';
import { MessageDTO } from 'src/app/shared/message-dto.model';
import { Message } from 'src/app/shared/message.model';
import { Room } from 'src/app/shared/room.model';
import { SignalrService } from 'src/app/signalr.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {

  public messages: Message[] = [];

  public roomId: number = 1;

  public message: MessageDTO = new MessageDTO;

  constructor(public signalRService: SignalrService) { }

  ngOnInit(): void {
    this.signalRService.connect();
  }

  sendMessage(){
    this.message.roomId = this.roomId;
    this.signalRService.sendMessageToUser(this.message);
  }
}
