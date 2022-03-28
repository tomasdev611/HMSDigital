import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';

@Component({
  selector: 'app-address-list',
  templateUrl: './address-list.component.html',
  styleUrls: ['./address-list.component.scss'],
})
export class AddressListComponent implements OnInit {
  @Input() title = '';
  @Input() address = [];
  @Output() delete = new EventEmitter<any>();

  constructor() {}

  ngOnInit(): void {}

  deleteAddress(index) {
    this.delete.emit(index);
  }
}
