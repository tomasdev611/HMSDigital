import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import {SieveOperators} from 'src/app/enums';
import {ItemsService} from 'src/app/services';
import {buildFilterString, getUniqArray} from 'src/app/utils';

@Component({
  selector: 'app-dispatch-list',
  templateUrl: './dispatch-list.component.html',
  styleUrls: ['./dispatch-list.component.scss'],
})
export class DispatchListComponent implements OnInit {
  @Input() loading = false;
  orderList = [];
  @Input() activeView = '';
  @Input() showPaginator = false;
  @Input() paginate = true;
  @Input() totalPage: number;
  @Input() markedOrderId;
  @Output() fetchPrev = new EventEmitter<any>();
  @Output() fetchNext = new EventEmitter<any>();
  @Output() orderSelected = new EventEmitter<any>();
  @Input() pageNumber: number;
  @Input() set orders(orders: [any]) {
    this.orderList = orders;
    if (this.orderList?.length) {
      this.getImages();
    }
  }

  constructor(private itemService: ItemsService) {}

  ngOnInit(): void {}

  getImages() {
    setTimeout(() => {
      if (this.activeView === 'orderView') {
        const itemIds = this.orderList.flatMap(o => {
          const orderItemIds = o?.orderLineItems.flatMap(ol => {
            return ol.itemId ? [ol.itemId] : [];
          });
          return orderItemIds ? orderItemIds : [];
        });
        if (itemIds.length) {
          const uniqItemIds = getUniqArray(itemIds);
          const filters = [
            {
              field: 'itemId',
              operator: SieveOperators.Equals,
              value: uniqItemIds,
            },
          ];
          const request = {
            filters: buildFilterString(filters),
          };

          this.itemService.getAllItemImages(request).subscribe((imageResponses: any) => {
            if (imageResponses?.records?.length) {
              this.orderList.map(o => {
                o?.orderLineItems?.map(ol => {
                  if (ol.itemId) {
                    const image = imageResponses.records.find(img => img.itemId === ol.itemId);
                    if (image) {
                      ol.image = {...image};
                    }
                  }
                  return ol;
                });
                return o;
              });
            }
          });
        }
      }
    }, 300);
  }
  selectOrder(event) {
    this.orderSelected.emit(event);
  }
  getPrev() {
    this.fetchPrev.emit();
  }
  getNext() {
    this.fetchNext.emit();
  }
}
