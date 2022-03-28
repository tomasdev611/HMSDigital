import {Component, OnInit, ViewChild} from '@angular/core';
import {FormGroup, FormBuilder, FormControl, Validators} from '@angular/forms';
import {SieveRequest, ConfirmDialog, PaginationResponse} from 'src/app/models';
import {DriverService, ToastService, SitesService, VehicleService} from 'src/app/services';
import {finalize} from 'rxjs/operators';
import {buildFilterString, deepCloneObject, IsPermissionAssigned} from 'src/app/utils';
import {NavbarSearchService} from 'src/app/services/navbar-search.service';
import {Subscription} from 'rxjs';
import {SieveOperators} from 'src/app/enums/sieve-operators';
import {TableVirtualScrollComponent} from 'src/app/common';

@Component({
  selector: 'app-drivers',
  templateUrl: './drivers.component.html',
  styleUrls: ['./drivers.component.scss'],
})
export class DriversComponent implements OnInit {
  showCurrentSiteVehicles = true;
  currentSiteVehicles = [];
  vehiclesRequest = new SieveRequest();
  loading = false;
  editFlyoutOpen = false;
  driversResponse: PaginationResponse;
  driverRequest = new SieveRequest();
  searchQuery = '';
  sites = [];
  vehicles = [];
  selectedDriver: any;
  pageNumber = 0;
  totalPageCount = 0;
  site: any;
  vehicle: any;
  vehicleId: any;
  headers = [
    {label: 'Name', field: 'name', sortable: true},
    {label: 'Phone', field: 'phoneNumber', fieldType: 'Phone'},
    {label: 'Email', field: 'email'},
    {label: 'Site', field: 'currentSiteName'},
  ];

  driverForm: FormGroup;
  formSubmit = false;
  subscriptions: Subscription[] = [];
  @ViewChild('confirmDialog', {static: false}) confirmDialog;
  deleteData = new ConfirmDialog();
  @ViewChild('driversTable ', {static: false})
  driversTable: TableVirtualScrollComponent;

  constructor(
    private driverService: DriverService,
    private toastService: ToastService,
    private navbarSearchService: NavbarSearchService,
    private fb: FormBuilder,
    private sitesService: SitesService,
    private vehicleService: VehicleService
  ) {
    this.setDriverForm();
  }

  setDriverForm() {
    this.driverForm = this.fb.group({
      id: new FormControl(null),
      user: new FormControl(null, Validators.required),
      email: new FormControl(null, Validators.required),
      firstName: new FormControl(null, Validators.required),
      lastName: new FormControl(null, Validators.required),
      countryCode: new FormControl('+1'),
      phoneNumber: new FormControl(0, Validators.required),
      currentSiteId: new FormControl(null),
      currentVehicleId: new FormControl(null),
      currentVehicle: new FormControl(null),
    });
  }

  patchDriverInfo() {
    this.loading = true;
    this.driverService
      .getDriverById(this.selectedDriver.id)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe((response: any) => {
        if (response.currentSiteId && response.currentSiteName) {
          this.site = {
            id: response.currentSiteId,
            name: response.currentSiteName,
          };
        } else {
          this.site = {
            id: null,
            name: null,
          };
        }
        this.vehicle = deepCloneObject(response.currentVehicle);
        this.vehicleId = deepCloneObject(response.currentVehicleId);
        this.driverForm.patchValue({
          ...response,
          user: {
            email: response.email,
            firstName: response.firstName,
            lastName: response.lastName,
            phoneNumber: response.phoneNumber,
          },
          countryCode: response.countryCode ? `+${response.countryCode}` : 0,
        });
      });
  }

  ngOnInit(): void {
    this.getDrivers();
    this.subscriptions.push(
      this.navbarSearchService.search.subscribe(text => this.searchDriver(text))
    );
  }

  getDrivers() {
    this.loading = true;
    (!this.searchQuery
      ? this.driverService.getAllDrivers(this.driverRequest)
      : this.driverService.searchDrivers({
          ...this.driverRequest,
          searchQuery: this.searchQuery,
        })
    )
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((response: any) => {
        this.driversResponse = response;
        this.driversResponse.records = this.driversResponse.records.map(x => {
          x.name = `${x.firstName}${x.lastName ? ' ' + x.lastName : ''}`;
          return x;
        });
      });
  }

  nextDrivers({pageNum}) {
    if (!this.driversResponse || pageNum > this.driversResponse.totalPageCount) {
      return;
    }
    this.driverRequest.page = pageNum;
    this.getDrivers();
  }

