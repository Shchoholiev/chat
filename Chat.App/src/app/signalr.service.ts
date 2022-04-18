import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { MessageDTO } from './shared/message-dto.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  private _hubConnection: HubConnection

  public messages: MessageDTO[] = [];

  private connectionUrl = 'https://localhost:7083/chat';

  private readonly apiUrl = 'https://localhost:7083/api/chat';

  constructor(private _http: HttpClient) { }

  public connect = () => {
    this.startConnection();
    this.addListeners();
  }

  public sendMessageToUser(message: MessageDTO) {
    this._http.post(this.apiUrl + '/to-user', message).subscribe(res => alert("Message sent!"))
  }

  private addListeners() {
    this._hubConnection.on("MessageSentToUser", (data: MessageDTO) => {
      alert(data.text);
      this.messages.push(data);
    });
  }

  private startConnection() {
    this._hubConnection = this.getConnection();

    this._hubConnection.start()
      .then(() => console.log('connection started'))
      .catch((err) => console.log('error while establishing signalr connection: ' + err))
  }

  private getConnection(): HubConnection {
    return new HubConnectionBuilder()
      .withUrl(this.connectionUrl + `?access-token=${localStorage.getItem('jwt')}`)
      // .withHubProtocol(new MessagePackHubProtocol())
      //  .configureLogging(LogLevel.Trace)
      .build();
  }
}
