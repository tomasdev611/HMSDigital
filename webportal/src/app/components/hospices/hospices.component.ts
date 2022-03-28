import {Component, OnInit, ViewChild} from '@angular/core';
import {HospiceLocationService, HospiceService} from 'src/app/services';
import {finalize} from 'rxjs/operators';
import {IsPermissionAssigned} from 'src/app/utils';
import {SieveRequest, PaginationResponse} from 'src/app/models';
import {Router} from '@angular/router';
import {NavbarSearchService} from 'src/app/services/navbar-search.service';
import {TableVirtualScrollComponent} from 'src/app/common';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-hospices',
  templateUrl: './hospices.component.html',
  styleUrls: ['./hospices.component.scss'],
})
export class HospicesComponent implements OnInit {
  hospiceRequest = new SieveRequest();
  hospiceResponse: PaginationResponse;
  subscriptions: Subscription[] = [];

  headers = [
    {label: 'Name', field: 'name', sortable: true},
    {label: 'Locations', field: 'hospiceLocations.length'},
    {actionBtn: 'View Contracts', actionBtnIcon: 'pi pi-file', class: 'btn-option'},
  ];
  loading = false;
  @ViewChild('hospicesTable ', {static: false})
  hospicesTable: TableVirtualScrollComponent;
  searchQuery: string;

  constructor(
    private hospiceService: HospiceService,
    private router: Router,
    private navbarSearchService: NavbarSearchService,
    private hospiceLocationService: HospiceLocationService
  ) {}

  ngOnInit(): void {
    this.getAllHospices();
    this.setActionButton();
    this.subscriptions.push(
      this.navbarSearchService.search.subscribe(text => this.searchHospice(text))
    );
  }

  setActionButton() {
    let actionButtonHeaders: any = {
      label: '',
      field: '',
      class: 'btn-option',
      linkParams: 'id',
    };
    if (IsPermissionAssigned('Hospice', 'Read')) {
      actionButtonHeaders = {
        ...actionButtonHeaders,
        editBtn: 'View Hospice',
        editBtnIcon: 'pi pi-info-circle',
        editBtnLink: '/hospice',
      };
    }
    this.headers.push(actionButtonHeaders);
  }

  getAllHospices() {
    this.loading = true;
    this.hospiceService
      .getAllhospices(this.hospiceRequest)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((response: PaginationResponse) => {
        this.hospiceResponse = response;
        if (this.hospiceResponse.totalRecordCount === 1) {
          this.router.navigate([`/hospice/${this.hospiceResponse.records[0].id}`]);
        } else if (this.hospiceResponse.totalRecordCount === 0) {
          this.getHospiceLocations();
        }
      });
  }

  getHospiceLocations(hospiceId?) {
    const hospiceLocationRequest = new SieveRequest();
    this.hospiceLocationService.getLocations(hospiceLocationRequest).subscribe((response: any) => {
      if (response.totalRecordCount > 0) {
        this.router.navigate([`/hospice/${response.records[0].hospiceId}`]);
      }
    });
  }

  getNext({pageNum}) {
    if (!this.hospiceResponse || pageNum > this.hospiceResponse.totalPageCount) {
      return;
    }
    this.hospiceRequest.page = pageNum;
    this.getAllHospices();
  }

  searchHospice(searchQuery) {
    this.dataTablesReset();
    this.hospiceRequest.page = 1;
    this.searchQuery = searchQuery;
    if (!searchQuery) {
      this.getAllHospices();
      return;
    }
    this.loading = true;
    this.hospiceService
      .searchHospices({...this.hospiceRequest, searchQuery})
      .pipe(finalize(() => (this.loading = false)))
      .subscribe((res: PaginationResponse) => {
        this.hospiceResponse = res;
      });
  }

  dataTablesReset() {
    if (this.hospicesTable) {
      this.hospicesTable.reset();
    }
    this.hospiceRequest.page = 1;
  }

  sortHospices(event) {
    switch (event.order) {
      case 0:
        this.hospiceRequest.sorts = '';
        break;
      case 1:
        this.hospiceRequest.sorts = event.field;
        break;
      case -1:
        this.hospiceRequest.sorts = '-' + event.field;
        break;
    }
    this.dataTablesReset();
    this.searchHospice(this.searchQuery);
  }

  viewHospiceContracts(hospice) {
    this.router.navigate([`/hospice/${hospice.id}`], {queryParams: {view: 'contracts'}});
  }
}
