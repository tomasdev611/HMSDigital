import {Component, OnInit, Input, Output, EventEmitter, OnChanges} from '@angular/core';
import {EditableTableColumn, EditableTableInputTypes, DropdownOptions} from 'src/app/models';

@Component({
  selector: 'app-input-mappings',
  templateUrl: './input-mappings.component.html',
  styleUrls: ['./input-mappings.component.scss'],
})
export class InputMappingsComponent implements OnInit, OnChanges {
  @Input() dataTypes: DropdownOptions[];
  @Input() fields: [];
  @Input() mappings: any;
  @Input() mappingList: any;
  @Output() save = new EventEmitter<any>();
  @Output() cancel = new EventEmitter<any>();

  inputColumnSchema: EditableTableColumn[];
  isValid = true;

  private valueSchema = {
    name: '',
    isRequired: false,
    type: '',
    columnOrder: 0,
    key: '',
    title: '',
  };

  constructor() {}

  ngOnChanges() {
    this.mappingList.sort((map1, map2) => {
      if (map1.columnOrder < map2.columnOrder) {
        return -1;
      } else if (map1.columnOrder > map2.columnOrder) {
        return 1;
      }
      return 0;
    });
  }

  ngOnInit(): void {
    this.inputColumnSchema = [
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
        label: 'Type',
        readOnly: false,
        key: 'type',
        type: EditableTableInputTypes.inputDropdown,
        options: this.dataTypes,
      },
      {
        label: 'Required',
        readOnly: false,
        key: 'isRequired',
        type: EditableTableInputTypes.inputSwitch,
        size: 'exsmall',
      },
    ];
  }

  onCancel() {
    this.cancel.emit();
  }

  onSave() {
    this.save.emit({mappingList: this.mappingList, type: 'input'});
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

  addInputField(event) {
    const currentField = event.itemValue;
    const selectedFields = event.value;

    // when select all is clicked the currentField is undefined/null
    if (!currentField && Array.isArray(selectedFields)) {
      selectedFields.forEach((v, index) => {
        if (this.mappings && this.mappings[v.key]) {
          selectedFields[index] = {
            ...this.mappings[v.key],
            key: v.key,
            title: v.title,
            columnOrder: index + 1,
          };
        } else {
          selectedFields[index] = this.getPopulatedSchema(v);
          selectedFields[index].columnOrder = index + 1;
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
