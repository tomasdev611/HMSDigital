import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss'],
})
export class ModalComponent implements OnInit {
  @Input() dialogStyleClass: string;
  @Input() header = '';
  @Input() visible = false;
  @Input() footer = false;
  @Input() footerBtnDisabled = false;
  @Input() footerBtnIcon = '';
  @Input() footerBtnLabel = '';
  @Input() contentStyle = {};
  @Output() footerBtnAction = new EventEmitter<any>();

  @Output() hide = new EventEmitter<any>();
  constructor() {}

  ngOnInit(): void {}

  hideDialog() {
    this.hide.emit();
  }

  footerBtnHandler() {
    this.footerBtnAction.emit();
  }
}
