import {Component, OnInit, ViewChild} from '@angular/core';
import {FormGroup, FormBuilder, FormControl, Validators} from '@angular/forms';
import {InventoryItem} from 'src/app/models/model.inventory-item';
import {ActivatedRoute, Router} from '@angular/router';
import {InventoryService} from 'src/app/services/inventory.service';
import {
  ToastService,
  ItemsService,
  SitesService,
  PatientService,
  VehicleService,
} from 'src/app/services';
import {finalize, mergeMap} from 'rxjs/operators';
import {Item, PaginationResponse, ConfirmDialog} from 'src/app/models';
import {createPatch, deepCloneObject, getEnum, IsPermissionAssigned} from 'src/app/utils';
import {EnumNames} from 'src/app/enums';
import {EMPTY} from 'rxjs';
import {Location} from '@angular/common';

@Component({
  selector: 'app-add-edit-inventory',
  templateUrl: './add-edit-inventory.component.html',
  styleUrls: ['./add-edit-inventory.component.scss'],
})
export class AddEditInventoryComponent implements OnInit {
  inventoryForm: FormGroup;
  editmode: boolean;
  loading = false;
  formSubmit = false;
  inventoryItem: InventoryItem;
  inventoryId: number;
  itemsList = [];
  location: any;
  locations = [];
  item: any;
  itemIsSerialized = true;
  locationNameField = 'name';
  emptyAutocompleteLabel = 'No matching results found';
  locationTypes;
  itemStatus;

