import {Component, OnInit, Input, OnChanges, Output, EventEmitter, ViewChild} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {DropdownOptions, ConfirmDialog} from 'src/app/models';

@Component({
  selector: 'app-mappings',
  templateUrl: './mappings.component.html',
  styleUrls: ['./mappings.component.scss'],
})
export class MappingsComponent implements OnInit, OnChanges {
  // Input Mappings props
  @Input() inputMappings: any;

  // output Mapping props
  @Input() outputMappings: any;

  // common states
  @Output() save = new EventEmitter<any>();
  @Output() cancel = new EventEmitter<any>();
  @Input() fields: [];

  inputMappingList;
  outputMappingList;
  mappingView;

  @ViewChild('confirmDialog', {static: false}) confirmDialog;
  cancelData = new ConfirmDialog();
  // common states
  dataTypes: DropdownOptions[] = [
    {label: 'String', value: 'string'},
    {label: 'Numeric', value: 'numeric'},
    {label: 'AlphaNumeric', value: 'alphanumeric'},
    {label: 'Boolean', value: 'boolean'},
    {label: 'Date', value: 'date'},
  ];

  constructor(private route: ActivatedRoute, private router: Router) {
    this.route.queryParams.subscribe(params => {
      this.mappingView = params.view;
    });
  }

  ngOnChanges(): void {
    if (this.inputMappings) {
      this.inputMappingList = this.convertMappingsToList(this.inputMappings);
    }
    if (this.outputMappings) {
      this.outputMappingList = this.convertMappingsToList(this.outputMappings);
    }
  }

  ngOnInit(): void {
    if (this.inputMappings) {
      this.inputMappingList = this.convertMappingsToList(this.inputMappings);
    }
    if (this.outputMappings) {
      this.outputMappingList = this.convertMappingsToList(this.outputMappings);
    }
  }

  convertMappingsToList(mappings) {
    const mappingList = Object.keys(mappings).map((key: any) => {
      const field: any = this.fields.find((f: any) => {
        return f.value.key === key;
      });
      return {...mappings[key], key, title: field.value.title};
    });
    return mappingList;
  }

  onSave({mappingList, type}) {
    const mappingsToSave = mappingList.reduce((mappings: any, field: any) => {
      mappings[field.key] = {...field};
      delete mappings[field.key].key;
      delete mappings[field.key].title;
      return mappings;
    }, {});
    this.save.emit({mappings: mappingsToSave, type});
  }

  onCancel() {
    this.cancelData.message = `Do you want to cancel? (All unsaved information will be lost)`;
    this.cancelData.header = 'Cancel Confirmation';
    this.confirmDialog.showDialog(this.cancelData);
  }

  cancelConfirmed() {
    this.cancel.emit();
  }

  onTabChange(event) {
    if (event.index === 0) {
      this.mappingView = 'input';
    } else if (event.index === 1) {
      this.mappingView = 'output';
    }
    this.router.navigate([], {queryParams: {view: this.mappingView}});
  }
}
