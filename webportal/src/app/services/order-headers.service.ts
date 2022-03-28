import {Injectable} from '@angular/core';
import {CoreApiService} from './http/core-api.service';
import {setQueryParams} from '../utils';

@Injectable({
  providedIn: 'root',
})
export class OrderHeadersService {
  constructor(private http: CoreApiService) {}

  getAllOrderHeaders(orderHeadersRequest?: any) {
    const queryParams = setQueryParams(orderHeadersRequest);
    return this.http.get('order-headers', queryParams);
  }

  getOrderHeaderById(orderHeaderId: number, includeFulfillmentDetails = false) {
    const query = `?includeFulfillmentDetails=${includeFulfillmentDetails}`;
    return this.http.get(`order-headers/${orderHeaderId}${query}`);
  }

  getOrderFulfillment(orderHeaderId: number, orderFulfillmentRequest?: any) {
    const queryParams = setQueryParams(orderFulfillmentRequest);
    return this.http.get(`order-headers/${orderHeaderId}/fulfillment`, queryParams);
  }

  createOrderHeader(order: any) {
    return this.http.post(`order-headers`, order);
  }

  updateOrderHeader(order: any) {
    return this.http.put(`order-headers`, order);
  }
}