  searchDriver(searchQuery) {
    this.dataTablesReset();
    this.driverRequest.page = 1;
    this.searchQuery = searchQuery;
    this.getDrivers();
  }

  searchSites({query}) {
    this.sitesService.searchSites({searchQuery: query}).subscribe((siteRes: PaginationResponse) => {
      this.sites = siteRes.records;
    });
  }

  searchVehicles({query}) {
    this.vehicleService
      .searchVehicles({searchQuery: query})
      .subscribe((vehiclesRes: PaginationResponse) => {
        this.vehicles = vehiclesRes.records;
      });
  }

  getCurrentSiteVehicles() {
    this.vehicleService
      .getAllVehicles(this.vehiclesRequest)
      .subscribe((vehiclesRes: PaginationResponse) => {
        this.currentSiteVehicles = vehiclesRes.records.map(record => {
          record.label = record.name;
          record.value = record.id;
          return record;
        });
      });
  }

  showDriverDetails({currentRow}) {
    this.closeFlyout();
    this.selectedDriver = currentRow;
    this.showCurrentSiteVehicles = true;
    this.vehiclesRequest.filters = buildFilterString([
      {
        field: 'SiteId',
        operator: SieveOperators.Equals,
        value: [this.selectedDriver.currentSiteId],
      },
    ]);
    this.getCurrentSiteVehicles();
    this.patchDriverInfo();
    this.editFlyoutOpen = true;
  }

  closeFlyout() {
    this.editFlyoutOpen = false;
  }

  onSubmitDriver(value: any) {
    const body = this.formatValues(value);
    this.updateDriver(body);
  }

  formatValues(value: any) {
    value.countryCode = parseInt(value.countryCode, 10) ?? 0;
    value.email = value?.user?.email || value.user || value.email;
    delete value.user;
    return value;
  }

  updateDriver(body) {
    this.formSubmit = true;
    this.driverService
      .updateDriver(this.selectedDriver.id, body)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe((response: any) => {
        this.driversResponse.records = this.driversResponse.records.map(item => {
          if (item.id === response.id) {
            item = response;
            item.name = `${item.firstName}${item.lastName ? ' ' + item.lastName : ''}`;
            this.selectedDriver = item;
          }
          return item;
        });
        this.toastService.showSuccess(`Driver updated successfully`);
      });
  }

  siteSelected(event) {
    this.driverForm.patchValue({currentSiteId: this.site.id || null});
    this.vehiclesRequest.filters = buildFilterString([
      {
        field: 'SiteId',
        operator: SieveOperators.Equals,
        value: [event.id],
      },
    ]);
    this.getCurrentSiteVehicles();
  }

  currentSiteVechicleSelected(event) {
    this.driverForm.patchValue({currentVehicleId: this.vehicleId || null});
  }

  vehicleSelected(event) {
    this.driverForm.patchValue({currentVehicleId: this.vehicle?.id || null});
  }

  deleteDriver() {
    this.deleteData.message = `Do you want to delete Driver ${
      this.selectedDriver.firstName || ''
    } ${this.selectedDriver.lastName || ''}?`;
    this.confirmDialog.showDialog(this.deleteData);
  }

  deleteConfirmed() {
    this.driverService.deleteDriver(this.selectedDriver.id).subscribe((response: any) => {
      this.initDriversList();
      this.editFlyoutOpen = false;
      this.toastService.showSuccess(`Driver Deleted successfully`);
    });
  }

  hasPermission(entity, permission = 'Read') {
    return IsPermissionAssigned(entity, permission);
  }

  dataTablesReset() {
    if (this.driversTable) {
      this.driversTable.reset();
    }
    this.driverRequest.page = 1;
  }

  getVehicleNameById(id) {
    return this.currentSiteVehicles.find(vehicle => vehicle.id === id)?.name;
  }

  getVehicleLicensePlateById(id) {
    return this.currentSiteVehicles.find(vehicle => vehicle.id === id)?.licensePlate;
  }

  initDriversList() {
    this.searchQuery = '';
    this.dataTablesReset();
    this.getDrivers();
  }

  sortDrivers(event) {
    switch (event.order) {
      case 0:
        this.driverRequest.sorts = '';
        break;
      case 1:
        this.driverRequest.sorts = event.field;
        break;
      case -1:
        this.driverRequest.sorts = '-' + event.field;
        break;
    }
    this.dataTablesReset();
    this.getDrivers();
  }
}
