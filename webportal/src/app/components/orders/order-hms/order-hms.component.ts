import {Component, Input, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {of} from 'rxjs';
import {mergeMap} from 'rxjs/operators';
import {EnumNames, SieveOperators} from 'src/app/enums';
import {
  HospiceFacilityService,
  HospiceLocationService,
  HospiceMemberService,
  HospiceService,
  InventoryService,
  OrderHeadersService,
  PatientService,
} from 'src/app/services';
import {buildFilterString, deepCloneObject, getEnum, getUniqArray, titleCase} from 'src/app/utils';

@Component({
  selector: 'app-order-hms',
  templateUrl: './order-hms.component.html',
  styleUrls: ['./order-hms.component.scss'],
})
export class OrderHmsComponent implements OnInit {
  patientUniqueId: string;
  orderType: string;
  editmode = false;
  patient: any;
  hospiceLocations = [];
  pickupAddresses = [];
  nurses = [];
  patientInventory = null;
  pickupTimes = [
    {
      value: {start: 8, end: 10},
      label: 'Morning: 8AM to 10AM',
      shortLabel: '8AM/10AM',
    },
    {
      value: {start: 12, end: 15},
      label: 'Afternoon: 12PM to 3PM',
      shortLabel: '12PM/3PM',
    },
    {
      value: {start: 17, end: 19},
      label: 'Evening: 5PM to 7PM',
      shortLabel: '5PM/7PM',
    },
    {
      value: {start: 20, end: 22},
      label: 'Night: 8PM to 10PM',
      shortLabel: '8PM/10PM',
    },
  ];

  constructor(
    private route: ActivatedRoute,
    private patientService: PatientService,
    private hospiceService: HospiceService,
    private hospiceLocationService: HospiceLocationService,
    private hospiceFacilityService: HospiceFacilityService,
    private inventoryService: InventoryService,
    private hospiceMemberService: HospiceMemberService,
    private orderHeaderService: OrderHeadersService
  ) {
    this.route.params.subscribe((params: any) => {
      this.patientUniqueId = params.patientUniqueId;
      this.orderType = params.orderType;
    });
    const {url} = this.route.snapshot;
    this.editmode = url[1]?.path === 'edit';
  }

  ngOnInit(): void {
    this.getPatient();
    this.getPatientInventory();
  }

  getPatient() {
    const filter = [
      {
        field: 'uniqueId',
        operator: SieveOperators.Equals,
        value: [this.patientUniqueId],
      },
    ];
    const patientRequest = {
      filters: buildFilterString(filter),
    };
    this.patientService.getPatients(patientRequest).subscribe((response: any) => {
      this.patient = response.records[0] ?? null;
      if (this.patient.hospiceId) {
        this.loadHospice();
        this.getNurses();
      }
      if (this.patient.hospiceLocationId) {
        this.loadHospiceLocation();
      }
    });
  }

  loadHospice() {
    this.hospiceService.getHospiceById(this.patient.hospiceId).subscribe((hospice: any) => {
      if (hospice) {
        this.patient.hospice = hospice;
      }
      this.getPatientFacilities();
    });
  }

  loadHospiceLocation() {
    const filters = [
      {
        field: 'id',
        operator: SieveOperators.Equals,
        value: [this.patient.hospiceLocationId],
      },
    ];
    const request = {
      filters: buildFilterString(filters),
    };
    this.hospiceLocationService.getLocations(request).subscribe((hospiceLocations: any) => {
      this.hospiceLocations = hospiceLocations?.records?.map(hl => {
        return {label: hl.name, value: hl.name};
      });
      const hospiceLocation = hospiceLocations?.records?.find(
        (h: any) => h.id === this.patient.hospiceLocationId
      );
      if (hospiceLocation) {
        this.patient.hospiceLocation = hospiceLocation;
      }
    });
  }

  getPatientFacilities() {
    const filterValues = [
      {
        field: 'patientUuid',
        operator: SieveOperators.Equals,
        value: [this.patientUniqueId],
      },
    ];
    const facilityReq = {filters: buildFilterString(filterValues)};
    this.hospiceFacilityService
      .getHospicePatientsFacilities(this.patient.hospiceId, facilityReq)
      .subscribe((facilityResponse: any) => {
        const facilityAddresses = facilityResponse.records.map((f: any) => {
          if (f.patientRoomNumber) {
            f.facility.address.addressLine1 += ` ${f.patientRoomNumber}`;
          }
          const addressTypeFacility = getEnum(EnumNames.AddressTypes).find(
            a => a.name === 'Facility'
          );
          return {
            address: f.facility.address,
            addressTypeId: addressTypeFacility.id,
          };
        });
        const {patientAddress} = this.patient;
        this.pickupAddresses = [...patientAddress, ...facilityAddresses];
      });
  }

  getNurses() {
    this.nurses = [];
    this.hospiceMemberService
      .getAllHospiceMembers(this.patient.hospiceId)
      .subscribe((response: any) => {
        if (response) {
          const nurses = [];
          response.records.forEach(record => {
            nurses.push({
              label: record.name,
              value: record.id,
            });
          });
          this.nurses = [...nurses];
        }
      });
  }

  getPatientInventory() {
    const filter = [
      {
        field: 'isConsumable',
        operator: SieveOperators.Equals,
        value: [false],
      },
    ];
    const patientInventoryRequest = {
      filters: buildFilterString(filter),
      includePickupDetails: true,
      includeDeliveryAddress: true,
    };
    this.inventoryService
      .getPatientInventoryByUuid(this.patientUniqueId, patientInventoryRequest)
      .pipe(
        mergeMap((response: any) => {
          this.setPatientInventory(response);
          const orderHeaderIds = this.patientInventory
            .map(pi => pi.orderHeaderId)
            .filter(ohid => ohid);
          if (!(orderHeaderIds && orderHeaderIds.length)) {
            return of(null);
          }
          const orderFilter = [
            {
              field: 'id',
              operator: SieveOperators.Equals,
              value: getUniqArray(orderHeaderIds),
            },
          ];
          const request = {filters: buildFilterString(orderFilter)};
          return this.orderHeaderService.getAllOrderHeaders(request);
        })
      )
      .subscribe((orderHeadersResponse: any) => {
        if (
          !(
            orderHeadersResponse &&
            orderHeadersResponse?.records &&
            orderHeadersResponse?.records?.length
          )
        ) {
          return;
        }
        this.patientInventory.forEach(pi => {
          const order = orderHeadersResponse.records.find(ohi => ohi.id === pi.orderHeaderId);
          if (order) {
            pi.orderNumber = order.orderNumber;
          }
        });
      });
  }

  setPatientInventory(patientInventoryResponse) {
    if (patientInventoryResponse) {
      this.patientInventory = patientInventoryResponse.records.map(record => {
        return {
          ...record,
          name: record.itemName,
          assetTagNumber: record.assetTagNumber?.toString() ?? '',
          serialNumber: record.serialNumber?.toString() ?? '',
          lotNumber: record.lotNumber?.toString() ?? '',
          equipmentSettings: [],
        };
      });
    }

    if (!this.patientInventory) {
      this.patientInventory = [];
    }
  }

  getTitleCase(text: string) {
    return titleCase(text);
  }
}
