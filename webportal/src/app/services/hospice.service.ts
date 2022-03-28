import {Injectable} from '@angular/core';
import {CoreApiService} from './http/core-api.service';
import {setQueryParams} from '../utils';

@Injectable({
  providedIn: 'root',
})
export class HospiceService {
  constructor(private http: CoreApiService) {}

  getHospiceMemberInputMapping(hospiceId: number, queryParams?: any) {
    const query = setQueryParams(queryParams);
    return this.http.get(`hospices/${hospiceId}/input-mappings`, query);
  }

  searchHospices(queryParams?: any) {
    const query = setQueryParams(queryParams);
    return this.http.get(`hospices/search`, query);
  }

  getHospiceMemberOutputMapping(hospiceId: number, queryParams?: any) {
    const query = setQueryParams(queryParams);
    return this.http.get(`hospices/${hospiceId}/output-mappings`, query);
  }

  updateHospiceMemberInputMapping(hospiceId, mappings) {
    const queryParams = {mappedItemType: 'hospiceMember'};
    return this.http.put(
      `hospices/${hospiceId}/input-mappings`,
      mappings,
      setQueryParams(queryParams)
    );
  }

  updateHospiceMemberOutputMapping(hospiceId, mappings) {
    const queryParams = {mappedItemType: 'hospiceMember'};
    return this.http.put(
      `hospices/${hospiceId}/output-mappings`,
      mappings,
      setQueryParams(queryParams)
    );
  }

  getAllhospices(hospicesRequest?: any) {
    const queryParams = setQueryParams(hospicesRequest);
    return this.http.get('hospices', queryParams);
  }

  getHospiceById(hospiceId: number) {
    return this.http.get(`hospices/${hospiceId}`);
  }

  createHospice(hospice) {
    return this.http.post(`hospices`, hospice);
  }

  updateHospice(hospiceId: number, hospicePatch) {
    return this.http.patch(`hospices/${hospiceId}`, hospicePatch);
  }

  getHospiceRoles(hospiceId: number) {
    return this.http.get(`hospices/${hospiceId}/roles`);
  }

  getHospiceZabSubscriptions(hospiceId: number, queryParams?: any) {
    const query = setQueryParams(queryParams);
    return this.http.get(`hospices/${hospiceId}/subscriptions`, query);
  }

  getHospiceHMS2Contracts(hospiceId: number, queryParams?: any) {
    const query = setQueryParams(queryParams);
    return this.http.get(`hospices/${hospiceId}/contracts`, query);
  }

  getHospiceZabSubscriptionItems(subscriptionId: number, queryParams?: any) {
    const query = setQueryParams(queryParams);
    return this.http.get(`hospices/subscriptions/${subscriptionId}/subscription-items`, query);
  }

  getHospiceHMS2ContractItems(contractId: number, queryParams?: any) {
    const query = setQueryParams(queryParams);
    return this.http.get(`hospices/contracts/${contractId}/contract-items`, query);
  }

  getHospiceZabSubscriptionContracts(queryParams?: any) {
    const query = setQueryParams(queryParams);
    return this.http.get(`hospices/contract-records`, query);
  }

  updateCreditHold(hospiceId: number, body: any) {
    return this.http.post(`hospices/${hospiceId}/credit-hold`, body);
  }

  getCreditHoldHistory(hospiceId: number, queryParam?: any) {
    const query = setQueryParams({
      ...queryParam,
    });
    return this.http.get(`hospices/${hospiceId}/credit-hold/history`, query);
  }
}
