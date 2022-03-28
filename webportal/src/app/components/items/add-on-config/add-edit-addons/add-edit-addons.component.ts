import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {SieveOperators} from 'src/app/enums';
import {SieveRequest} from 'src/app/models';
import {ItemsService, ToastService} from 'src/app/services';
import {buildFilterString, deepCloneObject} from 'src/app/utils';

@Component({
  selector: 'app-add-edit-addons',
  templateUrl: './add-edit-addons.component.html',
  styleUrls: ['./add-edit-addons.component.scss'],
})
export class AddEditAddonsComponent implements OnInit {
  selectedProduct = null;
  searchedItemsList = [];
  loading = false;
  addonGroupForm: FormGroup;
  addOnGroupDetails: any;
  editMode = false;

  @Input() showAddOnConfigDialog = false;
  @Input() set addOnGroup(group) {
    this.addOnGroupDetails = group;
    if (this.addOnGroupDetails) {
      this.editMode = true;
    }
    this.patchForm();
  }

  @Output() closeConfigDialog = new EventEmitter<any>();
  @Output() addOrUpdateAddonGroup = new EventEmitter<any>();

  constructor(private itemService: ItemsService, private fb: FormBuilder) {
    this.createAddonForm();
  }

  ngOnInit(): void {}

  createAddonForm() {
    this.addonGroupForm = this.fb.group({
      id: new FormControl(null),
      name: new FormControl(null, Validators.required),
      allowMultipleSelection: new FormControl(false, Validators.required),
      products: new FormControl([], Validators.required),
    });
  }

  patchForm() {
    if (this.addOnGroupDetails) {
      this.addonGroupForm.patchValue({
        id: this.addOnGroupDetails.id,
        name: this.addOnGroupDetails.name,
        allowMultipleSelection: this.addOnGroupDetails.allowMultipleSelection,
        products: [...this.addOnGroupDetails.addOnGroupProducts],
      });
    }
  }

  closeAddonConfigDialog() {
    this.closeConfigDialog.emit();
  }

  searchItems({query}) {
    const itemSearchRequest = new SieveRequest();
    if (query) {
      const filter = [
        {
          field: 'name',
          operator: SieveOperators.CI_Contains,
          value: [query],
        },
      ];
      itemSearchRequest.filters = buildFilterString(filter);
    }
    this.itemService.getItemsList(itemSearchRequest).subscribe((items: any) => {
      this.searchedItemsList = items.records;
    });
  }

  addItemToList(item) {
    const productsList = this.addonGroupForm.controls.products.value;
    const itemAlreadyInList = productsList.some(p => p.itemId === item.id);
    if (itemAlreadyInList) {
      this.selectedProduct = null;
      return;
    }

    productsList.push({
      itemId: item.id,
      itemName: item.name,
      item: deepCloneObject(item),
    });

    this.addonGroupForm.controls.products.setValue([...productsList]);
    this.selectedProduct = null;
  }

  saveAddonGroup(values: any) {
    const formValues = deepCloneObject(values);
    const addOnGroup = {
      id: formValues.id,
      name: formValues.name,
      allowMultipleSelection: formValues.allowMultipleSelection,
      addOnGroupProducts: formValues.products.map(p => {
        return {
          itemId: p.itemId,
          itemName: p.itemName,
        };
      }),
    };
    this.addOrUpdateAddonGroup.emit({groupId: formValues.id, addOnGroup});
    this.closeAddonConfigDialog();
  }

  getProducts() {
    return this.addonGroupForm.controls.products.value;
  }

  deleteListItem(item) {
    const productsList = this.addonGroupForm.controls.products.value;
    const itemIndex = productsList.indexOf(item);
    if (itemIndex >= 0) {
      productsList.splice(itemIndex, 1);
    }
    this.addonGroupForm.controls.products.setValue([...productsList]);
  }
}
