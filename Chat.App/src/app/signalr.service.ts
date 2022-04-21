import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { MessageDTO } from './shared/message-dto.model';
import { HttpClient } from '@angular/common/http';
import { Message } from './shared/message.model';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  private _hubConnection: HubConnection

  private readonly _hubURL = 'https://localhost:7083/chat';

  private readonly _apiURL = 'https://localhost:7083/api/chat';

  public messages: Message[] = [];

  constructor(private _http: HttpClient) { }

  public async chooseChat(roomId: number){
    await this._hubConnection.invoke("ChooseChat", roomId);
  }

  public sendMessage(message: MessageDTO) {
    this._http.post(this._apiURL + '/send', message).subscribe(res => console.log("message sent"))
  }
  
  public async connect() {
    await this.startConnection();
    this.addListeners();
  }

  private addListeners() {
    this._hubConnection.on("MessageSent", (data: Message) => {
      this.messages.unshift(data);
    });
  }

  private async startConnection() {
    this._hubConnection = this.getConnection();
    await this._hubConnection.start();
  }

  private getConnection() {
    return new HubConnectionBuilder()
      .withUrl(this._hubURL + `?access-token=${localStorage.getItem('jwt')}`)
      .build();
  }
}
