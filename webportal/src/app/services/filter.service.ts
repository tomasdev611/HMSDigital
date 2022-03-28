import {Injectable} from '@angular/core';
import {Subject} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FilterService {
  public filterValue = new Subject<object>();
  private lastFilterValue: object;
  constructor() {}

  forceFilterValueUpdate(filterObject: object) {
    if (filterObject !== this.lastFilterValue) {
      this.filterValue.next(filterObject);
      this.lastFilterValue = filterObject;
    }
  }
}
