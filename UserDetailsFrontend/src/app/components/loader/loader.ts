import { Component, OnInit } from '@angular/core';
import { LoaderService } from '../../core/services/loaderService/loader-service';

@Component({
  selector: 'app-loader',
  standalone: false,
  templateUrl: './loader.html',
  styleUrl: './loader.css',
})
export class Loader implements OnInit{  
  showLoader = false;

  constructor(public loaderService: LoaderService) {  }

  ngOnInit(): void {
    this.loaderService.showLoader$.subscribe((status)=>{
      this.showLoader = status;
    })
  }
}
