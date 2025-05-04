import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { RoomsService } from '../rooms.service';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.css']
})
export class RoomsComponent implements OnInit {

  public chosenRoomId: number = 0;

  constructor(public roomsService: RoomsService, private _router: Router) { }

  ngOnInit(): void {
    this.roomsService.refresh();
    this.chosenRoomId = +this._router.url.split('/')[2];
    this._router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.chosenRoomId = +this._router.url.split('/')[2];
      }
    });
  }
}
