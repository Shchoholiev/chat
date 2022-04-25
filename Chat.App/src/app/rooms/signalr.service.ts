import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { AuthService } from '../auth/auth.service';
import { Message } from '../shared/message.model';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  private _hubConnection: HubConnection

  private readonly _hubURL = 'https://localhost:7083/chat';

  public messages: Message[] = [];

  constructor(private _authService: AuthService) { }

  public async chooseChat(newRoomId: string, oldRoomId: string){
    await this._hubConnection.invoke("ChooseChat", newRoomId, oldRoomId);
  }
  
  public async connect() {
    await this.startConnection();
    this.addListeners();
  }

  public async disconnect(){
    await this._hubConnection.stop();
  }

  private addListeners() {
    this._hubConnection.on("MessageSent", (data: Message) => {
      this.messages.unshift(data);
    });
    this._hubConnection.on("MessageEdited", (data: Message) => {
      var message = this.messages.find(m => m.id == data.id);
      if (message) {
        var index = this.messages.indexOf(message);
        message.text = data.text;
        this.messages[index] = message;
      }
    });
    this._hubConnection.on("MessageHiddenForUser", (data: Message) => {
      if (data.sender.name == this._authService.name) {
        this.removeMessage(data.id);
      }
    });
    this._hubConnection.on("MessageDeleted", (data: number) => {
      this.removeMessage(data);
    });
  }

  private removeMessage(id: number){
    var message = this.messages.find(m => m.id == id);
    if (message) {
      var index = this.messages.indexOf(message);
      this.messages.splice(index, 1);
    }
  }

  private async startConnection() {
    this._hubConnection = new HubConnectionBuilder()
                          .withUrl(this._hubURL + `?access-token=${localStorage.getItem('jwt')}`)
                          .build();
    await this._hubConnection.start();
  }
}
