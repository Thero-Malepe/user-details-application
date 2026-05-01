import { NumberSymbol } from '@angular/common';
import { Component, EventEmitter, Input, input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-pagination',
  standalone: false,
  templateUrl: './pagination.html',
  styleUrl: './pagination.css',
})
export class Pagination implements OnChanges{

  @Input('itemsPerPage') itemsPerPage = 0;
  @Input('numberOfItems') numberOfItems = 1;
  @Input('currentPage') currentPage = 0;

  @Output() pageChange = new EventEmitter<any>();
  totalPages = 0;
  pages: any = [];
  
  ngOnChanges(): void {
    if(this.itemsPerPage > 0)
    {
      this.totalPages = Math.ceil(this.numberOfItems/this.itemsPerPage);
      this.pages = this.generatePages();
    }
  }

  changePage(page: any)
  {
    if(typeof page === "number")
    {
      this.currentPage = page;
      this.pageChange.emit(this.currentPage);
    }    
  }

  generatePages(): (number | string)[] 
  {
    const pages: (number | string)[] = [];
    const total = this.totalPages;
    const current = this.currentPage;

    if (total <= 7) {
      // Show all pages if small
      return Array.from({ length: total }, (_, i) => i + 1);
    }

    // Always show first page
    pages.push(1);

    // Left ellipsis
    if (current > 4) {
      pages.push('...');
    }

    // Middle pages (current -1, current, current +1)
    const start = Math.max(2, current - 1);
    const end = Math.min(total - 1, current + 1);

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }

    // Right ellipsis
    if (current < total - 2) {
      pages.push('....');
    }

    // Always show last page
    pages.push(total);

    return pages;    
  }
}
