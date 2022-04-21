import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MessageDTO } from 'src/app/shared/message-dto.model';
import { SignalrService } from 'src/app/signalr.service';

@Component({
  selector: 'app-send-message',
  templateUrl: './send-message.component.html',
  styleUrls: ['./send-message.component.css']
})
export class SendMessageComponent implements OnInit {

  public message: MessageDTO = new MessageDTO;

  constructor(private _signalRService: SignalrService, private _route: ActivatedRoute) { }

  ngOnInit(): void {
    this._route.firstChild?.paramMap.subscribe(() => {
      this.message.roomId = this._route.snapshot.firstChild?.params['id'];
     });
  }

  onSubmit(){
    this._signalRService.sendMessage(this.message);
    this.message.text = "";
  }
}
