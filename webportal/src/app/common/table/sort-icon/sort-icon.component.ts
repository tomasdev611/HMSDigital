import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';

@Component({
  selector: 'app-sort-icon',
  templateUrl: './sort-icon.component.html',
  styleUrls: ['./sort-icon.component.scss'],
})
export class SortIconComponent implements OnInit {
  @Input() field;
  @Output() sort = new EventEmitter<any>();
  @Input() orderIndex;
  orders: number[] = [0, 1, -1];
  iconClasses: string[] = ['pi pi-sort-alt', 'pi pi-sort-amount-down-alt', 'pi pi-sort-amount-up'];

  constructor() {}

  ngOnInit(): void {}

  sortField() {
    this.orderIndex = (this.orderIndex + 1) % 3;
    this.sort.emit({
      field: this.field,
      order: this.orders[this.orderIndex],
      orderIndex: this.orderIndex,
    });
  }

  getIconClass() {
    return this.iconClasses[this.orderIndex];
  }
}
