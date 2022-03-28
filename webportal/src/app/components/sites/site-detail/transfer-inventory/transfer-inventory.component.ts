import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {getEnum} from 'src/app/utils';
import {EnumNames} from 'src/app/enums';
import {
  SitesService,
  ToastService,
  VehicleService,
  ItemsService,
  InventoryService,
} from 'src/app/services';
import {forkJoin} from 'rxjs';
import {finalize, mergeMap} from 'rxjs/operators';
import {PaginationResponse} from 'src/app/models';
import {Location} from '@angular/common';

@Component({
  selector: 'app-transfer-inventory',
  templateUrl: './transfer-inventory.component.html',
  styleUrls: ['./transfer-inventory.component.scss'],
})
export class TransferInventoryComponent implements OnInit {
  itemIds: number[];
  sourceLocationTypeId: number;
  locationTypes: [];
  sourceLocationId: number;
  inventoryTransferForm: FormGroup;
  sourceLocation: any;
  destinationLocation: any;
  destLocationSuggestions = [];
  loading = false;
  items = [];

  listHeaders = [{label: 'Product Name', field: 'name'}];

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private siteService: SitesService,
    private vehicleService: VehicleService,
    private toastService: ToastService,
    private productService: ItemsService,
    private location: Location
  ) {
    const {paramMap, queryParamMap} = this.route.snapshot;
    this.sourceLocationId = Number(paramMap.get('siteId'));
    this.itemIds = JSON.parse(queryParamMap.get('itemIds'));
  }

  ngOnInit(): void {
    this.locationTypes = getEnum(EnumNames.InventoryLocationTypes).flatMap((lt: any) =>
      lt.name === 'Site' || lt.name === 'Vehicle' ? [{label: lt.name, value: lt.id}] : []
    );
    const locationTypeSite: any = this.locationTypes.find((lt: any) => lt.label === 'Site');
    this.sourceLocationTypeId = locationTypeSite?.value;
    this.setTransferForm();
    this.getItemDetails();
    this.getSourceLocation();
  }

  getItemDetails() {
    if (!this.itemIds?.length) {
      return;
    }
    let itemReq = [];
    itemReq = this.itemIds.map((id: number) => this.productService.getItemDetailsById(id));
    this.loading = true;
    forkJoin(itemReq)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe((res: any) => {
        this.items = res;
      });
  }

  setTransferForm() {
    this.inventoryTransferForm = this.fb.group({
      sourceLocationTypeId: new FormControl(
        {value: this.sourceLocationTypeId || null, disabled: true},
        Validators.required
      ),
      sourceLocation: new FormControl({value: null, disabled: true}, Validators.required),
      destinationLocationTypeId: new FormControl(null, Validators.required),
      destinationLocation: new FormControl(null, Validators.required),
    });
  }

  searchLocation({query}) {
    this.inventoryTransferForm.get('destinationLocation').reset();
    const locationTypeId = this.inventoryTransferForm.get('destinationLocationTypeId').value;
    const locationType: any = this.locationTypes.find((lt: any) => lt.value === locationTypeId);
    let searchLocationReq: any;
    switch (locationType.label) {
      case 'Site':
        searchLocationReq = this.siteService.searchSites({searchQuery: query});
        break;
      case 'Vehicle':
        searchLocationReq = this.vehicleService.searchVehicles({searchQuery: query});
        break;
    }

    if (!searchLocationReq) {
      this.toastService.showWarning('Please select a valid location type');
      this.destLocationSuggestions = [];
      return;
    }
    searchLocationReq.subscribe((res: PaginationResponse) => {
      this.destLocationSuggestions = res.records.map((lt: any) => {
        return {name: lt.name, id: lt.id};
      });
    });
  }

  destLocationSelected() {
    if (typeof this.destinationLocation !== 'object') {
      this.destinationLocation = '';
      this.toastService.showError('Please select a valid value');
      return;
    }
    this.inventoryTransferForm.get('destinationLocation').setValue(this.destinationLocation);
  }

  getSourceLocation() {
    this.siteService.getSiteById(this.sourceLocationId).subscribe((res: any) => {
      this.sourceLocation = {name: res.name, id: res.id};
      this.inventoryTransferForm.patchValue({sourceLocation: this.sourceLocation});
    });
  }

  requestTransfer() {
    const values = this.formatValue(this.inventoryTransferForm.getRawValue());
    const itemTransferReqs = [];
    this.items.forEach(({id}: any) => {
      itemTransferReqs.push(this.productService.transferProduct(id, values));
    });
    this.loading = true;
    forkJoin(itemTransferReqs)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(() => {
        this.toastService.showSuccess('Transfer Request created successfully');
        this.location.back();
      });
  }

  formatValue(values) {
    if (
      typeof values.destinationLocation !== 'object' ||
      typeof values.sourceLocation !== 'object'
    ) {
      this.toastService.showError('Please select valid values');
      return;
    }
    return {
      sourceLocationTypeId: values.sourceLocationTypeId,
      sourceLocationId: values.sourceLocation.id,
      destinationLocationTypeId: values.destinationLocationTypeId,
      destinationLocationId: values.destinationLocation.id,
    };
  }
}
