import {Component, OnInit, OnChanges, ViewChild, Input, Output, EventEmitter} from '@angular/core';
import {isAbsolute} from 'path';
import {finalize} from 'rxjs/operators';
import {ItemCategory, PaginationResponse, SieveRequest} from 'src/app/models';
import {ItemCategoriesService} from 'src/app/services';
import {IsPermissionAssigned} from 'src/app/utils';
import {TableVirtualScrollComponent} from 'src/app/common';

@Component({
  selector: 'app-item-categories',
  templateUrl: './item-categories.component.html',
  styleUrls: ['./item-categories.component.scss'],
})
export class ItemCategoriesComponent implements OnInit, OnChanges {
  categoriesReqeust = new SieveRequest();
  categoriesResponse: PaginationResponse;
  loading = false;
  editCategoriesHeader = {
    label: '',
    field: '',
    class: 'sm',
    editBtn: 'Edit Category',
    editBtnIcon: 'pi pi-info-circle',
    editBtnLink: 'item/category/info',
    linkParams: 'id',
  };
  categoriesHeaders = [{label: 'Name', field: 'name'}];
  @ViewChild('categoriesTable ', {static: false})
  categoriesTable: TableVirtualScrollComponent;
  @Input() searchQuery: string;
  @Output() setTotalRecordCount = new EventEmitter<any>();

  constructor(private categoryService: ItemCategoriesService) {}

  ngOnInit(): void {
    if (IsPermissionAssigned('Inventory', 'Update')) {
      this.categoriesHeaders.push(this.editCategoriesHeader);
    }
    this.getCategoriesList();
  }

  ngOnChanges(changes: any) {
    this.getCategoriesList();
  }

  getCategoriesList() {
    this.loading = true;
    (!this.searchQuery
      ? this.categoryService.getItemCategories(this.categoriesReqeust)
      : this.categoryService.searchItemCategories({
          ...this.categoriesReqeust,
          searchQuery: this.searchQuery,
        })
    )
      .pipe(finalize(() => (this.loading = false)))
      .subscribe((res: PaginationResponse) => {
        this.categoriesResponse = res;
        this.setTotalRecordCount.emit({
          totalCount: this.categoriesResponse.totalRecordCount,
        });
      });
  }

  fetchNext({pageNum}) {
    if (!this.categoriesResponse || pageNum > this.categoriesResponse.totalPageCount) {
      return;
    }
    this.categoriesReqeust.page = pageNum;
    this.getCategoriesList();
  }

  hasPermission(entity, permission = 'Read') {
    return IsPermissionAssigned(entity, permission);
  }

  dataTablesReset() {
    if (this.categoriesTable) {
      this.categoriesTable.reset();
    }
    this.categoriesReqeust.page = 1;
  }
}
