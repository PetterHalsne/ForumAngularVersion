import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryService } from './categorys.service';

@Component({
  selector: "app-categorys-categoryform",
  templateUrl: "./categoryform.component.html"
})

export class CategoryformComponent {

  categoryForm: FormGroup;
  isEditMode: boolean = false;
  categoryId: number = -1;


  constructor(private _formbuilder: FormBuilder, private _router: Router, private _route: ActivatedRoute, private _categoryService: CategoryService) {
    this.categoryForm = _formbuilder.group({
      categoryName: ['', Validators.required]
    });
  }

  onSubmit(){
    console.log("CategoryCreate form submitted");
    console.log(this.categoryForm);

    const newCategory = this.categoryForm.value;

    if (this.isEditMode) {
      this._categoryService.updateCategory(this.categoryId, newCategory).subscribe(response => {
        if (response.success) {
          console.log(response.message);
          this._router.navigate(['/categorys']);
        }
        else {
          console.log('Category update failed');
        }
      });
    }
    else {
      this._categoryService.createCategory(newCategory).subscribe(response => {
            if (response.success) {
              console.log(response.message);
              this._router.navigate(['/categorys']);
            }
            else {
              console.log('Category creation failed');
            }
          });
    }
    
  }

  backToCategories() {
    this._router.navigate(['/categorys']);
  }

  ngOnInit(): void {
    this._route.params.subscribe(params => {
      if (params['mode'] === 'create') {
        this.isEditMode = false; //create mode
      }
      else if (params['mode'] === 'edit') {
        this.isEditMode = true; //Edit mode
        this.categoryId = +params['id']; //convert the following id to a number
        this.loadCategoryForEdit(this.categoryId);
      }
    });
  }

  loadCategoryForEdit(categoryId: number) {
    this._categoryService.getCategoryById(categoryId).subscribe((category: any) => {
      console.log('Category retrieved: ' + category);
      this.categoryForm.patchValue({
        categoryName: category.CategoryName
      });
    },
      (error: any) => {
        console.error('Error loading category for Edit', error);
      }
    );
  }
}
