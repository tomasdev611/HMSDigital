import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {finalize} from 'rxjs/operators';
import {SieveOperators} from 'src/app/enums';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {HospiceService} from 'src/app/services';
import {buildFilterString} from 'src/app/utils';

@Component({
  selector: 'app-zab-subscription-contracts',
  templateUrl: './zab-subscription-contracts.component.html',
  styleUrls: ['./zab-subscription-contracts.component.scss'],
})
export class ZabSubscriptionContractsComponent implements OnInit {
  loading = false;
  subscriptionContractRequest = new SieveRequest();
  subscriptionContractResponse: PaginationResponse;
  subscriptionContractHeaders = [
    {
      label: 'Item Name',
      field: 'item.name',
    },
    {
      label: 'Rate',
      field: 'rate',
    },
    {
      label: 'Start Date',
      field: 'effectiveEndDate',
      fieldType: 'Date',
    },
    {
      label: 'End Date',
      field: 'effectiveStartDate',
      fieldType: 'Date',
    },
    {
      label: 'Risk Cap Eligible',
      field: 'riskCapEligible',
      fieldType: 'BoolToYesNo',
    },
    {
      label: 'Show On Order Screen',
      field: 'showOnOrderScreen',
      fieldType: 'BoolToYesNo',
    },
  ];
  detailsViewOpen = false;
  selectedItem = null;
  subscriptionId: number;

  constructor(private hospiceService: HospiceService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.params.subscribe((params: any) => {
      this.subscriptionId = params.subscriptionId;
      this.getSubscriptionContracts();
    });
  }

  getSubscriptionContracts() {
    this.loading = true;
    const filters = [
      {
        field: 'netSuiteSubscriptionId',
        value: [this.subscriptionId],
        operator: SieveOperators.Equals,
      },
    ];
    this.subscriptionContractRequest.filters = buildFilterString(filters);
    this.hospiceService
      .getHospiceZabSubscriptionContracts(this.subscriptionContractRequest)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((response: PaginationResponse) => {
        this.subscriptionContractResponse = response;
      });
  }

  fetchNext({pageNum}) {
    if (
      !this.subscriptionContractResponse ||
      pageNum >= this.subscriptionContractResponse.totalPageCount
    ) {
      return;
    }
    this.subscriptionContractRequest.page = pageNum;
    this.getSubscriptionContracts();
  }
}
