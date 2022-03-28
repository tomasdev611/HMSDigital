import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from 'src/environments/environment';
import {HttpService} from './http.service';

const API_BASE_URL = environment.routeOptimizerURL + '/api/';

@Injectable({
  providedIn: 'root',
})
export class RouteOptimizationApiService {
  constructor(private http: HttpClient, private httpService: HttpService) {}

  post(url, data) {
    return this.http.post(API_BASE_URL + url, data, this.getHeaders());
  }

  getHeaders(queryParams?) {
    return this.httpService.getHeaders(queryParams);
  }
}
