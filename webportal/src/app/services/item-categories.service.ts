import {Injectable} from '@angular/core';
import {CoreApiService} from './http/core-api.service';
import {ItemCategory} from '../models';
import {setQueryParams} from '../utils';

@Injectable({
  providedIn: 'root',
})
export class ItemCategoriesService {
  constructor(private http: CoreApiService) {}

  getItemCategories(queryParams?: any) {
    const query = setQueryParams(queryParams);
    return this.http.get(`item-categories`, query);
  }

  searchItemCategories(queryParams?: any) {
    const query = setQueryParams(queryParams);
    return this.http.get(`item-categories/search`, query);
  }

  getItemCategoryById(categoryId: number) {
    return this.http.get(`item-categories/${categoryId}`);
  }

  createItemCategory(itemCategory: ItemCategory) {
    return this.http.post(`item-categories`, itemCategory);
  }

  updateItemCategory(categoryId: number, categoryPatch) {
    return this.http.patch(`item-categories/${categoryId}`, categoryPatch);
  }

  deleteItemCategory(categoryid: number) {
    return this.http.delete(`item-categories/${categoryid}`);
  }
}
