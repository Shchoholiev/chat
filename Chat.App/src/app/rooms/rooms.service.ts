import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { Room } from '../shared/room.model';

@Injectable({
  providedIn: 'root'
})
export class RoomsService {

  private readonly _baseURL = 'https://localhost:7083/api/rooms';

  constructor(private _http: HttpClient) { }

  getPage(pageSize: number, pageNumber: number){
    return this._http.get(this._baseURL, { params: { 
                                            pageSize: pageSize, 
                                            pageNumber: pageNumber
                                          }, observe: 'response' });
  }

  getRoom(id: number){
    return this._http.get<Room>(`${this._baseURL}/${id}`);
  }
}
