import { Priority } from "../enums/priority.enum";
import { Status } from "../enums/status.enum";

export class Task {
    id: Number = 0;
    title: string = ''
    description: string = '';
    status: Status = Status.ToDo
    priority: Priority = Priority.Low;
    dueDate?: Date = undefined;
    createdAt: Date = new Date();
}