  @ViewChild('confirmDialog', {static: false}) confirmDialog;
  deleteData = new ConfirmDialog();

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private inventoryService: InventoryService,
    private toaster: ToastService,
    private itemService: ItemsService,
    private siteService: SitesService,
    private angularLocation: Location
  ) {}

  ngOnInit(): void {
    this.locationTypes = getEnum(EnumNames.InventoryLocationTypes).flatMap((l: any) => {
      if (['Site', 'Vehicle', 'Patient'].includes(l.name)) {
        return {label: l.name, value: l.id};
      }
      return [];
    });
    this.itemStatus = getEnum(EnumNames.InventoryStatusTypes).map((s: any) => {
      return {label: s.name, value: s.id};
    });
    const {url, paramMap} = this.route.snapshot;
    const urlLength = url && url.length;
    this.editmode = url[urlLength - 2]?.path === 'edit';
    this.itemsList = [];
    this.setInventoryForm();
    if (this.editmode) {
      this.inventoryId = Number(paramMap.get('inventoryId'));
      this.getInventoryItemDetails();
    }
  }

  getInventoryItemDetails() {
    this.loading = true;
    this.inventoryService
      .getInventoryItemById(this.inventoryId)
      .pipe(
        mergeMap((itemDetails: any) => {
          this.inventoryItem = itemDetails;
          this.item = {
            id: this.inventoryItem.item?.id,
            name: this.inventoryItem.item?.name,
          };
          this.inventoryForm.patchValue(this.inventoryItem);
          if (!this.inventoryItem.serialNumber) {
            this.itemIsSerialized = false;
          }
          if (this.inventoryForm.controls.count.value === 0) {
            this.inventoryForm.controls.count.enable();
          }
          delete this.inventoryItem.item;
          delete this.inventoryItem.status;
          delete this.inventoryItem.assetTagNumber;
          delete this.inventoryItem.lotNumber;
          if (itemDetails?.currentLocationId) {
            return this.siteService.searchSites({searchQuery: ''});
          } else {
            return EMPTY;
          }
        }),
        finalize(() => (this.loading = false))
      )
      .subscribe((sites: any) => {
        if (!sites?.records) {
          return;
        }
        const currentLocationDetails = sites.records.find(
          x => x.id === this.inventoryItem?.currentLocationId
        );
        if (currentLocationDetails) {
          const location = {
            name: currentLocationDetails.name,
            id: currentLocationDetails.id,
          };
          this.location = currentLocationDetails;
          this.inventoryForm.patchValue({location});
        }
      });
  }

  setInventoryForm() {
    this.inventoryForm = this.fb.group({
      id: new FormControl(null),
      itemId: new FormControl(null, Validators.required),
      item: new FormControl(null, Validators.required),
      serialNumber: new FormControl({value: null, disabled: this.editmode}),
      count: new FormControl({value: null, disabled: this.editmode}, Validators.required),
      quantityAvailable: new FormControl(null),
      statusId: new FormControl(null),
      currentLocationId: new FormControl(null, Validators.required),
      location: new FormControl(null, Validators.required),
    });
  }

  onSubmitItem() {
    let values = deepCloneObject(this.inventoryForm.getRawValue());
    values = this.formatValues(values);
    if (this.editmode) {
      this.updateInventoryItem(values);
    } else {
      this.saveInventoryItem(values);
    }
  }

  updateInventoryItem(values) {
    if (values.count !== 0 && this.inventoryItem.count !== values.count) {
      values.quantityAvailable = values.count;
      values.count = this.inventoryItem.count;
    }
    const patch = createPatch(this.inventoryItem, values);
    if (!patch.length) {
      return;
    }
    this.formSubmit = true;
    this.inventoryService
      .updateInventoryItem(this.inventoryId, patch)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe((res: any) => {
        this.toaster.showSuccess('Inventory item updated successfully');
        if (this.inventoryItem.quantityAvailable !== res.quantityAvailable) {
          this.inventoryItem.quantityAvailable = res.quantityAvailable;
          this.inventoryItem.count = res.count;
          this.inventoryForm.controls.count.disable();
        }
        this.getInventoryItemDetails();
      });
  }

  serialNumberChanged(event) {
    const {value} = event.currentTarget;
    if (!value || !value.length) {
      this.inventoryForm.controls.count.enable();
      return;
    }
    this.inventoryForm.controls.count.disable();
    this.inventoryForm.patchValue({count: 1});
  }

  saveInventoryItem(values) {
    this.formSubmit = true;
    values.quantityAvailable = values.count;
    this.inventoryService
      .createInventoryItem(values)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe(res => {
        this.toaster.showSuccess('Inventory Item Created Successfully');
        this.angularLocation.back();
      });
  }

  itemSelected(item) {
    this.inventoryForm.patchValue({
      item,
      itemId: item.id,
    });
    this.inventoryForm.markAsTouched();
  }

  searchItems({query}) {
    this.itemService.searchItems({searchQuery: query}).subscribe((res: any) => {
      this.itemsList = res.records.map(p => ({id: p.id, name: p.name}));
    });
  }

  searchLocation({query}) {
    this.siteService
      .searchSites({
        searchQuery: query,
      })
      .subscribe((locationRes: PaginationResponse) => {
        this.locations = locationRes.records.map((location: any) => {
          return {name: location.name, id: location.id};
        });
      });
  }

  locationSelected(location) {
    this.inventoryForm.get('location').setValue({...location});
    this.inventoryForm.get('currentLocationId').setValue(location.id);
    this.inventoryForm.markAsTouched();
  }

  formatValues(values) {
    delete values.location;
    delete values.item;
    if (this.editmode) {
      values.netSuiteInventoryId = this.inventoryItem.netSuiteInventoryId;
    }
    return values;
  }

  clearAutocompleteField(field) {
    this[field] = this.inventoryForm.get(field).setValue(null);
  }

  deleteInventoryItem() {
    this.deleteData.message = `Do you want to delete Inventory Item ${
      this.inventoryItem.serialNumber || ''
    }?`;
    this.confirmDialog.showDialog(this.deleteData);
  }

  deleteConfirmed() {
    this.inventoryService
      .deleteInventoryItem(this.inventoryItem.id)
      .subscribe((response: Response) => {
        this.toaster.showSuccess(`Inventory Item Deleted successfully`);
        this.angularLocation.back();
      });
  }

  canDelete() {
    return IsPermissionAssigned('Inventory', 'Delete');
  }
}
