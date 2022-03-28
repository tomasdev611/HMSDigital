import {Injectable} from '@angular/core';
import {setQueryParams} from '../utils';
import {CoreApiService} from './http/core-api.service';

@Injectable({
  providedIn: 'root',
})
export class ProductCatalogService {
  constructor(private http: CoreApiService) {}

  getProducts(hospiceId: number, locationId: number, queryParams?: any) {
    const query = setQueryParams(queryParams);
    return this.http.get(
      `hospices/${hospiceId}/hospice-locations/${locationId}/product-catalog`,
      query
    );
  }
}
