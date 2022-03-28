import {Component, OnInit} from '@angular/core';
import {HospiceService} from 'src/app/services';
import {ActivatedRoute} from '@angular/router';
import {finalize} from 'rxjs/operators';
import {PaginationResponse, SieveRequest} from 'src/app/models';
@Component({
  selector: 'app-contract-items',
  templateUrl: './contract-items.component.html',
  styleUrls: ['./contract-items.component.scss'],
})
export class ContractItemsComponent implements OnInit {
  loading = false;
  contractItemRequest = new SieveRequest();
  contractItemResponse: PaginationResponse;
  contractItemHeaders = [
    {
      label: 'Item Name',
      field: 'itemName',
    },
    {
      label: 'Is Per Diem',
      field: 'isPerDiem',
    },
    {
      label: 'Rental Price',
      field: 'rentalPrice',
    },
    {
      label: 'Sale Price',
      field: 'salePrice',
    },
    {
      label: 'Show on Order Screen',
      field: 'showOnOrderScreen',
    },
  ];
  detailsViewOpen = false;
  selectedItem = null;
  contractId: number;

  constructor(private hospiceService: HospiceService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.params.subscribe((params: any) => {
      this.contractId = params.contractId;
      this.getContractItems();
    });
  }

  getContractItems() {
    this.loading = true;
    this.hospiceService
      .getHospiceHMS2ContractItems(this.contractId, this.contractItemRequest)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((response: PaginationResponse) => {
        this.contractItemResponse = response;
        this.contractItemResponse?.records?.forEach(ci => {
          ci.itemName = ci.item?.name;
          ci.showOnOrderScreen = ci.showOnOrderScreen ? 'Yes' : 'No';
          ci.isPerDiem = ci.isPerDiem ? 'Yes' : 'No';
          ci.rentalPrice = `$ ${ci.rentalPrice}`;
          ci.salePrice = `$ ${ci.salePrice}`;
        });
      });
  }

  itemSelected(event) {
    this.selectedItem = event.currentRow;
    this.detailsViewOpen = true;
  }

  fetchNext({pageNum}) {
    if (!this.contractItemResponse || pageNum >= this.contractItemResponse.totalPageCount) {
      return;
    }
    this.contractItemRequest.page = pageNum;
    this.getContractItems();
  }

  closeItemDetails() {
    this.detailsViewOpen = false;
  }
}
