import {Component, OnInit, Input, EventEmitter, Output, OnChanges} from '@angular/core';
import {EditableTableColumn, EditableTableInputTypes, DropdownOptions} from 'src/app/models';

@Component({
  selector: 'app-output-mappings',
  templateUrl: './output-mappings.component.html',
  styleUrls: ['./output-mappings.component.scss'],
})
export class OutputMappingsComponent implements OnInit, OnChanges {
  @Input() dataTypes: DropdownOptions[];
  @Input() fields: [];
  @Input() mappings: any;
  @Input() mappingList: any;
  @Output() save = new EventEmitter<any>();
  @Output() cancel = new EventEmitter<any>();

  isValid = true;
  outputColumnSchema: EditableTableColumn[];

  private valueSchema = {
    name: '',
    type: '',
    columnOrder: 0,
    showInDisplay: false,
    includeInExport: false,
    isFilterable: false,
    isSortable: false,
    key: '',
    title: '',
  };

  constructor() {}

  ngOnChanges() {
    this.sortMappingList();
  }

  ngOnInit(): void {
    this.outputColumnSchema = [
      {
        label: 'Field',
        readOnly: true,
        key: 'title',
      },
      {
        label: 'CSV Column',
        key: 'name',
        readOnly: false,
        type: EditableTableInputTypes.inputText,
      },
      {
        label: 'Column Order',
        readOnly: true,
        key: 'columnOrder',
        type: EditableTableInputTypes.inputNumeric,
      },
      {
        label: 'Type',
        readOnly: false,
        key: 'type',
        type: EditableTableInputTypes.inputDropdown,
        options: this.dataTypes,
      },
      {
        label: 'Show In Display',
        readOnly: false,
        key: 'showInDisplay',
        type: EditableTableInputTypes.inputSwitch,
        size: 'exsmall',
      },
      {
        label: 'Include In Export',
        readOnly: false,
        key: 'includeInExport',
        type: EditableTableInputTypes.inputSwitch,
        size: 'exsmall',
      },
      {
        label: 'Sortable',
        readOnly: false,
        key: 'isSortable',
        type: EditableTableInputTypes.inputSwitch,
        size: 'exsmall',
      },
      {
        label: 'Filterable',
        readOnly: false,
        key: 'isFilterable',
        type: EditableTableInputTypes.inputSwitch,
        size: 'exsmall',
      },
    ];
  }

  sortMappingList() {
    this.mappingList.sort((map1, map2) => {
      if (map1.columnOrder < map2.columnOrder) {
        return -1;
      } else if (map1.columnOrder > map2.columnOrder) {
        return 1;
      }
      return 0;
    });
  }

  onCancel() {
    this.cancel.emit();
  }

  onSave() {
    this.save.emit({mappingList: this.mappingList, type: 'output'});
  }

  private getPopulatedSchema(values) {
    return {
      ...this.valueSchema,
      name: values.title,
      type: values.type,
      key: values.key,
      title: values.title,
    };
  }

  addOutputField(event) {
    const currentField = event.itemValue;
    const selectedFields = event.value;

    // when select all is clicked the currentField is undefined/null
    if (!currentField && Array.isArray(selectedFields)) {
      selectedFields.forEach((v, index) => {
        if (this.mappings && this.mappings[v.key]) {
          selectedFields[index] = {...this.mappings[v.key], key: v.key, title: v.title};
        } else {
          selectedFields[index] = this.getPopulatedSchema(v);
        }
      });
    } else {
      const index = selectedFields.findIndex(v => v.key === currentField.key);
      if (index >= 0) {
        if (this.mappings && this.mappings[currentField.key]) {
          selectedFields[index] = {
            ...this.mappings[currentField.key],
            key: currentField.key,
            title: currentField.title,
          };
        } else {
          selectedFields[index] = this.getPopulatedSchema(currentField);
        }
      }
    }
    this.changeColumnOrder(null);
  }

  changeColumnOrder(event) {
    this.mappingList.forEach((_, index: number) => {
      this.mappingList[index].columnOrder = index + 1;
    });
  }

  validateData({key, index, data}) {
    this.isValid = data.name?.length;
  }
}
