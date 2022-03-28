import {Component, OnInit} from '@angular/core';
import {HospiceService} from 'src/app/services';
import {ActivatedRoute} from '@angular/router';
import {finalize} from 'rxjs/operators';
import {PaginationResponse, SieveRequest} from 'src/app/models';

@Component({
  selector: 'app-hospice-zab-subscription',
  templateUrl: './hospice-zab-subscription.component.html',
  styleUrls: ['./hospice-zab-subscription.component.scss'],
})
export class HospiceZabSubscriptionComponent implements OnInit {
  hospiceId: number;
  loading = false;
  subscriptionRequest = new SieveRequest();
  subscriptionResponse: PaginationResponse;
  subscriptionHeaders = [
    {
      label: 'Name',
      field: 'name',
    },
    {
      label: 'Bill To Customer',
      field: 'billToCustomer',
    },
    {
      label: 'Bill To Entity',
      field: 'billToEntity',
    },
    {
      label: 'Charge Schedule',
      field: 'chargeSchedule',
    },
    {
      label: 'Inactive',
      field: 'isInactive',
      fieldType: 'BoolToYesNo',
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
  selectedSubscription = null;

  constructor(private hospiceService: HospiceService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.params.subscribe((params: any) => {
      this.hospiceId = params.hospiceId;
      this.getSubscriptions();
    });
  }

  getSubscriptions() {
    this.loading = true;
    this.hospiceService
      .getHospiceZabSubscriptions(this.hospiceId, this.subscriptionRequest)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((response: PaginationResponse) => {
        this.subscriptionResponse = response;
      });
  }

  subscriptionSelected(event) {
    this.selectedSubscription = event.currentRow;
    this.detailsViewOpen = true;
  }

  fetchNext({pageNm}) {
    if (!this.subscriptionResponse || pageNm > this.subscriptionResponse.totalPageCount) {
      return;
    }
    this.subscriptionRequest.page = pageNm;
    this.getSubscriptions();
  }

  closeSubscriptionDetails() {
    this.detailsViewOpen = false;
  }
}
