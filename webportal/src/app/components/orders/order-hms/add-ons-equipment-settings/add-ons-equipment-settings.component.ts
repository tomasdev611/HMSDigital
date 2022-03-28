import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'app-add-ons-equipment-settings',
  templateUrl: './add-ons-equipment-settings.component.html',
  styleUrls: ['./add-ons-equipment-settings.component.scss'],
})
export class AddOnsEquipmentSettingsComponent implements OnInit {
  showModal = false;
  product: any;
  @Input() set productContext(product: any) {
    if (product?.equipmentSettings) {
      this.product = product;
    }
  }
  @Output() updateProductHandler = new EventEmitter<any>();
  constructor() {}

  ngOnInit(): void {}
  show() {
    this.showModal = true;
  }
  close() {
    this.showModal = false;
  }
  submit() {
    if (!this.checkValidity()) {
      this.updateProductHandler.emit({product: this.product});
      this.close();
    }
  }
  checkValidity() {
    let content = ``;
    const header = `<span>Required fields are not complete</span><ul>`;
    if (this.product?.equipmentSettings) {
      const value = this.product.equipmentSettings.filter(x => !x.value);
      if (value.length) {
        content += header;
        value.map(setting => {
          content += `<li>${setting.name}</li>`;
        });
      }
    }
    if (this.product?.item?.addOnGroups) {
      const value = this.product?.item?.addOnGroups.filter(x => !x.selected?.length);
      if (value.length) {
        content = content === '' ? header : content;
        value.map(group => {
          content += `<li>${group.name}</li>`;
        });
      }
    }
    return content;
  }
  selectValue(event, addOn, group) {
    if (addOn.itemId === 0 || !group.allowMultipleSelection) {
      const selectedValue = group.selected[group.selected.length - 1];
      group.selected.length = 0;
      if (selectedValue) {
        group.selected.push(selectedValue);
      }
    } else {
      group.selected = group.selected.filter(x => x.itemId !== 0);
    }
  }
}
