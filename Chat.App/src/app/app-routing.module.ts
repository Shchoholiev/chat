import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './account/login/login.component';
import { RegisterComponent } from './account/register/register.component';
import { RoomComponent } from './rooms/rooms/room/room.component';
import { RoomsComponent } from './rooms/rooms/rooms.component';

const routes: Routes = [
  { path: 'account/register', component: RegisterComponent },
  { path: 'account/login', component: LoginComponent },
  { 
    path: 'rooms', 
    component: RoomsComponent,
    children: [{ path: ':id', component: RoomComponent}] },
  // { path: ':id', component: RoomComponent, outlet: 'rooms' },
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forRoot(routes),
    CommonModule
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
