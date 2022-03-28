import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {HospiceService} from 'src/app/services';
import {finalize} from 'rxjs/operators';
import {IsPermissionAssigned} from 'src/app/utils';
import {Location} from '@angular/common';

@Component({
  selector: 'app-hospice-detail',
  templateUrl: './hospice-detail.component.html',
  styleUrls: ['./hospice-detail.component.scss'],
})
export class HospiceDetailComponent implements OnInit {
  hospice: any;
  hospiceId: number;
  hospiceLoading = false;
  hospiceView = 'locations';
  editmode: boolean;
  detailsViewOpen: boolean;
  selectedLocation: any;
  tabsList = ['locations', 'facilities', 'members', 'settings', 'contracts'];
  activeTabsList = [];

  constructor(
    private hospiceService: HospiceService,
    private route: ActivatedRoute,
    private cd: ChangeDetectorRef,
    private location: Location
  ) {
    this.initializeTabsList();
    this.route.params.subscribe((params: any) => {
      this.hospiceId = params.hospiceId;
    });
    this.route.queryParams.subscribe(params => {
      this.hospiceView = params.view || this.hospiceView || 'locations';
      this.location.replaceState(
        window.location.pathname,
        new URLSearchParams({view: this.hospiceView}).toString()
      );
    });
  }

  ngOnInit(): void {
    if (this.hospiceId) {
      this.getHospice();
    }
  }

  initializeTabsList() {
    this.tabsList.forEach(tab => {
      switch (tab) {
        case 'locations':
          if (this.hasPermission('HospiceLocation')) {
            this.activeTabsList.push(tab);
          }
          break;
        case 'facilities':
          if (this.hasPermission('Facility')) {
            this.activeTabsList.push(tab);
          }
          break;
        case 'members':
          if (this.hasPermission('Hospice')) {
            this.activeTabsList.push(tab);
          }
          break;
        case 'settings':
          if (this.hasPermission('Hospice', 'Update')) {
            this.activeTabsList.push(tab);
          }
          break;
        case 'contracts':
          if (this.hasPermission('Hospice', 'Update')) {
            this.activeTabsList.push(tab);
          }
          break;
      }
    });
  }

  getHospice() {
    this.hospiceLoading = true;
    this.hospiceService
      .getHospiceById(this.hospiceId)
      .pipe(
        finalize(() => {
          this.hospiceLoading = false;
        })
      )
      .subscribe((response: any) => {
        this.hospice = response;
      });
  }

  onTabChange(event) {
    this.hospiceView = this.activeTabsList[event.index];

    this.closeDetailsView();
    this.location.replaceState(
      window.location.pathname,
      new URLSearchParams({view: this.hospiceView}).toString()
    );
    this.cd.markForCheck();
  }

  hasPermission(entity, permission = 'Read') {
    return IsPermissionAssigned(entity, permission);
  }

  isCurrentView(view) {
    return this.hospiceView === view;
  }

  closeDetailsView() {
    this.detailsViewOpen = false;
  }

  showLocationDetails(location) {
    this.selectedLocation = location;
    this.detailsViewOpen = true;
  }

  getFormattedAddress(address) {
    if (address) {
      return `${address.addressLine1 || ''} ${address.addressLine2 || ''} ${
        address.addressLine3 || ''
      },
       ${address.city || ''}, ${address.state || ''} ${address.zipCode || 0} - ${
        address.plus4Code || 0
      }`;
    }
    return '';
  }
  recieveHospice(event) {
    this.hospice = {...this.hospice, ...event};
  }
}
