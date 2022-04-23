import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { MessageDTO } from 'src/app/shared/message-dto.model';
import { SignalrService } from 'src/app/signalr.service';

@Component({
  selector: 'app-send-message',
  templateUrl: './send-message.component.html',
  styleUrls: ['./send-message.component.css']
})
export class SendMessageComponent implements OnInit {

  public message: MessageDTO = new MessageDTO;

  constructor(private _signalRService: SignalrService, private _router: Router) { }

  ngOnInit(): void {
    this.message.roomId = +this._router.url.split('/')[2];
    this._router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.message.roomId = +this._router.url.split('/')[2];
      }
    });
  }

  onSubmit(){
    this._signalRService.sendMessage(this.message);
    this.message.text = "";
  }
}
