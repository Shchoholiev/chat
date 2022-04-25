import { Component, Input, OnInit } from '@angular/core';
import { MessagesService } from 'src/app/rooms/messages.service';
import { Message } from 'src/app/shared/message.model';
import { ManagingMessagesService } from '../send-message/managing-messages.service';

@Component({
  selector: 'app-sent-message',
  templateUrl: './sent-message.component.html',
  styleUrls: ['./sent-message.component.css']
})
export class SentMessageComponent implements OnInit {

  @Input() message: Message;

  constructor(public messagesService: MessagesService, public managingMessagesService: ManagingMessagesService) { }

  ngOnInit(): void {
  }

  getTime(){
    return (new Date(this.message.sendDate)).toLocaleTimeString('uk-UA', { hour: '2-digit', minute: '2-digit'});
  }
}
