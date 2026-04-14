import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Priority } from '../../core/enums/priority.enum';
import { Status } from '../../core/enums/status.enum';
import { TaskService } from '../../core/services/taskService/task-service';
import { Task } from '../../core/models/task.model';
import { LoaderService } from '../../core/services/loaderService/loader-service';

@Component({
  selector: 'app-task-list',
  standalone: false,
  templateUrl: './task-list.html',
  styleUrl: './task-list.css'
})
export class TaskList implements OnInit {
  filteredTasks: Task[] = [];
  tasks: Task[] = [];
  searchQuery: string = '';
  sortOrder: 'dueDate:asc' | 'dueDate:desc' = 'dueDate:asc';
  
  constructor(
    private taskService: TaskService,
    private loader: LoaderService, 
    private router: Router,) {}

  ngOnInit() {   
    this.loader.show();
    this.taskService.getTasks().subscribe((response) => {
      this.tasks = response;
      this.filteredTasks = [...this.tasks];
      this.applyFilters();
    });    

    setTimeout(() => {
        this.loader.hide();
      }, 
      4000
    );
  }

  navigate(){
    this.router.navigate(['/tasks/new']);
  }

  applyFilters(): void {
    let result = [...this.tasks];
    if (this.searchQuery.trim()) {
      const query = this.searchQuery.toLowerCase();
      result = result.filter(task =>
        task.title.toLowerCase().includes(query) ||
        task.description.toLowerCase().includes(query)
      );
    }

    // sort by due date
    result.sort((a, b) => {
      if (!a.dueDate && !b.dueDate) return 0;
      if (!a.dueDate) return 1;
      if (!b.dueDate) return -1;
      const date1 = new Date(a.dueDate).getTime();
      const date2 = new Date(b.dueDate).getTime();
      return this.sortOrder === 'dueDate:asc' ? date1 - date2 : date2 - date1;
    });

    this.filteredTasks = result;
  }

 
  onSearchChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.searchQuery = target.value;
    this.applyFilters();
  }
  
  onSortChange(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.sortOrder = target.value as 'dueDate:asc' | 'dueDate:desc';
    this.applyFilters();
  }

  editTask(taskId: Number): void {
    this.router.navigate([`/tasks/${taskId}`], {
      state: { isEditMode: true}
    });
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-ZA', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }


  deleteTask(taskId: Number): void {
    if (confirm('Are you sure you want to delete this task?')) {
      this.taskService.deleteTask(taskId).subscribe(() =>{
        this.tasks = this.tasks.filter(t => t.id !== taskId);
        this.applyFilters();
        alert('Task deleted');
      });
    }
  }


  trackByTaskId(index: number, task: Task): Number {
    return task.id;
  }

  getStatusClass(status: string): string {
    const statusClasses: { [key: string]: string } = {
      '1': 'bg-primary',
      '2': 'bg-warning text-dark',
      '3': 'bg-success'
    };
    return statusClasses[status] || 'bg-secondary';
  }

  getStatusLabel(status: string): string {
    const statusLabels: { [key: string]: string } = {
      '1': 'To Do',
      '2': 'In Progress',
      '3': 'Done'
    };
    return statusLabels[status] || status;
  }

  getPriorityClass(priority: Priority): string {
    const priorityClasses: { [key: string]: string } = {
      '1': 'bg-secondary',
      '2': 'bg-info',
      '3': 'bg-danger'
    };
    return priorityClasses[priority] || 'bg-secondary';
  }

  // Get priority label
  getPriorityLabel(priority: Priority): string {
    const priorityClasses: { [key: string]: string } = {
      '1': 'Low Priority',
      '2': 'Medium Priority',
      '3': 'High Priority'
    };
    return priorityClasses[priority];
  }

  isOverdue(task: Task): boolean {
    if (task.status === Status.Done || task.dueDate == null) return false;
    const today = new Date();
    const dueDate = new Date(task.dueDate);
    today.setHours(0, 0, 0, 0);
    dueDate.setHours(0, 0, 0, 0);
    return dueDate < today;
  }
}

