import {Component, OnInit} from '@angular/core';
import {HospiceService} from 'src/app/services';
import {ActivatedRoute} from '@angular/router';
import {finalize} from 'rxjs/operators';
import {PaginationResponse, SieveRequest} from 'src/app/models';

@Component({
  selector: 'app-zab-subscription-items',
  templateUrl: './zab-subscription-items.component.html',
  styleUrls: ['./zab-subscription-items.component.scss'],
})
export class ZabSubscriptionItemsComponent implements OnInit {
  loading = false;
  subscriptionRequest = new SieveRequest();
  subscriptionItemResponse: PaginationResponse;
  subscriptionHeaders = [
    {
      label: 'Name',
      field: 'name',
    },
    {
      label: 'NetSuite Subscription ItemId',
      field: 'netSuiteSubscriptionItemId',
    },
    {
      label: 'Rate Type',
      field: 'rateType',
    },
    {
      label: 'Rate Plan',
      field: 'ratePlan',
    },
    {
      label: 'Inactive',
      field: 'isInactive',
    },
    {
      label: 'Start Date',
      field: 'startDate',
      fieldType: 'Date',
    },
    {
      label: 'End Date',
      field: 'endDate',
      fieldType: 'Date',
    },
  ];
  detailsViewOpen = false;
  selectedItem = null;
  subscriptionId: number;

  constructor(private hospiceService: HospiceService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.params.subscribe((params: any) => {
      this.subscriptionId = params.subscriptionId;
      this.getSubscriptionItems();
    });
  }

  getSubscriptionItems() {
    this.loading = true;
    this.hospiceService
      .getHospiceZabSubscriptionItems(this.subscriptionId, this.subscriptionRequest)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((response: PaginationResponse) => {
        this.subscriptionItemResponse = response;
      });
  }

  itemSelected(event) {
    this.selectedItem = event.currentRow;
    this.detailsViewOpen = true;
  }

  fetchNext({pageNum}) {
    if (!this.subscriptionItemResponse || pageNum >= this.subscriptionItemResponse.totalPageCount) {
      return;
    }
    this.subscriptionRequest.page = pageNum;
    this.getSubscriptionItems();
  }

  closeItemDetails() {
    this.detailsViewOpen = false;
  }
}
