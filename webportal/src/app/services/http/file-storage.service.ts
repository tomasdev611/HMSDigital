import {Injectable} from '@angular/core';
import {HttpHeaders, HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class FileStorageService {
  constructor(private http: HttpClient) {}

  createHeaders() {
    let headers = new HttpHeaders();
    headers = headers.append('x-ms-blob-type', 'BlockBlob');
    return headers;
  }

  storeFile(url: string, file: File) {
    return this.http.put(url, file, {headers: this.createHeaders()});
  }
}
