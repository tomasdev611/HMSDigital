import {Injectable} from '@angular/core';
import {RouteOptimizationApiService} from './http/route-optimization-api.service';

@Injectable({
  providedIn: 'root',
})
export class RouteOptimizationService {
  constructor(private http: RouteOptimizationApiService) {}

  getOptimizedRoutes(routeRequests: any) {
    return this.http.post('route-optimizer', routeRequests);
  }
}
