import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {setQueryParams} from '../utils';

@Injectable({
  providedIn: 'root',
})
export class AzureService {
  constructor(private http: HttpClient) {}

  getRouteDirection(fromCoOrdinates: any[], toCoOrdinates: any[]) {
    const subscriptionKey = environment.azure.mapSubscriptionKey;
    const query = `${fromCoOrdinates.join(',')}:${toCoOrdinates.join(',')}`;
    const params = setQueryParams({
      'subscription-key': subscriptionKey,
      'api-version': 1.0,
      query, // value should be in lat,lng:lat,lng format
      travelMode: 'truck',
    });
    return this.http.get(`https://atlas.microsoft.com/route/directions/json?${params}`);
  }
}
