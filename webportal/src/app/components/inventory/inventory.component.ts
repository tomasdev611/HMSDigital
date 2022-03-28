import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {InventoryService, SitesService} from 'src/app/services';
import {finalize} from 'rxjs/operators';
import {deepCloneObject, IsPermissionAssigned} from 'src/app/utils';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {ActivatedRoute, Router} from '@angular/router';
import {Subscription} from 'rxjs';
import {NavbarSearchService} from 'src/app/services/navbar-search.service';
import {ItemImageBaseResponse} from '../../models/model.product-image';
import {TableVirtualScrollComponent} from 'src/app/common';
import {ItemComponent, ItemCategoriesComponent} from 'src/app/components/items';
import {Location} from '@angular/common';

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.scss'],
})
export class InventoryComponent implements OnInit, OnDestroy {
  @ViewChild('inventoryTable', {static: false})
  inventoryTable: TableVirtualScrollComponent;

  @ViewChild('items', {static: false})
  items: ItemComponent;

  @ViewChild('categories', {static: false})
  categories: ItemCategoriesComponent;

  loading = false;
  inventory;
  inventoryList;
  inventoryResponse: PaginationResponse;
  inventoryRequest = new SieveRequest();
  tabView = 'inventory';

  imageItemUrlList: ItemImageBaseResponse[] = [];
  imageItemActiveIndex: number;
  imageItemDisplayFullScreen: boolean;
  imageItemDisplayNavigator: boolean;

  headers: any = [
    {label: 'Item Name', field: 'item.name'},
    {label: 'Serial Number', field: 'serialNumber'},
    {label: 'Asset Tag', field: 'assetTagNumber'},
    {label: 'Count', field: 'count'},
    {label: 'Status', field: 'status'},
    {label: 'Current Location', field: 'locationLabel'},
  ];
  searchQuery = '';
  subscriptions: Subscription[] = [];
  selectedItem: any;
  selectedAssetTag: string;
  selectedSerialNumber: string;
  detailFlyoutOpen: boolean;
  itemSearchQuery = '';
  categorySearchQuery = '';
  totalRecordCount = 0;
  isConfigDialogOpen = false;
  equipmentSettingsConfig: any = null;

