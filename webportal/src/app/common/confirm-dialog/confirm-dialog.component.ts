import {Component, OnInit, Output, EventEmitter} from '@angular/core';
import {ConfirmationService} from 'primeng/api';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.scss'],
})
export class ConfirmDialogComponent implements OnInit {
  @Output() accepted = new EventEmitter<any>();
  @Output() rejected = new EventEmitter<any>();
  constructor(private confirmationService: ConfirmationService) {}

  ngOnInit(): void {}

  showDialog(event) {
    this.confirmationService.confirm({
      message: event.message,
      header: event.header,
      icon: event.icon,
      acceptLabel: event.acceptLabel,
      rejectLabel: event.rejectLabel,
      accept: () => {
        this.accepted.emit(event.data);
      },
      reject: () => {
        this.rejected.emit();
      },
    });
  }
  close() {
    this.confirmationService.close();
  }
}
