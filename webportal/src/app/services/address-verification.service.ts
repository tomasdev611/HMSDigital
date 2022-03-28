import {Injectable} from '@angular/core';
import {setQueryParams} from 'src/app/utils/http-params.utils';
import {CoreApiService} from './http/core-api.service';

@Injectable({
  providedIn: 'root',
})
export class AddressVerificationService {
  constructor(private http: CoreApiService) {}

  verifyAddress(addressRequest: any) {
    return this.http.post('verifyaddress', addressRequest);
  }

  verifiedAddressSuggestions(addressRequest) {
    addressRequest.maxrecords = 5;
    return this.http.post('verifyaddress/suggestions/', addressRequest);
  }
}
