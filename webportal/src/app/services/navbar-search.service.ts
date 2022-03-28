import {Injectable} from '@angular/core';
import {Subject} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class NavbarSearchService {
  public search = new Subject<string>();
  private lastValue = '';
  public image = new Subject<string>();
  private lastImage = null;
  public userInfo = new Subject<object>();
  private lastUserInfo = null;
  constructor() {}

  searchTextChanged(text: string) {
    if (text !== this.lastValue) {
      this.search.next(text);
      this.lastValue = text;
    }
  }

  userImageChanged(image: string) {
    if (image !== this.lastImage) {
      this.image.next(image ?? '');
      this.lastImage = image ?? '';
    }
  }

  userInfoUpdated(userInfo: object) {
    if (userInfo !== this.lastUserInfo) {
      this.userInfo.next(userInfo);
      this.lastUserInfo = userInfo;
    }
  }
}
