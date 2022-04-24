import { User } from "./user.model";

export class Message {
    id: number = 0;
    text: string = "";
    sendDate: Date;
    sender: User;
    repliedTo: Message;
    hideForSender: boolean;
}
