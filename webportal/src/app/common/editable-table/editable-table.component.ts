import {Component, OnInit, Input, OnChanges, Output, EventEmitter} from '@angular/core';
import {EditableTableColumn, EditableTableInputTypes} from 'src/app/models';

@Component({
  selector: 'app-editable-table',
  templateUrl: './editable-table.component.html',
  styleUrls: ['./editable-table.component.scss'],
})
export class EditableTableComponent implements OnInit {
  inputText = EditableTableInputTypes.inputText;
  inputNumeric = EditableTableInputTypes.inputNumeric;
  inputDropdown = EditableTableInputTypes.inputDropdown;
  inputSwitch = EditableTableInputTypes.inputSwitch;

  colGroupClasses = {
    exsmall: 'ex-small',
  };

  @Input() columns: EditableTableColumn[];
  @Input() data: [];
  @Input() reorderable: boolean;
  @Output() rowReorder = new EventEmitter<any>();
  @Output() rowChanged = new EventEmitter<any>();

  constructor() {}

  ngOnInit(): void {}

  getColGroupClass(cl) {
    return this.colGroupClasses[cl];
  }

  rowOrderChanged(event) {
    this.rowReorder.emit(event);
  }

  rowDataChanged(key, index) {
    this.rowChanged.emit({key, index, data: this.data[index]});
  }
}
