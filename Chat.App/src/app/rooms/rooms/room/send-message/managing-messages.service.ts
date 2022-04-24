import { DOCUMENT } from '@angular/common';
import { Inject, Injectable } from '@angular/core';
import { MessagesService } from 'src/app/rooms/messages.service';
import { MessageDTO } from 'src/app/shared/message-dto.model';
import { Message } from 'src/app/shared/message.model';

@Injectable({
  providedIn: 'root'
})
export class ManagingMessagesService {

  public message: MessageDTO = new MessageDTO("", 0, 0);

  public replyMessage: Message = new Message;

  public editMessage: Message = new Message;

  constructor(private _messagesService: MessagesService, @Inject(DOCUMENT) document: Document) { }
  
  public cancelReply(){
    this.replyMessage = new Message;
    this.message.repliedTo = 0;
  }

  public cancelEdit(){
    this.editMessage = new Message;
    this.message.text = "";
  }

  public edit(message: Message){
    this.editMessage = message;
    this.message.text = message.text;
    if (this.replyMessage) {
      this.cancelReply();
    }
  }

  public reply(message: Message){
    this.replyMessage = message;
    this.message.repliedTo = message.id;
    if (this.editMessage) {
      this.cancelEdit();
    }
  }

  public send(){
    if (!this.message.repliedTo && this.editMessage?.id) {
      this._messagesService.edit(this.editMessage.id, this.message);
      this.cancelEdit();
    } else {
      this._messagesService.send(this.message);
      if (this.replyMessage) {
        this.cancelReply();
      }
    }
    this.message.text = "";
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
