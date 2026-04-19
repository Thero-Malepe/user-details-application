import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Priority } from '../../core/enums/priority.enum';
import { Status } from '../../core/enums/status.enum';
import { Task } from '../../core/models/task.model';
import { TaskModel } from '../../core/models/taskDto.model';
import { TaskService } from '../../core/services/taskService/task-service';
import { LoaderService } from '../../core/services/loaderService/loader-service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-task-form',
  standalone: false,
  templateUrl: './task-form.html',
  styleUrl: './task-form.css'
})
export class TaskForm implements OnInit {
  isEditMode: boolean = false;
  form!: FormGroup;
  task: Task = new Task();
  taskModel: TaskModel = new TaskModel();
  taskId: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder, 
    private loader: LoaderService, 
    private taskService: TaskService
  ) { 
    const navigation = this.router.getCurrentNavigation();
    if (navigation?.extras.state) {
      this.isEditMode = navigation.extras.state['isEditMode'] || false;
    }
  }

  ngOnInit() { 
    this.loader.show();
    this.route.paramMap.subscribe(params => {
      this.taskId = params.get('taskId')!;
    });

    this.form = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(1)]],
      description: ['', [Validators.required, Validators.minLength(1)]],
      status: ['', Validators.required],
      priority: ['', Validators.required],
      dueDate: ['', Validators.required]
    });

    if (this.isEditMode) {
      this.loadTask(this.taskId!);
    }    

    setTimeout(() => {
        this.loader.hide();
      }, 
      4000
    );
  }

  loadTask(taskId: string) {
    this.taskService.getTaskById(+taskId).subscribe({
      next: (response) => {
        if (response) {
          this.form.patchValue({
            title: response.title,
            description: response.description,
            status: response.status,
            priority: response.priority,
            dueDate: response.dueDate?.toString().split("T")[0]
          });
        }
      },
      error: (error: HttpErrorResponse) => {
        if(error.status === 404)
          alert("Task not found.")
      }
    });
  }

  performTask()
  {
    if(!this.form.invalid)
    {
      switch(this.form.get("status")?.value)
      {
        case "1":
          this.task.status = Status.ToDo;
        break;
        case "2":
          this.task.status = Status.InProgress;
        break;
        case "3":
          this.task.status = Status.Done;
        break;
      }

      switch(this.form.get("priority")?.value)
      {
        case "1":
          this.task.priority = Priority.Low;
        break;
        case "2":
          this.task.priority = Priority.Medium;
        break;
        case "3":
          this.task.priority = Priority.High;
        break;
      }

      this.task.description = this.form.get("description")?.value;     
      this.task.dueDate = this.form.get("dueDate")?.value;
      this.task.title = this.form.get("title")?.value;
      
      if(this.isEditMode)
      {
        this.taskService.updateTask(+this.taskId ,this.task).subscribe({
          next: () => {
            this.loader.show();
            this.router.navigate(['/home']);
          },
          error: (error: HttpErrorResponse) => {
            if(error.status === 400 )
              alert("Form invalid");            
          }
        });
      }
      else{
        this.taskService.CreateTask(this.task).subscribe({
          next: () => {
            this.loader.show();
            this.router.navigate(['/home']);
          },
          error: (error: HttpErrorResponse) => {
            if(error.status === 400 )
              alert("Form invalid");
          }          
        });
      }      
    }   
  }
}
