import { Component, OnInit } from '@angular/core';
import { ICategory } from './category';
import { Router } from '@angular/router';
import { CategoryService } from './categorys.service';

@Component({
  selector: 'app-categorys-component',
  templateUrl: './categorys.component.html',
  styleUrls: ['./categorys.component.css']
})

export class CategorysComponent implements OnInit {
  viewTitle: string = 'Categories';
  private _listfilter: string = "";

  categorys: ICategory[] = [];

  constructor(
    private _router: Router,
    private _categoryService: CategoryService) { }

  
  get listFilter(): string {
    return this._listfilter;
  }
  set listFilter(value: string) {
    this._listfilter = value;
    console.log('Listfilter setMethod: ', value);
    this.filteredCategorys = this.performFilter(value);
  }

  deleteCategory(category: ICategory): void {
    const confirmDelete = confirm(`Are you sure you want to delete "${category.CategoryName}"?`);
    if (confirmDelete) {
      this._categoryService.deleteCategory(category.CategoryId).subscribe((response) => {
        if (response.success) {
          console.log(response.message);
          this.filteredCategorys = this.filteredCategorys.filter(c => c !== category);
        }
      },
        (error) => {
          console.error('Error deleting category: ', error);
        }
      );
    }
  }

  getCategorys(): void {
    this._categoryService.getCategorys().subscribe(data => {
      console.log('All', JSON.stringify(data));
      this.categorys = data;
      this.filteredCategorys = this.categorys;
    });
  }

  filteredCategorys: ICategory[] = this.categorys;
  performFilter(filterBy: string): ICategory[] {
    filterBy = filterBy.toLocaleLowerCase();
    return this.categorys.filter((category: ICategory) =>
      category.CategoryName.toLocaleLowerCase().includes(filterBy));
  }

  navigateToCategoryform() {
    this._router.navigate(['/categoryform']);
  }

  ngOnInit(): void {
    console.log('CategoryComponent created');
    this.getCategorys();
  }
}
