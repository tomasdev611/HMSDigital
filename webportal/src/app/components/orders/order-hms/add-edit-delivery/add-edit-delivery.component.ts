import {Location} from '@angular/common';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {Subject} from 'rxjs';
import {debounceTime, distinctUntilChanged, finalize} from 'rxjs/operators';
import {hms} from 'src/app/constants';
import {SieveOperators} from 'src/app/enums';
import {SieveRequest} from 'src/app/models';
import {
  ItemCategoriesService,
  OrderHeadersService,
  ProductCatalogService,
  StorageService,
} from 'src/app/services';
import {
  buildFilterString,
  checkEqualArray,
  deepCloneObject,
  encode,
  getFormattedPhoneNumber,
  getUniqArray,
} from 'src/app/utils';
import {AddOnsEquipmentSettingsComponent} from '../add-ons-equipment-settings/add-ons-equipment-settings.component';

@Component({
  selector: 'app-add-edit-delivery',
  templateUrl: './add-edit-delivery.component.html',
  styleUrls: ['./add-edit-delivery.component.scss'],
})
export class AddEditDeliveryComponent implements OnInit {
  formatPhoneNumber = getFormattedPhoneNumber;
  orderHeader: any;
  fulfilledItems = [];
  orderId: number;
  activeStep = 'productCatalog';
  cartItems = [];
  storeKey = '';
  tmpSessionKey = '';
  @Input() patientUniqueId = '';
  @Input() editmode = false;
  @Input() hospiceLocations = [];
  @Input() patient: any;
  @Input() nurses = [];
  @Input() pickupTimeOptions = [];
  @Input() patientAddresses;
  @Input() patientInventories: any;

  categories: any;
  selectedCategory: any;
  selectedSubCategory: any;
  searchKey = '';
  itemRequest = new SieveRequest();
  products: any;
  productsLoading = false;
  searchChanged: Subject<string> = new Subject<string>();
  deliveryDetails: any;
  selectedNurse: any;
  backupFormState = null;
  selectedProduct: any;

  @ViewChild('dv', {static: true}) dataview: any;
  @ViewChild('addOnsEquipments')
  addOnsEquipmentsModal: AddOnsEquipmentSettingsComponent;

  constructor(
    private itemCategoriesService: ItemCategoriesService,
    private productCatalogService: ProductCatalogService,
    private router: Router,
    private orderHeaderService: OrderHeadersService,
    private route: ActivatedRoute,
    private storageService: StorageService,
    private location: Location
  ) {
    this.handleSearchEvent();

    this.route.params.subscribe((params: any) => {
      this.patientUniqueId = params.patientUniqueId;
      this.orderId = params.orderId;
    });
  }

  ngOnInit(): void {
    this.prepareStoreKeys();
    this.getDefaultItemRequest();
    this.getItemCategories();
    this.getOrderDetails();
    this.setQueryParams();
  }

  handleSearchEvent() {
    this.searchChanged
      .pipe(
        debounceTime(500), // wait 500ms after the last event before emitting last event
        distinctUntilChanged() // only emit if value is different from previous value
      )
      .subscribe((model: any) => {
        this.getDefaultItemRequest();
        if (this.searchKey) {
          this.selectedCategory = null;
          this.selectedSubCategory = null;
          this.getItems(true);
        } else {
          this.getItems();
        }
      });
  }

  setQueryParams() {
    this.route.queryParams.subscribe(params => {
      this.activeStep = params.view || this.activeStep || 'productCatalog';
      this.location.replaceState(
        window.location.pathname,
        new URLSearchParams({view: this.activeStep}).toString()
      );
    });
  }

  prepareStoreKeys() {
    if (this.patient?.uniqueId) {
      const key = `delivery_${this.editmode ? 'edit' : 'create'}_${this.patient?.id}_${
        this.patient?.hospiceLocationId ?? ''
      }`;
      this.storeKey = encode(key);
      this.tmpSessionKey = encode(key + '_tmpSession');
      this.patchOldFormState();
      this.getItems();
      if (!this.editmode) {
        this.getStoreCart();
      }

      this.getCartItemPrices();
    }
  }

  patchOldFormState() {
    const storedDetails = this.getFormState();
    if (storedDetails && storedDetails.deliveryDetails) {
      this.deliveryDetails = storedDetails.deliveryDetails;
    }
  }

  getFormState() {
    if (this.editmode) {
      return null;
    }
    if (!this.backupFormState) {
      const dataString = this.storageService.get(this.tmpSessionKey, 'session');
      this.storageService.remove(this.tmpSessionKey, 'session');
      if (dataString) {
        this.backupFormState = JSON.parse(dataString);
      }
    }
    return this.backupFormState;
  }

