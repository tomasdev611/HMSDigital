import {Injectable} from '@angular/core';
import {CoreApiService} from './http/core-api.service';
import {HttpParams} from '@angular/common/http';
import {CustomHttpUrlEncodingCodecService} from './custom-http-url-encoding-codec.service';
import {setQueryParams} from '../utils';

@Injectable({
  providedIn: 'root',
})
export class DispatchService {
  constructor(private http: CoreApiService) {}

  getAllDispatchInstructions(orderHeadersRequest: any) {
    const queryParams = setQueryParams(orderHeadersRequest);
    return this.http.get('dispatch-instructions', queryParams);
  }

  getDispatchInstructionsById(dispatchInstructionId: number) {
    return this.http.get(`dispatch-instructions/${dispatchInstructionId}`);
  }

  dispatchAssign(body) {
    return this.http.post(`dispatch/assign`, body);
  }

  fulfillOrder(body) {
    return this.http.post(`dispatch/fulfill-order`, body);
  }

  getDispatchOrderLocations(body) {
    return this.http.post(`dispatch/order-locations`, body);
  }

  updateDispatch(body) {
    return this.http.put(`dispatch`, body);
  }
}
