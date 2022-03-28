import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {finalize} from 'rxjs/operators';
import {ItemCategory} from 'src/app/models';
import {ItemCategoriesService} from 'src/app/services';

@Component({
  selector: 'app-item-category-details',
  templateUrl: './item-category-details.component.html',
  styleUrls: ['./item-category-details.component.scss'],
})
export class ItemCategoryDetailsComponent implements OnInit {
  categoryId: number;
  categoryDetails: ItemCategory;
  subCategories = [];
  subCategoryHeader = [
    {
      label: 'ID',
      field: 'id',
    },
    {
      label: 'Name',
      field: 'name',
    },
  ];
  loading = false;

  constructor(private categoryService: ItemCategoriesService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    const {paramMap} = this.route.snapshot;
    this.categoryId = Number(paramMap.get('categoryId'));
    this.getCategoryDetails();
  }

  getCategoryDetails() {
    this.loading = true;
    this.categoryService
      .getItemCategoryById(this.categoryId)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((res: any) => {
        this.categoryDetails = res;
        this.subCategories = res.itemSubCategories;
      });
  }
}
