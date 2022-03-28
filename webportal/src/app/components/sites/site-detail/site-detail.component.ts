import {Component, OnInit} from '@angular/core';
import {SieveRequest, SiteView} from 'src/app/models';
import {SitesService} from 'src/app/services';
import {ActivatedRoute} from '@angular/router';
import {IsPermissionAssigned} from 'src/app/utils';
import {PhoneNumberTypes} from 'src/app/enums';
import {Location} from '@angular/common';

@Component({
  selector: 'app-site-detail',
  templateUrl: './site-detail.component.html',
  styleUrls: ['./site-detail.component.scss'],
})
export class SiteDetailComponent implements OnInit {
  siteId: number;
  zipCodes = [];
  siteView: string;
  siteResponse: SiteView;
  itemIds: number[] = [];

  constructor(
    private siteService: SitesService,
    private route: ActivatedRoute,
    private location: Location
  ) {
    const {paramMap} = this.route.snapshot;

    this.siteId = Number(paramMap.get('siteId'));
    this.getSiteDetail();

    this.route.queryParams.subscribe(params => {
      this.siteView = params.view || this.siteView || 'site';
      this.location.replaceState(
        window.location.pathname,
        new URLSearchParams({view: this.siteView}).toString()
      );
    });
  }

  ngOnInit(): void {}

  tabChanged(event) {
    switch (event.index) {
      case 0:
        this.siteView = 'site';
        break;
      case 1:
        this.siteView = 'members';
        break;
      case 2:
        this.siteView = 'serviceareas';
        break;
      case 3:
        this.siteView = 'inventory';
        break;
      default:
        this.siteView = 'site';
        break;
    }
    this.location.replaceState(
      window.location.pathname,
      new URLSearchParams({view: this.siteView}).toString()
    );
  }

  getSiteDetail() {
    this.siteService.getSiteById(this.siteId).subscribe((response: SiteView) => {
      this.siteResponse = {...response, isActive: !response.isDisable};
      response.sitePhoneNumber?.map(x => {
        if (x.phoneNumber.numberTypeId === PhoneNumberTypes.Fax) {
          this.siteResponse.fax = x.phoneNumber.number;
        } else {
          this.siteResponse.phoneNumber = x.phoneNumber.number;
        }
      });
      if (this.siteResponse.parentNetSuiteLocationId) {
        this.mapParentSite();
      }
    });
  }

  mapParentSite() {
    const sieveReq = new SieveRequest();
    this.siteService.searchSites({...sieveReq, searchQuery: ''}).subscribe((responses: any) => {
      this.siteResponse.parent = responses.records.find(
        s => s.netSuiteLocationId === this.siteResponse.parentNetSuiteLocationId
      );
    });
  }

  canView(tabName) {
    return IsPermissionAssigned(tabName, 'Read');
  }

  inventorySelected({checked, currentRow}) {
    checked
      ? this.itemIds.push(currentRow.data.item.id)
      : this.itemIds.splice(this.itemIds.indexOf(currentRow.data.item.id), 1);
  }

  getTransferRequestParams() {
    return {
      itemIds: JSON.stringify(this.itemIds),
    };
  }

  isCurrentView(view) {
    return this.siteView === view;
  }
}
