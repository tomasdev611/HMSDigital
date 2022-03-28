import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import {finalize, mergeMap} from 'rxjs/operators';
import {EnumNames, SieveOperators} from 'src/app/enums';
import {ConfirmDialog, PaginationResponse, SieveRequest} from 'src/app/models';
import {HospiceLocationService, HospiceService, PatientService} from 'src/app/services';
import {
  buildFilterString,
  formatDateToString,
  getEnum,
  getUniqArray,
  IsPermissionAssigned,
} from 'src/app/utils';

@Component({
  selector: 'app-create-order-modal',
  templateUrl: './create-order-modal.component.html',
  styleUrls: ['./create-order-modal.component.scss'],
})
export class CreateOrderModalComponent implements OnInit {
  @Input() patientContext;
  @Output() next = new EventEmitter<any>();
  @Output() cancel = new EventEmitter<any>();
  patientSelected: any;
  patientsLoading = false;
  orderType: string;
  orderBtns = [
    {name: 'Delivery', value: 'delivery', disabled: false},
    {name: 'Exchange', value: 'exchange', disabled: false},
    {name: 'Move', value: 'move', disabled: false},
    {name: 'Pickup', value: 'pickup', disabled: false},
  ];
  orderingEnabled: boolean;
  patientReq = new SieveRequest();
  patientsList: any;
  patientAddressHome = getEnum(EnumNames.AddressTypes).find(a => a.name === 'Home');
  patientResponse: PaginationResponse;
  creditHoldMessage = '';
  @ViewChild('confirmDialog', {static: false}) confirmDialog;
  confirmData = new ConfirmDialog();

  constructor(
    private patientService: PatientService,
    private hospiceLocationService: HospiceLocationService,
    private hospiceService: HospiceService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.orderType = this.orderBtns[0].value;
  }

  showOrdering() {
    this.orderingEnabled = true;
    if (!this.patientContext) {
      this.searchPatients();
    } else {
      this.hospiceService
        .getHospiceById(this.patientContext.hospiceId)
        .subscribe((response: any) => {
          const hospice = response;
          if (hospice?.isCreditOnHold) {
            this.creditHoldMessage = this.getCreditHoldMessage(this.patientContext.hospice);
            this.orderBtns.map((x: any) => {
              this.orderType = '';
              x.disabled =
                x.value !== 'pickup' && !this.hasPermission('Orders', 'OverrideCreditHold')
                  ? true
                  : false;
              return x;
            });
          } else {
            this.creditHoldMessage = '';
            this.orderType = this.orderBtns[0].value;
          }
        });
    }
  }

  getCreditHoldMessage(hopiceLocationName) {
    return `Delivery, Exchange and Move order for ${hopiceLocationName} are on Credit-Hold`;
  }

  closeCreateOrderModal(event?) {
    this.orderType = null;
    this.orderingEnabled = false;
    this.cancel.emit();
  }

  proceedToCreateOrder(event?) {
    const patient = this.patientContext || this.patientSelected;
    if (this.creditHoldMessage && this.orderType !== 'pickup') {
      this.confirmData.data = {
        orderType: this.orderType,
        patient,
        mode: 'hospice-credit-hold-check',
      };
      this.confirmData.header = `Credit-Hold`;
      this.confirmData.message = `Are you sure you want to proceed? ${this.creditHoldMessage}.`;
      this.closeCreateOrderModal();
      this.confirmDialog.showDialog(this.confirmData);
      return false;
    } else {
      this.navigateOrEmitCreateOrder(this.orderType, patient);
    }
  }

  navigateOrEmitCreateOrder(orderType: string, patient: any) {
    if (!orderType && !patient) {
      return;
    }
    return this.router.navigate([`/orders/create/${orderType}/${patient.uniqueId}`]);
  }

  emitCreateOrderAccepted(data?) {
    this.next.emit({
      orderType: data?.orderType || this.orderType,
      patient: data?.patient || this.patientContext || this.patientSelected,
    });
  }

  subscribePatients(request) {
    request
      .pipe(
        mergeMap((patientResponse: PaginationResponse) => {
          this.patientResponse = patientResponse;
          return this.getHospiceRequest();
        }),
        finalize(() => (this.patientsLoading = false))
      )
      .subscribe((hospicelocations: any) => {
        this.formatPatientResponse(hospicelocations);
        this.getHospices();
      });
  }

  filterPatients(filterString) {
    this.patientReq.filters = filterString;
    this.subscribePatients(this.getPatients());
  }

