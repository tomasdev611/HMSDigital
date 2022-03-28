import {Injectable} from '@angular/core';

@Injectable()
export class MessageConstants {
  constructor() {}
  // toaster messages
  getMessage(type) {
    // type is message switch_case_code
    switch (type) {
      case 403:
        return 'You do not have access to this page/item. Please contact system administrator';
      case 405:
        return 'We have encountered an issue. We will work on fixing it';
      case 400:
        return 'Please review the necessary fields and resubmit';
      case 0:
        return 'The requested service is unavailable';
      default:
        return '';
    }
  }
}
