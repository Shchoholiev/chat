import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { MessageDTO } from '../shared/message-dto.model';
import { RoomsService } from './rooms.service';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {

  private readonly _baseURL = 'https://localhost:7083/api/messages';

  constructor(private _http: HttpClient, private _roomsService: RoomsService) { }

  public getPage(pageSize: number, pageNumber: number, roomId: number){
    return this._http.get(`${this._baseURL}/${roomId}`, { params: { 
                                                          pageSize: pageSize, 
                                                          pageNumber: pageNumber
                                                        }, observe: 'response' });
  }

  public send(message: MessageDTO) {
    this._http.post(this._baseURL, message).subscribe()
  }

  public edit(id: number, message: MessageDTO){
    this._http.put(`${this._baseURL}/${id}`, message).subscribe();
  }

  public hide(id: number){
    this._http.put(`${this._baseURL}/hide/${id}`, {}).subscribe();
  }

  public delete(messageId: number){
    this._http.delete(`${this._baseURL}/${messageId}`).subscribe();
  }

  public replyInPerson(recipientEmail: string, message: MessageDTO){
    this._http.post(`${this._baseURL}/replyInPerson/${recipientEmail}`, message).subscribe(
      () => {
        if (!(this._roomsService.rooms.find(r => r.displayName == null && 
                                            r.users.find(u => u.email == recipientEmail)))) {
          this._roomsService.refresh();
        }
      }
    )
  }
}