  searchPatients(searchQuery?) {
    this.patientReq.page = 1;
    const patientReq = searchQuery
      ? this.patientService.searchPatientsBySearchQuery({
          searchQuery,
          ...this.patientReq,
        })
      : this.getPatients();

    this.patientsLoading = true;

    this.subscribePatients(patientReq);
  }

  getPatients() {
    return this.patientService.getPatients(this.patientReq);
  }

  getHospiceRequest() {
    const hospiceLocationIds = getUniqArray(
      this.patientResponse.records.map(({hospiceLocationId}: any) => {
        return hospiceLocationId;
      })
    );
    const filterValues = [
      {
        field: 'id',
        operator: SieveOperators.Equals,
        value: [...hospiceLocationIds],
      },
    ];
    const hospiceLocationReq = {filters: buildFilterString(filterValues)};
    return this.hospiceLocationService.getLocations(hospiceLocationReq);
  }

  getHospices() {
    const hospiceIds = getUniqArray(
      this.patientResponse.records.map(({hospiceId}: any) => {
        return hospiceId;
      })
    );
    const filterValues = [
      {
        field: 'id',
        operator: SieveOperators.Equals,
        value: [...hospiceIds],
      },
    ];
    const hospiceReq = {filters: buildFilterString(filterValues)};
    this.hospiceService.getAllhospices(hospiceReq).subscribe((hospices: any) => {
      this.patientsList = this.patientsList.map(x => {
        const hospice = hospices?.records.find(h => h.id === x.value.hospiceId);
        if (hospice) {
          x.value.hospice = hospice;
        }
        return x;
      });
    });
  }

  formatPatientResponse(hospices) {
    this.patientsList = this.patientResponse.records.map((p: any) => {
      const hospiceLocation = hospices.records.find((h: any) => h.id === p.hospiceLocationId);
      if (hospiceLocation) {
        p.hospiceLocationName = hospiceLocation.name;
      }
      const name = `${p.firstName || ''} ${p.lastName}`;
      let address = p.patientAddress.length ? p.patientAddress[0].address : null;
      address = this.getAddressString(address);
      return {label: name, value: {...p, name, address}};
    });
    if (!this.patientsList.length) {
      this.patientSelected = null;
    }
  }

  getAddressString(address) {
    if (!address || (!address.addressLine1 && !address.state)) {
      return `Address not available`;
    }
    return `${address.addressLine1 ? address.addressLine1 + ',' : ','}
     ${address.addressLine2 ? address.addressLine2 + ',' : ''} ${
      address.city ? address.city + ',' : ''
    }
    ${address.state ? address.state : ''}`;
  }
  patientSelection(event) {
    if (event.value.hospice?.isCreditOnHold) {
      this.orderType = '';
      this.creditHoldMessage = this.getCreditHoldMessage(event.value.hospiceLocationName);
    } else {
      this.creditHoldMessage = '';
    }
    this.orderBtns.map((x: any) => {
      x.disabled =
        event.value.hospice?.isCreditOnHold &&
        x.value !== 'pickup' &&
        !this.hasPermission('Orders', 'OverrideCreditHold')
          ? true
          : false;
      return x;
    });

    if (event?.value?.status === 'Inactive') {
      this.confirmData.header = 'Inactive Patient';
      const patientReason = event?.value?.statusReason
        ? `as patient was identified as ${event?.value?.statusReason} on
        ${formatDateToString(event?.value?.statusChangedDate, 'MM/DD/YYYY')}`
        : '';
      this.confirmData.message = `Please confirm that you want to create a new order for <strong>${
        event?.value?.firstName || ''
      }
        ${
          event?.value?.lastName || ''
        }</strong> ${patientReason}. <div>Do you want to continue?</div>`;
      this.confirmData.data = {
        patient: event?.value,
        mode: 'patient-status-check',
      };
      this.orderingEnabled = false;
      this.confirmDialog.showDialog(this.confirmData);
    }
  }

  hasPermission(entity, permission = 'Read') {
    return IsPermissionAssigned(entity, permission);
  }

  confirmAccepted() {
    const {orderType, patient, mode} = this.confirmData?.data;

    this.confirmDialog.close();
    if (mode === 'patient-status-check') {
      this.orderingEnabled = true;
      return;
    }
    this.creditHoldMessage = '';
    this.patientSelected = null;
    this.navigateOrEmitCreateOrder(orderType, patient);
  }

  confirmRejected() {
    this.creditHoldMessage = '';
    this.orderType = '';
    this.patientSelected = null;
    this.confirmDialog.close();
  }
}
