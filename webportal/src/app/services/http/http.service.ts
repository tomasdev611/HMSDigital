import {Injectable} from '@angular/core';
import {OAuthService} from 'angular-oauth2-oidc';
import {HttpHeaders, HttpParams} from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  constructor(private oAuthService: OAuthService) {}

  createHeader() {
    const token = this.oAuthService.getAccessToken();
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }

  getHeaders(queryParams?: HttpParams, data?) {
    return {
      headers: this.createHeader(),
      params: queryParams,
      body: data,
    };
  }

  getCsvHeaders(queryParams?: HttpParams): {
    headers: HttpHeaders;
    params: HttpParams;
    responseType: 'text';
  } {
    return {
      headers: this.createHeader(),
      params: queryParams,
      responseType: 'text',
    };
  }
}
