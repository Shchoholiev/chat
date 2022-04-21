import { Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/shared/message.model';

@Component({
  selector: 'app-server-message',
  templateUrl: './server-message.component.html',
  styleUrls: ['./server-message.component.css']
})
export class ServerMessageComponent implements OnInit {

  @Input() message: Message;

  constructor() { }

  ngOnInit(): void {
  }

  getTime(){
    return (new Date(this.message.sendDate)).toLocaleTimeString('uk-UA', { hour: '2-digit', minute: '2-digit'});
  }
}
