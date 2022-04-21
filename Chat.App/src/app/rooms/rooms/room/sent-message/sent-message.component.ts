import { Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/shared/message.model';

@Component({
  selector: 'app-sent-message',
  templateUrl: './sent-message.component.html',
  styleUrls: ['./sent-message.component.css']
})
export class SentMessageComponent implements OnInit {

  @Input() message: Message;

  constructor() { }

  ngOnInit(): void {
  }

  getTime(){
    return (new Date(this.message.sendDate)).toLocaleTimeString('uk-UA', { hour: '2-digit', minute: '2-digit'});
  }
}
