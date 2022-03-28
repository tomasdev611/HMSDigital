import {HttpParams} from '@angular/common/http';
import {CustomHttpUrlEncodingCodecService} from '../services/custom-http-url-encoding-codec.service';

export function setQueryParams(queryRequest?): HttpParams {
  let queryParams: HttpParams = new HttpParams({
    encoder: new CustomHttpUrlEncodingCodecService(),
  });
  if (queryRequest) {
    const keys = Object.keys(queryRequest);
    keys.forEach(key => {
      if (queryRequest[key]) {
        queryParams = queryParams.set(key, queryRequest[key]);
      }
    });
  }
  return queryParams;
}
