import {Component, Input} from '@angular/core';
import {FormGroup} from '@angular/forms';
import {getDistinctList, getStates} from 'src/app/utils';
import {HideAddressFields} from 'src/app/models';

@Component({
  selector: 'app-address',
  templateUrl: './address.component.html',
  styleUrls: ['./address.component.scss'],
})
export class AddressComponent {
  @Input() address: FormGroup;
  hide = new HideAddressFields();
  @Input() set hiddenFields(fields: [any]) {
    fields.map(field => {
      this.hide[field] = true;
    });
  }

  cities = [];
  states = getStates('');
  search(event, type, list) {
    if (event.query.length >= 2) {
      this[list] = [...getDistinctList(type, event.query)];
    }
  }
  isRequiredFileds(field) {
    if (this.address?.controls[field].validator !== null) {
      return true;
    }
    return false;
  }
}
