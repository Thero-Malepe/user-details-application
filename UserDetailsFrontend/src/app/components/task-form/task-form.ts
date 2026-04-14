import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Priority } from '../../core/enums/priority.enum';
import { Status } from '../../core/enums/status.enum';
import { Task } from '../../core/models/task.model';
import { TaskModel } from '../../core/models/taskDto.model';
import { TaskService } from '../../core/services/taskService/task-service';
import { LoaderService } from '../../core/services/loaderService/loader-service';

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
  minDate = new Date().toString().split("T")[0];
  taskId: string | null = '';

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
      this.taskId = params.get('taskId');
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
    this.taskService.getTaskById(+taskId).subscribe((response) =>{
      this.task = response;
      if (this.task) {
        this.form.patchValue({
          title: this.task.title,
          description: this.task.description,
          status: this.task.status,
          priority: this.task.priority,
          dueDate: this.task.dueDate?.toString().split("T")[0]
        });
      }
    });    
  }

  cancel() {
    this.router.navigate(['/home']);
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

      this.task.createdAt = this.isEditMode? this.task.createdAt : new Date();
      this.task.description = this.form.get("description")?.value;     
      this.task.dueDate = this.form.get("dueDate")?.value;
      this.task.title = this.form.get("title")?.value;
      
      if(this.isEditMode)
      {
        this.taskService.updateTask(this.task.id ,this.task).subscribe(() =>{
          alert('Task updated');
          this.router.navigate(['/home']);
        });
      }
      else{
        this.taskService.CreateTask(this.task).subscribe(() =>{
          alert('Task created');
          this.router.navigate(['/home']);
        });
      }
      
    }else{
      alert('Form Invalid');
    }    
  }
}