  constructor(
    private inventoryService: InventoryService,
    private router: Router,
    private route: ActivatedRoute,
    private siteService: SitesService,
    private navbarSearchService: NavbarSearchService,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.tabView = params.view || this.tabView || 'inventory';
      this.location.replaceState(
        window.location.pathname,
        new URLSearchParams({view: this.tabView}).toString()
      );
    });
    this.loading = true;
    if (IsPermissionAssigned('Inventory', 'Update')) {
      this.headers.push({
        label: '',
        field: '',
        class: 'sm',
        linkParams: 'id',
        editBtn: 'Edit Inventory Item',
        editBtnIcon: 'pi pi-pencil',
        editBtnLink: 'edit',
        actionBtn: 'Transfer Logs',
        actionBtnIcon: 'pi pi-clock',
      });
    }
    this.getInventoryList();
    this.subscriptions.push(
      this.navbarSearchService.search.subscribe(text => this.searchInventory(text))
    );

    this.imageItemActiveIndex = 0;
    this.imageItemDisplayFullScreen = false;
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  onTabChange({index}) {
    this.selectedItem = null;
    this.detailFlyoutOpen = false;
    this.searchQuery = '';
    this.categorySearchQuery = '';
    this.itemSearchQuery = '';
    this.inventoryRequest.filters = '';
    switch (index) {
      case 0:
        this.tabView = 'inventory';
        this.dataTablesReset();
        this.getInventoryList();
        break;
      case 1:
        this.tabView = 'items';
        break;
      case 2:
        this.tabView = 'categories';
        break;
    }
    this.location.replaceState(
      window.location.pathname,
      new URLSearchParams({view: this.tabView}).toString()
    );
  }

  fetchNext({pageNum}) {
    if (!this.inventoryResponse || pageNum >= this.inventoryResponse.totalPageCount) {
      return;
    }
    this.inventoryRequest.page = pageNum;
    this.getInventoryList();
  }

  getInventoryList() {
    this.loading = true;
    (!this.searchQuery
      ? this.inventoryService.getInvetoryList(this.inventoryRequest)
      : this.inventoryService.searchInventory({
          ...this.inventoryRequest,
          searchQuery: this.searchQuery,
        })
    )
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((res: any) => {
        this.inventoryResponse = res;
        this.totalRecordCount = this.inventoryResponse.totalRecordCount;
        this.getLocationInfo(res);
      });
  }

  searchInventory(searchQuery) {
    if (this.isCurrentView('inventory')) {
      this.dataTablesReset();
      this.inventoryRequest.page = 1;
      this.searchQuery = searchQuery;
      this.getInventoryList();
    }
    if (this.isCurrentView('items')) {
      if (this.items) {
        this.items.dataTablesReset();
      }
      this.itemSearchQuery = searchQuery;
    }
    if (this.isCurrentView('categories')) {
      if (this.categories) {
        this.categories.dataTablesReset();
      }
      this.categorySearchQuery = searchQuery;
    }
  }

  hasPermission(entity, permission = 'Read') {
    return IsPermissionAssigned(entity, permission);
  }

  isCurrentView(entity) {
    return this.tabView === entity;
  }

  redirectToLogs({object}) {
    this.router.navigate([`logs/${object?.id}`], {relativeTo: this.route});
  }

  getLocationInfo(res) {
    const siteIds = res.records.flatMap(site =>
      site.currentLocationId ? [site.currentLocationId] : []
    );
    // site request
    if (siteIds.length) {
      this.loading = true;
      this.siteService
        .searchSites({searchQuery: ''})
        .pipe(finalize(() => (this.loading = false)))
        .subscribe((siteInfo: []) => {
          this.showLocationDetails(siteInfo);
        });
    }
  }

  showLocationDetails(siteInfo) {
    this.inventoryResponse.records = this.inventoryResponse.records.map(invItem => {
      let info;
      info = siteInfo?.records?.find(s => s.id === invItem.currentLocationId);
      if (info) {
        invItem.locationLabel = `${info.locationType} (${info.name})`;
      }
      return invItem;
    });
  }

  showItemDetails({currentRow}) {
    this.selectedItem = currentRow.item;
    this.selectedItem.category = this.selectedItem.categories.map(c => c.name).join(',');
    this.selectedItem.subCategory = this.selectedItem.subCategories.map(sc => sc.name).join(',');
    this.detailFlyoutOpen = true;

    this.imageItemUrlList = currentRow.images ? currentRow.images : this.imageItemUrlList || [];
    this.selectedAssetTag = currentRow.assetTagNumber;
    this.selectedSerialNumber = currentRow.serialNumber;
  }

  closeFlyout() {
    this.selectedItem = null;
    this.detailFlyoutOpen = false;
  }

  imageItemClick(index: number): void {
    this.imageItemActiveIndex = index;
    this.imageItemDisplayFullScreen = true;
    this.imageItemDisplayNavigator = this.imageItemUrlList.length > 1;
  }

  filterInventories(filterString) {
    this.inventoryRequest.filters = filterString;
    this.inventoryRequest.page = 1;
    this.getInventoryList();
  }

  setTotalRecordCount({totalCount}) {
    this.totalRecordCount = totalCount;
  }

  dataTablesReset() {
    if (this.inventoryTable) {
      this.inventoryTable.reset();
    }
    this.inventoryRequest.page = 1;
  }

  showEquipmentSettingsConfigurationDialog() {
    this.equipmentSettingsConfig = deepCloneObject(this.selectedItem.equipmentSettingsConfig);
    this.isConfigDialogOpen = true;
  }

  closeEquipmentSettingsConfigurationDialog() {
    this.equipmentSettingsConfig = null;
    this.isConfigDialogOpen = false;
  }
}
