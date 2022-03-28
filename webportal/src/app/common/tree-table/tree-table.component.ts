import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {getObjectValueByKey} from 'src/app/utils';

@Component({
  selector: 'app-tree-table',
  templateUrl: './tree-table.component.html',
  styleUrls: ['./tree-table.component.scss'],
})
export class TreeTableComponent implements OnInit {
  @Input() nodes;
  @Input() selection = false;
  @Input() pageResponse: PaginationResponse;
  @Input() headers;
  @Input() paginate = true;
  @Input() parentSelectOnly;
  @Output() next = new EventEmitter<any>();
  @Output() prev = new EventEmitter<any>();
  @Output() rowSelected = new EventEmitter<any>();

  selectedRows = [];

  constructor() {}

  ngOnInit(): void {}

  emitNext() {
    this.next.emit();
  }

  emitPrev() {
    this.prev.emit();
  }

  getValueByField(obj, fieldName) {
    return getObjectValueByKey(obj, fieldName) || '';
  }

  rowSelectable(node) {
    return this.parentSelectOnly ? this.selection && !node.leaf : this.selection;
  }

  onRowSelection(event, checked) {
    this.rowSelected.emit({selectedRows: this.selectedRows, currentRow: event.node, checked});
  }
}
