import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Room } from '../../shared/room.model';
import { RoomsService } from '../rooms.service';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.css']
})
export class RoomsComponent implements OnInit {

  public chosenRoomId: number = 0;

  public rooms: Room[] = [];
  
  public metadata: string | null;

  public pageSize = 10;

  constructor(private _roomsService: RoomsService, 
              private _router: Router) { }

  ngOnInit(): void {
    this.getPage(1);
    this.chosenRoomId = +this._router.url.split('/')[2];
    this._router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.chosenRoomId = +this._router.url.split('/')[2];
      }
    });
  }

  getPage(pageNumber: number){
    this._roomsService.getPage(this.pageSize, pageNumber).subscribe(
      response => {
        this.rooms = this.rooms.concat(response.body as Room[]);
        this.metadata = response.headers.get('x-pagination');
      }
    );
  }
}
