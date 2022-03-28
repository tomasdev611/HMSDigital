import {Injectable, NgZone} from '@angular/core';
import {MessageService} from 'primeng/api';
@Injectable({
  providedIn: 'root',
})
export class ToastService {
  constructor(public toasterService: MessageService, private zone: NgZone) {}

  showSuccess(detail: string, life?): void {
    this.showToast('success', 'Success', detail, life);
  }

  showInfo(detail: string, life?): void {
    this.showToast('info', 'Info', detail, life);
  }

  showWarning(detail: string, life?): void {
    this.showToast('warn', 'Warning', detail, life);
  }

  showError(detail: string, life?): void {
    this.showToast('error', 'Error', detail, life);
  }

  showToast(severity: string, summary: string, detail: string, life = 3000) {
    this.zone.run(() => {
      this.toasterService.add({severity, summary, detail, life});
    });
  }
}
