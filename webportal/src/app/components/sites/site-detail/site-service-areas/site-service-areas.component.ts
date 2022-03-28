import {Component, OnInit} from '@angular/core';
import {FormGroup, FormBuilder, FormControl, Validators, FormArray} from '@angular/forms';
import {ActivatedRoute} from '@angular/router';
import {validator} from 'fast-json-patch';
import {finalize} from 'rxjs/operators';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {SitesService, ToastService} from 'src/app/services';
import {IsPermissionAssigned} from 'src/app/utils';

@Component({
  selector: 'app-site-service-areas',
  templateUrl: './site-service-areas.component.html',
  styleUrls: ['./site-service-areas.component.scss'],
})
export class SiteServiceAreasComponent implements OnInit {
  serviceAreasRequest: SieveRequest = new SieveRequest();
  serviceAreasResponse: PaginationResponse;
  siteId: number = Number(this.route.snapshot.paramMap.get('siteId'));
  loading = false;
  serviceAreasHeaders = [
    {
      label: 'Zip Code',
      field: 'zipCode',
    },
  ];

  serviceAreaDeleteHeader = {
    label: '',
    field: '',
    class: 'xs',
    deleteBtn: 'Delete',
    deleteBtnIcon: 'pi pi-trash',
  };
  showAddModal = false;
  zipCodeForm: FormGroup;
  formSubmit = false;
  messages: any;

  constructor(
    private fb: FormBuilder,
    private sitesService: SitesService,
    private route: ActivatedRoute,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    if (this.checkPermission('Site', 'Update') && this.checkPermission('Site', 'Delete')) {
      this.serviceAreasHeaders = [...this.serviceAreasHeaders, this.serviceAreaDeleteHeader];
    }
    this.getServiceAreas();
  }

  setZipcodeForm() {
    this.zipCodeForm = this.fb.group({
      zipCodes: this.fb.array([
        this.fb.group({
          zipCode: new FormControl(null, [Validators.required, Validators.minLength(5)]),
        }),
      ]),
    });
  }

  getServiceAreas() {
    this.loading = true;
    this.sitesService
      .getSiteServiceAreas(this.siteId, this.serviceAreasRequest)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe((res: PaginationResponse) => {
        this.serviceAreasResponse = res;
      });
  }

  getNextServiceAreas({pageNum}) {
    if (!this.serviceAreasResponse || pageNum > this.serviceAreasResponse.totalPageCount) {
      return;
    }

    this.serviceAreasRequest.page = pageNum;
    this.getServiceAreas();
  }

  addZipCodes() {
    this.setZipcodeForm();
    this.showAddModal = true;
  }

  hideZipcodeModal() {
    this.showAddModal = false;
  }

  onSubmitZipcode(values) {
    const zipCodes = [];
    values.zipCodes.forEach(value => {
      zipCodes.push(value.zipCode);
    });

    const uniqueZipCodes = [...new Set(zipCodes)];
    if (zipCodes.length !== uniqueZipCodes.length) {
      this.toastService.showError('Zip Codes are duplicated. Please add unique Zip Codes');
      return;
    }

    this.loading = true;
    this.sitesService
      .addSiteServiceAreas(this.siteId, zipCodes)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(
        () => {
          this.toastService.showSuccess('Zip Codes have been added successfully.');
          this.showAddModal = false;
          this.setZipcodeForm();
          this.serviceAreasRequest = new SieveRequest();
          this.serviceAreasResponse = null;
          this.getServiceAreas();
        },
        error => {
          this.toastService.showError(error.error);
        }
      );
  }

  addOneMore() {
    const zipCodes = this.zipCodeForm.controls.zipCodes as FormArray;
    const newLength = zipCodes.length;
    const newzipCodes: FormGroup = this.fb.group({
      zipCode: new FormControl(null, [Validators.required, Validators.minLength(5)]),
    });
    zipCodes.insert(newLength, newzipCodes);
  }

  checkFormValidity() {
    if (!this.zipCodeForm.valid) {
      return '<span>Required fields are not complete</span>';
    }
    return '';
  }

  deleteZipCode(site) {
    const zipCodes = [];
    zipCodes.push(site.zipCode);
    this.loading = true;
    this.sitesService
      .deleteSiteServiceAreas(this.siteId, zipCodes)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(
        () => {
          this.toastService.showSuccess('Zip Codes have been deleted successfully.');
          this.serviceAreasRequest = new SieveRequest();
          this.serviceAreasResponse = null;
          this.getServiceAreas();
        },
        () => {
          this.toastService.showError('Failed to delete Zip Codes.');
        }
      );
  }

  checkPermission(entity: string, action: string) {
    return IsPermissionAssigned(entity, action);
  }

  getErrorForIndex(idx) {
    const controls = (this.zipCodeForm.controls.zipCodes as any).controls;
    const currentControl = controls[idx];
    if (currentControl.controls.zipCode?.errors?.minlength) {
      return 'Only 5 digit zip codes are accepted';
    }
  }

  removeZipCode(index) {
    const zipCodes = this.zipCodeForm.controls.zipCodes as FormArray;
    zipCodes.removeAt(index);
  }
}
