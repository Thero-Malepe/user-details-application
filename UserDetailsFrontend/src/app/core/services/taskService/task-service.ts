import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Task } from '../../models/task.model';
import { TaskModel } from '../../models/taskDto.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  public apiUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) { }

  getTasks(): Observable<Task[]> {
    return this.http.get<Task[]>(
      `${this.apiUrl}/Task/get-tasks`
    );
  }

  getTaskById(id: Number): Observable<Task> {
    return this.http.get<Task>(
      `${this.apiUrl}/Task/get-task/${id}`
    );
  }

  getTasksByUserId(): Observable<Task[]> {
    return this.http.get<Task[]>(
      `${this.apiUrl}/Task/get-task-by-user/`
    );
  }

  CreateTask(task: TaskModel): Observable<Task> {
    return this.http.post<Task>(
      `${this.apiUrl}/Task/add-task`, task
    );
  }

  updateTask(id: Number, taskDetails: TaskModel): Observable<Task> {
    return this.http.put<Task>(
      `${this.apiUrl}/Task/update-task/${id}`,
      taskDetails
    );
  }

  deleteTask(id: Number): Observable<any> {
    return this.http.delete<any>(
      `${this.apiUrl}/Task/delete-task/${id}`
    );
  }
}
