import {Injectable} from '@angular/core';
import {HttpUrlEncodingCodec} from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class CustomHttpUrlEncodingCodecService extends HttpUrlEncodingCodec {
  constructor() {
    super();
  }
  encodeValue(k: string): string {
    return encodeURIComponent(k);
  }
}
