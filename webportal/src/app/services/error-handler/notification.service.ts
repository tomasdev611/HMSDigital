import {Injectable, NgZone} from '@angular/core';
import {ToastService} from '../toaster.service';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  constructor(public toastService: ToastService, private zone: NgZone) {}

  showError(message: string): void {
    this.zone.run(() => {
      this.toastService.showError(message, 8000);
    });
  }
}
