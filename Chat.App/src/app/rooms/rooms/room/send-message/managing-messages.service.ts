import { Injectable } from '@angular/core';
import { MessagesService } from 'src/app/rooms/messages.service';
import { RoomsService } from 'src/app/rooms/rooms.service';
import { MessageDTO } from 'src/app/shared/message-dto.model';
import { Message } from 'src/app/shared/message.model';

@Injectable({
  providedIn: 'root'
})
export class ManagingMessagesService {

  public message: MessageDTO = new MessageDTO("", 0, 0);

  public replyMessage: Message = new Message;

  public inPersonMessage: Message = new Message;

  public editMessage: Message = new Message;

  constructor(private _messagesService: MessagesService) { }

  public edit(message: Message){
    if (this.replyMessage) {
      this.cancelReply();
    }
    if (this.inPersonMessage) {
      this.cancelReplyInPerson();
    }
    this.editMessage = message;
    this.message.text = message.text;
  }

  public reply(message: Message){
    if (this.editMessage) {
      this.cancelEdit();
    }
    if (this.inPersonMessage) {
      this.cancelReplyInPerson();
    }
    this.replyMessage = message;
    this.message.repliedTo = message.id;
  }

  public replyInPerson(message: Message){
    if (this.editMessage) {
      this.cancelEdit();
    }
    if (this.replyMessage) {
      this.cancelReply();
    }
    this.inPersonMessage = message;
    this.message.repliedTo = message.id;
  }

  public async send(){
    if (this.inPersonMessage?.id) {
      this._messagesService.replyInPerson(this.inPersonMessage.sender.email, this.message);
      this.cancelReplyInPerson();
    }
    else{
      if (!this.message.repliedTo && this.editMessage?.id) {
        this._messagesService.edit(this.editMessage.id, this.message);
        this.cancelEdit();
      } else {
        this._messagesService.send(this.message);
        if (this.replyMessage) {
          this.cancelReply();
        }

        await this.delay(2);
        var element = document.getElementById('scroll');
        if (element) {
          element.scrollTop = element.scrollHeight;
        }
      }
    }
    this.message.text = "";
  }

  public cancelReply(){
    this.replyMessage = new Message;
    this.message.repliedTo = 0;
  }
  
  public cancelReplyInPerson(){
    this.inPersonMessage = new Message;
    this.message.repliedTo = 0;
  }

  public cancelEdit(){
    this.editMessage = new Message;
    this.message.text = "";
  }

  public clearAll(){
    this.cancelEdit();
    this.cancelReply();
    this.cancelReplyInPerson();
  }

  public async toMessage(messageId: number) {
    var element = document.getElementById(`message-${messageId}`);
    if (element) {
      this.scroll(element);
      element.classList.add('highlight');
      await this.delay(500);
      element.classList.remove('highlight');
    }
  }

  public scroll(element: HTMLElement) {
    element.scrollIntoView({behavior: 'smooth'});
  }

  private delay = (ms: number) => new Promise(res => setTimeout(res, ms));
}
