import {Injectable} from '@angular/core';
import {map} from 'rxjs/operators';
import {EnumNames} from '../enums';
import {CoreApiService} from './http/core-api.service';

@Injectable({
  providedIn: 'root',
})
export class EnumService {
  constructor(private http: CoreApiService) {}

  getEnumerations() {
    return this.http.get('enumerations').pipe(
      map((enums: any) => {
        enums[EnumNames.OrderTypes] = enums[EnumNames.OrderTypes]?.filter(
          e => e.name !== 'Respite'
        );
        return enums;
      })
    );
  }
}
