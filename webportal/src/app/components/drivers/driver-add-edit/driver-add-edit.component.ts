import {Component, OnInit, ViewChild} from '@angular/core';
import {FormGroup, FormBuilder, FormControl, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {
  DriverService,
  ToastService,
  SitesService,
  VehicleService,
  UserService,
} from 'src/app/services';
import {finalize} from 'rxjs/operators';
import {ConfirmDialog, PaginationResponse} from 'src/app/models';
import {deepCloneObject, IsPermissionAssigned} from 'src/app/utils';
import {Location} from '@angular/common';

@Component({
  selector: 'app-driver-add-edit',
  templateUrl: './driver-add-edit.component.html',
  styleUrls: ['./driver-add-edit.component.scss'],
})
export class DriverAddEditComponent implements OnInit {
  editmode = false;
  driverId: number;
  driverForm: FormGroup;
  loading = false;
  driver: any;
  formSubmit = false;
  sites = [];
  vehicles = [];
  site: any;
  vehicle: any;
  searchedUsers = [];
  @ViewChild('confirmDialog', {static: false}) confirmDialog;
  deleteData = new ConfirmDialog();

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private driverService: DriverService,
    private toastService: ToastService,
    private router: Router,
    private sitesService: SitesService,
    private vehicleService: VehicleService,
    private userService: UserService,
    private location: Location
  ) {
    const {url, paramMap} = this.route.snapshot;
    const urlLength = url && url.length;
    this.editmode = url[urlLength - 2]?.path === 'edit';
    this.setDriverForm();
    if (this.editmode) {
      this.driverId = Number(paramMap.get('driverId'));
      this.getDriverDetail();
    }
  }

  setDriverForm() {
    this.driverForm = this.fb.group({
      id: new FormControl(null),
      user: new FormControl(null, Validators.required),
      email: new FormControl(null),
      firstName: new FormControl(null, Validators.required),
      lastName: new FormControl(null, Validators.required),
      countryCode: new FormControl('+1'),
      phoneNumber: new FormControl(0),
      currentSiteId: new FormControl(null),
      currentVehicleId: new FormControl(null),
      currentVehicle: new FormControl(null),
    });
  }

  ngOnInit(): void {}

  getDriverDetail() {
    this.loading = true;
    this.driverService
      .getDriverById(this.driverId)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe((response: any) => {
        this.refreshDriverInfo(response);
      });
  }

  refreshDriverInfo(response) {
    this.driver = response;
    if (response.currentSiteId && response.currentSiteName) {
      this.site = {
        id: response.currentSiteId,
        name: response.currentSiteName,
      };
    }
    this.vehicle = deepCloneObject(response.currentVehicle);
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

  siteSelected(event) {
    this.driverForm.patchValue({currentSiteId: this.site.id || null});
  }

  vehicleSelected(event) {
    this.driverForm.patchValue({currentVehicleId: this.vehicle?.id || null});
  }

  removeItem(field, index) {
    this.driverForm.getRawValue()[field].splice(index, 1);
    this.driverForm.controls[field].setValue([...this.driverForm.getRawValue()[field]]);
  }

  onSubmitDriver(value: any) {
    const body = this.formatValues(value);
    if (this.driverId) {
      this.updateDriver(body);
    } else {
      this.saveDriver(body);
    }
  }

  saveDriver(body) {
    this.formSubmit = true;
    this.driverService
      .saveDriver(body)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe((response: any) => {
        this.toastService.showSuccess(`Driver saved successfully`);
        this.router.navigate([`/drivers/edit/${response.id}`]);
      });
  }

  updateDriver(body) {
    this.formSubmit = true;
    this.driverService
      .updateDriver(this.driverId, body)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe((response: any) => {
        this.driver = response;
        this.toastService.showSuccess(`Driver updated successfully`);
        const driverId = body.currentVehicleId ? response.id : null;
        this.router.navigate([`/drivers/edit/${driverId}`]);
      });
  }

  formatValues(value: any) {
    value.countryCode = parseInt(value.countryCode, 10) ?? 0;
    value.email = value?.user?.email || value.user || value.email;
    if (!this.editmode) {
      delete value.id;
    }
    delete value.user;
    return value;
  }

  searchUsers({query}) {
    if (this.editmode) {
      this.searchedUsers = [];
      return;
    }
    this.userService.searchUser({searchQuery: query}).subscribe((res: any) => {
      this.searchedUsers = res.records.map((usr: any) => {
        return {
          email: usr.email,
          firstName: usr.firstName,
          lastName: usr.lastName,
          phoneNumber: usr.phoneNumber,
        };
      });
    });
  }

  setUserFields(event) {
    this.driverForm.patchValue(event);
  }

  clearEmail() {
    this.driverForm.get('email').reset();
    this.driverForm.get('user').reset();
  }

  deleteDriver() {
    this.deleteData.message = `Do you want to delete Driver ${this.driver.firstName || ''} ${
      this.driver.lastName || ''
    }?`;
    this.confirmDialog.showDialog(this.deleteData);
  }

  deleteConfirmed() {
    this.driverService.deleteDriver(this.driverId).subscribe(() => {
      this.toastService.showSuccess(`Driver Deleted successfully`);
      this.location.back();
    });
  }

  hasPermission(entity, permission = 'Read') {
    return IsPermissionAssigned(entity, permission);
  }
}
