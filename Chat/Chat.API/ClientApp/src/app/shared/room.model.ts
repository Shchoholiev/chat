import { User } from "./user.model";

export class Room {
    id: number = 0;
    displayName: string | null = "";
    users: User[] = [];
}
