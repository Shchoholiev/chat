import { Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/shared/message.model';
import { ManagingMessagesService } from '../send-message/managing-messages.service';

@Component({
  selector: 'app-received-message',
  templateUrl: './received-message.component.html',
  styleUrls: ['./received-message.component.css']
})
export class ReceivedMessageComponent implements OnInit {

  @Input() message: Message;

  constructor(public managingMessagesService: ManagingMessagesService) { }

  ngOnInit(): void {
  }

  getTime(){
    return (new Date(this.message.sendDate)).toLocaleTimeString('uk-UA', { hour: '2-digit', minute: '2-digit'});
  }
}
