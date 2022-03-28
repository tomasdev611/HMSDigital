import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {environment} from 'src/environments/environment';
import {HttpService} from './http.service';

const API_BASE_URL = environment.patientApiServerUrl + '/api/';

@Injectable({
  providedIn: 'root',
})
export class PatientApiService {
  constructor(private http: HttpClient, private httpService: HttpService) {}

  get(url, queryParams?: HttpParams) {
    return this.http.get(API_BASE_URL + url, this.getHeaders(queryParams));
  }

  getCsv(url, queryParams?: HttpParams) {
    return this.http.get(API_BASE_URL + url, this.httpService.getCsvHeaders(queryParams));
  }

  post(url, data) {
    return this.http.post(API_BASE_URL + url, data, this.getHeaders());
  }

  put(url, data) {
    return this.http.put(API_BASE_URL + url, data, this.getHeaders());
  }

  patch(url, patchDocument, queryParams?) {
    return this.http.patch(API_BASE_URL + url, patchDocument, this.getHeaders(queryParams));
  }

  delete(url) {
    return this.http.delete(API_BASE_URL + url, this.getHeaders());
  }

  getHeaders(queryParams?) {
    return this.httpService.getHeaders(queryParams);
  }
}
