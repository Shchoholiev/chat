import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { ManagingMessagesService } from './managing-messages.service';

@Component({
  selector: 'app-send-message',
  templateUrl: './send-message.component.html',
  styleUrls: ['./send-message.component.css']
})
export class SendMessageComponent implements OnInit {

  constructor(public managingMessages: ManagingMessagesService, private _router: Router) { }

  ngOnInit(): void {
    this.managingMessages.message.roomId = +this._router.url.split('/')[2];
    this._router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.managingMessages.message.roomId = +this._router.url.split('/')[2];
      }
    });
  }
}
