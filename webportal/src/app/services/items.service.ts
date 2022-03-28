import {Injectable} from '@angular/core';
import {CoreApiService} from './http/core-api.service';
import {Item, ItemImageRequest} from '../models';
import {setQueryParams} from '../utils';

@Injectable({
  providedIn: 'root',
})
export class ItemsService {
  constructor(private http: CoreApiService) {}

  getItemsList(queryParams?: any) {
    const query = setQueryParams(queryParams);
    return this.http.get(`items`, query);
  }

  getItemDetailsById(productId) {
    return this.http.get(`items/${productId}`);
  }

  createItem(item: Item) {
    return this.http.post(`items`, item);
  }

  updateItem(productId: number, productPatch) {
    return this.http.patch(`items/${productId}`, productPatch);
  }

  searchItems(searchQuery?: any) {
    const query = setQueryParams(searchQuery);
    return this.http.get(`items/search`, query);
  }

  deleteItem(productId: number) {
    return this.http.delete(`items/${productId}`);
  }

  getImageUploadUrl(itemId: number, body: ItemImageRequest) {
    return this.http.post(`items/${itemId}/images`, body);
  }

  confirmImageUpload(productId: number, imageId: number) {
    return this.http.post(`items/${productId}/images/${imageId}/uploaded`, null);
  }

  getItemImages(productId: number) {
    return this.http.get(`items/${productId}/images`);
  }

  transferProduct(productId, transferReq) {
    return this.http.post(`items/${productId}/transfer`, transferReq);
  }

  getAllItemImages(queryParams) {
    const query = setQueryParams(queryParams);
    return this.http.get(`items/images`, query);
  }

  updateEquipmentSettingConfig(itemId: number, equipmentSettings: any[]) {
    return this.http.put(`items/${itemId}/equipment-config`, equipmentSettings);
  }

  getEquipmentSettingTypes(queryParams) {
    const query = setQueryParams(queryParams);
    return this.http.get(`items/equipment-setting-types`, query);
  }

  updateAddOnGroups(itemId: number, addOnGroups) {
    return this.http.put(`items/${itemId}/addons-config`, addOnGroups);
  }
}
