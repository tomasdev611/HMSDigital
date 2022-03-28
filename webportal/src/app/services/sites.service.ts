import {Injectable} from '@angular/core';
import {CoreApiService} from './http/core-api.service';
import {filterByFields, setQueryParams, SortByField} from '../utils';
import {makeStateKey, TransferState} from '@angular/platform-browser';
import {map, mergeMap, tap} from 'rxjs/operators';
import {of} from 'rxjs';
import {PaginationResponse} from '../models';
import {deepClone} from 'fast-json-patch/module/core';

@Injectable({
  providedIn: 'root',
})
export class SitesService {
  constructor(private http: CoreApiService, private ngState: TransferState) {}

  getAllSites() {
    return this.searchSites();
  }

  getSiteById(siteId: number) {
    return this.searchSites().pipe(
      mergeMap(cachedResponse => {
        return of(this.findSite(cachedResponse, siteId));
      })
    );
  }

  findSite(response: any, siteId: number) {
    return response.records.find(site => site.id === siteId);
  }

  getVehiclesByLocationId(locationId: number) {
    return this.http.get(`vehicles/location/${locationId}`);
  }

  createSite(site) {
    return this.http.post(`sites`, site);
  }

  updateSite(siteId: number, sitePatch) {
    return this.http.patch(`sites/${siteId}`, sitePatch);
  }

  getSiteRoles(siteId: number) {
    return this.http.get(`sites/${siteId}/roles`);
  }
  deleteSite(siteId: number) {
    return this.http.delete(`sites/${siteId}`);
  }

  searchSites(sitesRequest?, fields?) {
    const sitesKey = makeStateKey('sites');
    const cachedResponse = this.ngState.get(sitesKey, null);
    if (!cachedResponse) {
      const result = this.http.get('sites').pipe(
        tap((res: PaginationResponse) => {
          this.ngState.set(sitesKey, res);
        }),
        map((res: PaginationResponse) =>
          sitesRequest ? this.cachedSitesFilterAndSorting(res, sitesRequest, fields) : res
        )
      );
      return result;
    }
    if (sitesRequest) {
      return of(this.cachedSitesFilterAndSorting(cachedResponse, sitesRequest, fields));
    } else {
      return of(cachedResponse);
    }
  }

  cachedSitesFilterAndSorting(response, sitesRequest, fields?) {
    const res = deepClone(response);
    res.records = filterByFields(res.records, fields ? fields : ['name'], sitesRequest.searchQuery);
    res.pageSize = res.records.length;
    res.totalRecordCount = res.records.length;
    if (sitesRequest.sorts) {
      const sortFields = sitesRequest.sorts.split(',');
      res.records = SortByField(res.records, sortFields);
    }
    return res;
  }

  getSiteServiceAreas(siteId: number, params?: any) {
    const queryParams = setQueryParams(params);
    return this.http.get(`sites/${siteId}/service-areas`, queryParams);
  }

  addSiteServiceAreas(siteId: number, zipCodes) {
    return this.http.post(`sites/${siteId}/service-areas`, zipCodes);
  }

  deleteSiteServiceAreas(siteId: number, zipCodes) {
    return this.http.delete(`sites/${siteId}/service-areas`, zipCodes);
  }
}
