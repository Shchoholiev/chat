import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavigationBarComponent } from './navigation-bar/navigation-bar.component';
import { RegisterComponent } from './account/register/register.component';
import { FormsModule } from '@angular/forms';
import { JwtModule } from '@auth0/angular-jwt';
import { LoginComponent } from './account/login/login.component';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './auth-interceptor';
import { RoomsComponent } from './rooms/rooms/rooms.component';
import { RoomComponent } from './rooms/rooms/room/room.component';
import { ReceivedMessageComponent } from './rooms/rooms/room/received-message/received-message.component';
import { SendMessageComponent } from './rooms/rooms/room/send-message/send-message.component';
import { SentMessageComponent } from './rooms/rooms/room/sent-message/sent-message.component';
import { ServerMessageComponent } from './rooms/rooms/room/server-message/server-message.component';

export function tokenGetter() {
  return localStorage.getItem("jwt");
}

@NgModule({
  declarations: [
    AppComponent,
    NavigationBarComponent,
    RegisterComponent,
    LoginComponent,
    RoomsComponent,
    RoomComponent,
    ReceivedMessageComponent,
    SendMessageComponent,
    SentMessageComponent,
    ServerMessageComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:7083"],
        disallowedRoutes: []
      }
    })
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }