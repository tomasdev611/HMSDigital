import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {HttpClient, HttpParams, HttpHeaders} from '@angular/common/http';
import {HttpService} from './http.service';
import {OAuthService} from 'angular-oauth2-oidc';

const API_BASE_URL = environment.reportServerUrl + '/api/';

@Injectable({
  providedIn: 'root',
})
export class ReportApiService {
  constructor(
    private http: HttpClient,
    private httpService: HttpService,
    private oAuthService: OAuthService
  ) {}

  get(url, queryParams?: HttpParams) {
    return this.http.get(API_BASE_URL + url, this.getHeaders(queryParams));
  }

  getCsv(url, queryParams?: HttpParams) {
    return this.http.get(API_BASE_URL + url, this.httpService.getCsvHeaders(queryParams));
  }

  post(url, data) {
    return this.http.post(API_BASE_URL + url, data, this.getHeaders());
  }

  put(url, data, queryParams?: HttpParams) {
    return this.http.put(API_BASE_URL + url, data, this.getHeaders(queryParams));
  }

  patch(url, patchDocument) {
    return this.http.patch(API_BASE_URL + url, patchDocument, this.getHeaders());
  }

  delete(url, data?) {
    return this.http.delete(API_BASE_URL + url, this.getHeaders(null, data));
  }

  getHeaders(queryParams?, data?) {
    return this.httpService.getHeaders(queryParams, data);
  }
}
