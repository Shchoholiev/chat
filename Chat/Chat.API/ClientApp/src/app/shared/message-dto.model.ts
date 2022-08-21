export class MessageDTO {
    text: string = "";
    roomId: number = 0;
    repliedTo: number = 0;

    constructor(text: string, roomId: number, repliedTo: number) {
        this.text = text;
        this.roomId = roomId;
        this.repliedTo = repliedTo;
    }
}