  getItemCategories() {
    this.itemCategoriesService.getItemCategories().subscribe((response: any) => {
      this.categories = response;
    });
  }
  categoryChange(event) {
    this.selectedSubCategory = null;
    this.searchKey = '';
    this.getDefaultItemRequest();
    this.getItems();
  }
  subCategoryChange(event) {
    this.getDefaultItemRequest();
    this.getItems();
  }

  getDefaultItemRequest() {
    this.itemRequest = new SieveRequest();
    this.itemRequest.pageSize = 24;
    if (this.dataview) {
      // paginator's current page index changd to default ie. 1
      this.dataview.paginate({
        first: 0,
        rows: this.itemRequest.pageSize,
      });
    }
  }

  searchItems(text: string) {
    this.searchChanged.next(text);
  }

  clearSearch() {
    this.searchKey = '';
    this.searchItems('');
  }

  getItems(filterViaName = false) {
    if (!this.patient?.uniqueId) {
      return;
    }
    this.productsLoading = true;
    const filters = [];
    if (filterViaName) {
      filters.push({
        field: 'itemName',
        operator: SieveOperators.Contains,
        value: [this.searchKey],
      });
    } else {
      if (this.selectedCategory?.id) {
        filters.push({
          field: 'categoryId',
          operator: SieveOperators.Equals,
          value: [this.selectedCategory.id],
        });
      }
      if (this.selectedSubCategory?.id) {
        filters.push({
          field: 'subCategoryId',
          operator: SieveOperators.Equals,
          value: [this.selectedSubCategory.id],
        });
      }
    }
    if (filters.length) {
      this.itemRequest.filters = buildFilterString(filters);
    }
    this.productCatalogService
      .getProducts(this.patient.hospiceId, this.patient.hospiceLocationId, this.itemRequest)
      .pipe(
        finalize(() => {
          this.productsLoading = false;
        })
      )
      .subscribe((response: any) => {
        response.records = response.records.map((x: any) => {
          const count = this.cartItems
            .filter(item => item?.item?.id === x.item.id)
            .reduce((a: any, b: any) => {
              return a + b.count;
            }, 0);
          x.count = count;
          x.id =
            this.orderHeader?.orderLineItems.find((oli: any) => oli.itemId === x.item.id)?.id ??
            null;
          x.equipmentSettings = x.equipmentSettingFields.map((setting: any) => ({
            name: setting,
            value: null,
          }));
          return x;
        });
        this.products = response;
      });
  }

  pageChange(event) {
    this.itemRequest.page = event.first / event.rows + 1;
    this.getItems();
  }

  proceedToDeliveryDetails() {
    if (!this.cartItems.length) {
      return;
    }
    this.changeActiveStep('deliveryDetails');
  }

  cancelOrder() {
    this.storageService.remove(this.storeKey);
    this.storageService.remove(this.tmpSessionKey, 'session');
    this.router.navigate([hms.defaultRoute]);
  }
  updateCartReceiver(event) {
    const {action, product} = event;
    if (action && product) {
      this.updateCart(action, product, false);
    }
  }

  checkUnfulfilledEquipments(equipmentSettings) {
    return equipmentSettings.some(x => !x.value);
  }

  updateCart(action: string, item: any, shouldCheckAddOns = true) {
    const product = deepCloneObject(item);
    if (
      action === 'increment' &&
      ((product.equipmentSettings.length &&
        this.checkUnfulfilledEquipments(product.equipmentSettings)) ||
        (shouldCheckAddOns && product?.item?.addOnGroups.length))
    ) {
      if (product?.item?.addOnGroups.length) {
        product.item.addOnGroups = product.item.addOnGroups.map(x => {
          x.addOnGroupProducts.push({itemName: 'None of the above', itemId: 0});
          x.selected = null;
          return x;
        });
      }
      this.selectedProduct = product;
      this.addOnsEquipmentsModal.show();
      return;
    }
    const index = this.cartItems.findIndex(
      x =>
        x.item.id === product.item.id &&
        (product.equipmentSettings.length === 0 ||
          checkEqualArray(product.equipmentSettings, x.equipmentSettings))
    );
    const prodIndex = this.products.records.findIndex(x => x.item.id === product.item.id);
    switch (action) {
      case 'increment':
        if (index > -1) {
          this.cartItems[index].count = this.cartItems[index].count + 1;
        } else {
          product.count = 1;
          this.cartItems = [...this.cartItems, product];
        }
        this.updateCartStore(prodIndex, action);
        break;
      case 'decrement':
        if (product.count === 0) {
          return;
        }
        if (index > -1) {
          const count = this.cartItems[index].count - 1;
          if (count === 0) {
            this.cartItems.splice(index, 1);
          } else {
            this.cartItems[index].count = count;
          }
        }
        this.updateCartStore(prodIndex, action);
        break;
      case 'remove':
        this.cartItems.splice(index, 1);
        this.updateCartStore(prodIndex, action, product.count);
        break;
      default:
        return 0;
    }
  }

