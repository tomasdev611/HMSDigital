import {CoreApiService} from './http/core-api.service';
import {Injectable} from '@angular/core';
import {setQueryParams} from '../utils';
import {InventoryItem} from '../models/model.inventory-item';

@Injectable({
  providedIn: 'root',
})
export class InventoryService {
  constructor(private http: CoreApiService) {}

  getInvetoryList(queryParams?: any) {
    const query = setQueryParams(queryParams);
    return this.http.get(`inventory`, query);
  }

  getInventoryItemById(itemId: number) {
    return this.http.get(`inventory/${itemId}`);
  }

  createInventoryItem(inventoryItem: InventoryItem) {
    return this.http.post(`inventory`, inventoryItem);
  }

  updateInventoryItem(inventoryItemId: number, patch) {
    return this.http.patch(`inventory/${inventoryItemId}`, patch);
  }

  deleteInventoryItem(inventoryItemId: number) {
    return this.http.delete(`inventory/${inventoryItemId}`);
  }

  getInventoryItemLogs(inventoryItemId: number) {
    return this.http.get(`inventory/${inventoryItemId}/history`);
  }
  getAuditLogDetailById(inventoryItemId: number, auditId: number) {
    return this.http.get(`inventory/${inventoryItemId}/history/${auditId}`);
  }

  getPatientInventoryByUuid(uuId: string, query?: any) {
    const queryParams = setQueryParams(query);
    return this.http.get(`inventory/patient/${uuId}`, queryParams);
  }

  getPatientInventoryByUuidSearch(uuId: string, query?: any) {
    const queryParams = setQueryParams(query);
    return this.http.get(`inventory/patient/${uuId}/search`, queryParams);
  }

  searchInventory(inventoryRequset?: any) {
    const query = setQueryParams(inventoryRequset);
    return this.http.get(`inventory/search`, query);
  }
}
