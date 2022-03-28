import {Component, OnInit} from '@angular/core';
import {HospiceService} from 'src/app/services';
import {ActivatedRoute, Router} from '@angular/router';
import {finalize} from 'rxjs/operators';
import {PaginationResponse, SieveRequest} from 'src/app/models';

@Component({
  selector: 'app-hospice-contracts',
  templateUrl: './hospice-contracts.component.html',
  styleUrls: ['./hospice-contracts.component.scss'],
})
export class HospiceContractsComponent implements OnInit {
  hospiceId: number;
  loading = false;
  contractRequest = new SieveRequest();
  contractResponse: PaginationResponse;
  contractHeaders = [
    {
      label: 'Name',
      field: 'contractName',
    },
    {
      label: 'Contract #',
      field: 'contractNumber',
    },
    {
      label: 'Per Diem Rate',
      field: 'perDiemRate',
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
    {
      actionBtn: 'View Contract Items',
      actionBtnIcon: 'pi pi-info-circle',
    },
  ];
  detailsViewOpen = false;
  selectedContract = null;

  constructor(
    private hospiceService: HospiceService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params: any) => {
      this.hospiceId = params.hospiceId;
      this.getContracts();
    });
  }

  getContracts() {
    this.loading = true;
    this.hospiceService
      .getHospiceHMS2Contracts(this.hospiceId, this.contractRequest)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((response: PaginationResponse) => {
        this.contractResponse = response;
      });
  }

  contractSelected(event) {
    this.selectedContract = event.currentRow;
    this.detailsViewOpen = true;
  }

  fetchNext({pageNm}) {
    if (!this.contractResponse || pageNm > this.contractResponse.totalPageCount) {
      return;
    }
    this.contractRequest.page = pageNm;
    this.getContracts();
  }

  closeContractDetails() {
    this.detailsViewOpen = false;
  }

  showContractItems(contract) {
    this.router.navigate([`/hospice/${contract.hospiceId}/contract/${contract.id}/items`]);
  }
}