  updateCartStore(productIndex: number, action: string, count?: number) {
    this.products.records[productIndex].count =
      action === 'remove'
        ? count
          ? this.products.records[productIndex].count - count
          : 0
        : action === 'increment'
        ? this.products.records[productIndex].count + 1
        : this.products.records[productIndex].count - 1;
    if (!this.editmode) {
      if (this.cartItems.length === 0) {
        this.storageService.remove(this.storeKey);
      } else {
        this.storageService.set(this.storeKey, JSON.stringify(this.cartItems));
      }
    }
  }

  getStoreCart() {
    const carts = this.storageService.get(this.storeKey);
    this.cartItems = carts ? JSON.parse(carts) : [];
  }

  changeStepReceiver(event) {
    if (event.step) {
      this.changeActiveStep(event.step);
    }
    if (event?.deliveryDetails) {
      this.deliveryDetails = event?.deliveryDetails;
    }
    this.selectedNurse = event?.nurse ?? null;
  }

  changeActiveStep(step: string) {
    this.activeStep = step;
    this.location.replaceState(
      window.location.pathname,
      new URLSearchParams({view: this.activeStep}).toString()
    );
  }

  getOrderDetails() {
    if (!(this.editmode && this.orderId)) {
      return;
    }
    this.orderHeaderService
      .getOrderHeaderById(this.orderId, true)
      .subscribe((orderHeaderData: any) => {
        this.orderHeader = orderHeaderData;
        this.cartItems = this.orderHeader.orderLineItems.map(x => {
          return {
            id: x.id ?? null,
            count: x.itemCount,
            item: x.item,
            itemImageUrls: [],
            rate: x.rate ?? null,
            equipmentSettings: x.equipmentSettings
              ? this.getEquipmentSettings(x.equipmentSettings)
              : [],
          };
        });
        this.getCartItemPrices();
        this.fulfilledItems = this.orderHeader.orderFulfillmentLineItems;
        setTimeout(() => {
          this.proceedToDeliveryDetails();
        }, 300);
      });
  }

  getEquipmentSettings(settings) {
    const equipmentSettings = settings.map((eq: any) => {
      const keys = Object.keys(eq);
      return {
        name: keys[0],
        value: eq[keys[0]],
      };
    });
    return equipmentSettings;
  }

  checkFormValidity() {
    return !this.cartItems.length
      ? `<span>Required fields are not complete</span><ul><li>Order Items</li></ul>`
      : '';
  }
  updateProductReceiver(event) {
    if (event?.product) {
      this.updateCart('increment', event.product, false);
      if (event.product?.item?.addOnGroups?.length) {
        event.product.item.addOnGroups.forEach(x => {
          if (x?.selected?.length) {
            x.selected.map((addOn: any) => {
              if (addOn.itemId === 0) {
                return;
              }
              addOn.item = {
                name: addOn.itemName,
                id: addOn.itemId,
              };
              addOn.id = null;
              addOn.equipmentSettings = [];
              this.updateCart('increment', addOn, false);
            });
          }
        });
      }
    }
  }

  getFulfilledItemsForLineItem(orderLineItemId) {
    return this.fulfilledItems?.filter(fi => fi.orderLineItemId === orderLineItemId);
  }

  getFulfilledLineItemsCount(orderLineItemId) {
    return this.getFulfilledItemsForLineItem(orderLineItemId).reduce((a: any, b: any) => {
      return a + b.quantity;
    }, 0);
  }

  checkDisabled(product: any) {
    const fulfilledItems = this.getFulfilledLineItemsCount(product.id) || 0;
    return product.equipmentSettings.length ||
      product.count === 0 ||
      (product.count > 0 && product.count <= fulfilledItems)
      ? true
      : false;
  }

  getCartItemPrices() {
    if (!this.patient || !this.orderHeader) {
      return;
    }

    const itemIds = this.cartItems.map(item => item.item.id);
    const filters = [
      {
        field: 'itemId',
        operator: SieveOperators.Equals,
        value: getUniqArray(itemIds),
      },
    ];

    const catalogRequest = new SieveRequest();
    catalogRequest.filters = buildFilterString(filters);
    catalogRequest.pageSize = itemIds.length;

    this.productCatalogService
      .getProducts(this.patient.hospiceId, this.patient.hospiceLocationId, catalogRequest)
      .subscribe((response: any) => {
        response?.records?.forEach(record => {
          const replacementItemIndex = this.cartItems.findIndex(
            ci => ci.item.id === record.item.id
          );
          this.cartItems[replacementItemIndex].rate = record.rate;
        });
      });
  }

  shouldShowDeliveryDetailsView() {
    const shouldShow = this.patient && (this.editmode ? this.orderHeader : true);
    return shouldShow;
  }
}
