import {Component, OnInit, ViewChild} from '@angular/core';
import {VehicleService} from 'src/app/services';
import {finalize} from 'rxjs/operators';
import {SieveRequest, PaginationResponse} from 'src/app/models';
import {NavbarSearchService} from 'src/app/services/navbar-search.service';
import {TableVirtualScrollComponent} from 'src/app/common';
import {Subscription} from 'rxjs';
import {buildFilterString} from 'src/app/utils';
import {SieveOperators} from 'src/app/enums';

@Component({
  selector: 'app-vehicles',
  templateUrl: './vehicles.component.html',
  styleUrls: ['./vehicles.component.scss'],
})
export class VehiclesComponent implements OnInit {
  @ViewChild('vehiclesTable ', {static: false})
  vehiclesTable: TableVirtualScrollComponent;

  loading = false;
  vehicleRequest = new SieveRequest();
  vehiclesResponse: PaginationResponse;
  selectedVehicle: any;
  headers = [
    {
      label: 'CVN',
      class: 'md',
      field: 'cvn',
      sortable: true,
      sortField: 'cvn',
    },
    {
      label: 'License Plate',
      class: 'md',
      field: 'licensePlate',
      sortable: true,
    },
    {
      label: 'Driver',
      field: 'currentDriverName',
      sortable: true,
      sortField: 'currentDriverName',
    },
    {label: 'VIN', field: 'vin'},
    {label: 'Site', field: 'siteName', sortable: true},
    {
      label: '',
      field: '',
      class: 'sm',
      actionBtn: 'View Inventory',
      actionBtnLabel: '',
      actionBtnIcon: 'pi pi-list',
    },
  ];
  showFlyout = false;
  subscriptions: Subscription[] = [];
  constructor(
    private vehicleService: VehicleService,
    private navbarSearchService: NavbarSearchService
  ) {}

  ngOnInit(): void {
    this.getVehicles();
    this.subscriptions.push(
      this.navbarSearchService.search.subscribe(text => this.searchVehicles(text))
    );
  }

  getVehicles() {
    this.loading = true;
    this.vehicleService
      .getAllVehicles(this.vehicleRequest)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((response: PaginationResponse) => {
        this.vehiclesResponse = response;
      });
  }

  nextVehicles({pageNum}) {
    if (
      !this.vehiclesResponse ||
      this.vehiclesResponse.pageNumber >= this.vehiclesResponse.totalPageCount
    ) {
      return;
    }
    this.vehicleRequest.page = pageNum;
    this.getVehicles();
  }

  searchVehicles(searchQuery) {
    this.dataTablesReset();
    this.vehicleRequest.page = 1;
    if (searchQuery) {
      this.vehicleRequest.filters = buildFilterString([
        {
          fields: this.headers.filter(h => h.field).map(h => h.field),
          operator: SieveOperators.CI_Contains,
          value: [searchQuery],
        },
      ]);
    } else {
      this.vehicleRequest.filters = '';
    }
    this.getVehicles();
  }

  sortVehicles(event) {
    this.dataTablesReset();
    switch (event.order) {
      case 0:
        this.vehicleRequest.sorts = '';
        break;
      case 1:
        this.vehicleRequest.sorts = event.field;
        break;
      case -1:
        this.vehicleRequest.sorts = '-' + event.field;
        break;
    }
    this.getVehicles();
  }
  showInventoryFlyout(vehicle) {
    this.selectedVehicle = vehicle;
    this.showFlyout = true;
  }

  closeInventoryFlyout() {
    this.showFlyout = false;
    this.selectedVehicle = null;
  }

  dataTablesReset() {
    if (this.vehiclesTable) {
      this.vehiclesTable.reset();
    }
    this.vehicleRequest.page = 1;
  }
}
