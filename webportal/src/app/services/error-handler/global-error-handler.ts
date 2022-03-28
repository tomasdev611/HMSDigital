import {ErrorHandler, Injectable, Injector} from '@angular/core';
import {HttpErrorResponse} from '@angular/common/http';
import {MonitoringService} from './log.service';
import {NotificationService} from './notification.service';
import {MessageConstants} from '../../constants';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private injector: Injector, private messageConstants: MessageConstants) {}

  handleError(error: Error | HttpErrorResponse) {
    const logger = this.injector.get(MonitoringService);
    const notifier = this.injector.get(NotificationService);

    let message;
    if (error instanceof HttpErrorResponse) {
      // Server Error
      message =
        error.error && typeof error.error !== 'object'
          ? error.error
          : this.messageConstants.getMessage(error.status);
      notifier.showError(message ? message : error.message);
    }

    // log errors
    logger.logException(error);
    console.error(error);
  }
}
