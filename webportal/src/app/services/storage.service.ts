import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class StorageService {
  constructor() {}

  get(key, type = 'local') {
    return type === 'local' ? localStorage.getItem(key) : sessionStorage.getItem(key);
  }

  set(key, value: string, type = 'local') {
    type === 'local' ? localStorage.setItem(key, value) : sessionStorage.setItem(key, value);
  }

  remove(key, type = 'local') {
    type === 'local' ? localStorage.removeItem(key) : sessionStorage.removeItem(key);
  }
}
