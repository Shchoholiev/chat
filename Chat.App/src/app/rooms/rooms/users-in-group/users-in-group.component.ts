import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { User } from 'src/app/shared/user.model';

@Component({
  selector: 'app-users-in-group',
  templateUrl: './users-in-group.component.html',
  styleUrls: ['./users-in-group.component.css']
})
export class UsersInGroupComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: { users: User[] }) { }

  ngOnInit(): void {
  }

}
