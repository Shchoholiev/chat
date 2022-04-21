import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'

@Injectable({
  providedIn: 'root'
})
export class MessagesService {

  private readonly _baseURL = 'https://localhost:7083/api/chat';

  constructor(private _http: HttpClient) { }

  getPage(pageSize: number, pageNumber: number, roomId: number){
    return this._http.get(`${this._baseURL}/${roomId}`, { params: { 
                                                          pageSize: pageSize, 
                                                          pageNumber: pageNumber
                                                        }, observe: 'response' });
  }
}
