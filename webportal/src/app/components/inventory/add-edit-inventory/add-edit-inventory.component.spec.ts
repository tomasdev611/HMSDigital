import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {AddEditInventoryComponent} from './add-edit-inventory.component';
import {ReactiveFormsModule, FormsModule} from '@angular/forms';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthModule} from 'angular-oauth2-oidc';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {MessageService, ConfirmationService} from 'primeng/api';
import {AutoCompleteModule} from 'primeng/autocomplete';
import {InputNumberModule} from 'primeng/inputnumber';
import {DropdownModule} from 'primeng/dropdown';
import {TransferState} from '@angular/platform-browser';
import {InventoryService, ToastService, ItemsService, SitesService} from 'src/app/services';
import {BehaviorSubject} from 'rxjs';
import {PaginationResponse} from 'src/app/models';

describe('AddEditInventoryComponent', () => {
  let component: AddEditInventoryComponent;
  let fixture: ComponentFixture<AddEditInventoryComponent>;
  let inventoryService: any;
  let toastService: any;
  let itemService: any;
  let siteService: any;

  beforeEach(
    waitForAsync(() => {
      const inventoryServiceStub = jasmine.createSpyObj('InventoryService', [
        'getInventoryItemById',
        'updateInventoryItem',
        'getInventoryItemDetails',
        'createInventoryItem',
        'deleteInventoryItem',
      ]);
      inventoryServiceStub.getInventoryItemById.and.returnValue(
        new BehaviorSubject(invenotryItemResponse)
      );
      inventoryServiceStub.updateInventoryItem.and.returnValue(
        new BehaviorSubject(invenotryItemResponse)
      );
      inventoryServiceStub.getInventoryItemDetails.and.returnValue(
        new BehaviorSubject(inventoryItemDetails)
      );
      inventoryServiceStub.createInventoryItem.and.returnValue(
        new BehaviorSubject(inventoryItemDetails)
      );
      inventoryServiceStub.deleteInventoryItem.and.returnValue(new BehaviorSubject(null));

      const toastServiceStub = jasmine.createSpyObj('ToastService', ['showError', 'showSuccess']);
      toastServiceStub.showError.and.callThrough();
      toastServiceStub.showSuccess.and.callThrough();

      const itemServiceStub = jasmine.createSpyObj('ItemsService', ['searchItems']);
      itemServiceStub.searchItems.and.returnValue(
        new BehaviorSubject<PaginationResponse>(itemsResponse)
      );

      const siteSerivceStub = jasmine.createSpyObj('SitesService', ['searchSites']);
      siteSerivceStub.searchSites.and.returnValue(
        new BehaviorSubject<PaginationResponse>(siteResponse)
      );

      TestBed.configureTestingModule({
        declarations: [AddEditInventoryComponent],
        imports: [
          ReactiveFormsModule,
          RouterTestingModule.withRoutes([{path: 'inventory', redirectTo: ''}]),
          HttpClientTestingModule,
          AutoCompleteModule,
          InputNumberModule,
          DropdownModule,
          FormsModule,
          OAuthModule.forRoot(),
        ],
        providers: [
          {
            provide: InventoryService,
            useValue: inventoryServiceStub,
          },
          {
            provide: ItemsService,
            useValue: itemServiceStub,
          },
          {
            provide: ToastService,
            useValue: toastServiceStub,
          },
          {
            provide: SitesService,
            useValue: siteSerivceStub,
          },
          MessageService,
          ConfirmationService,
          TransferState,
        ],
      }).compileComponents();
      inventoryService = TestBed.inject(InventoryService);
      itemService = TestBed.inject(ItemsService);
      toastService = TestBed.inject(ToastService);
      siteService = TestBed.inject(SitesService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEditInventoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    spyOn(component, 'getInventoryItemDetails').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getInventoryItemById` method on getInventoryItemDetails and match the result', () => {
    component.inventoryId = inventoryId;
    component.getInventoryItemDetails();
    expect(inventoryService.getInventoryItemById).toHaveBeenCalled();
  });

  it('should call `updateInventoryItem` method of InventoryService on updateInventoryItem ', () => {
    component.inventoryItem = invenotryItem;
    component.inventoryId = inventoryId;
    component.updateInventoryItem(formValues);
    expect(inventoryService.updateInventoryItem).toHaveBeenCalled();
    expect(toastService.showSuccess).toHaveBeenCalled();
    expect(component.getInventoryItemDetails).toHaveBeenCalled();
  });

  it('should call `createInventoryItem` method of InventoryService on saveInventoryItem', () => {
    component.editmode = false;
    component.saveInventoryItem(formValues);
    expect(toastService.showSuccess).toHaveBeenCalled();
    expect(inventoryService.createInventoryItem).toHaveBeenCalled();
  });

  it('inventoryForm form invalid when empty', () => {
    expect(component.inventoryForm.valid).toBeFalsy();
  });

  it('inventoryForm form valid when all required fileds are set', () => {
    component.inventoryId = inventoryId;
    component.inventoryForm.controls.item.setValue(itemFormControlValue);
    component.inventoryForm.controls.itemId.setValue(3546);
    component.inventoryForm.controls.currentLocationId.setValue(
      invenotryItemResponse.currentLocationId
    );
    component.inventoryForm.controls.location.setValue({
      id: 460,
      name: 'Audi',
    });
    component.inventoryForm.controls.count.setValue(1);

    expect(component.inventoryForm.valid).toBeTruthy();
  });

  it('should call `searchItems` method of ItemService on searchItems', () => {
    const itemsList = [
      {
        id: itemRecord.id,
        name: itemRecord.name,
      },
    ];
    component.searchItems({query: 'query'});
    expect(itemService.searchItems).toHaveBeenCalled();
    expect(component.itemsList).toEqual(itemsList);
  });

  it('should call `searchSites` method of SiteService on searchLocation', () => {
    const locations = [
      {
        name: site.name,
        id: site.id,
      },
    ];
    component.searchLocation({query: 'query'});
    expect(siteService.searchSites).toHaveBeenCalled();
    expect(component.locations).toEqual(locations);
  });

  it('should call `deleteInventoryItem` method of InventoryService on deleteConfirmed', () => {
    component.inventoryItem = invenotryItem;
    component.deleteConfirmed();
    expect(inventoryService.deleteInventoryItem).toHaveBeenCalled();
    expect(toastService.showSuccess).toHaveBeenCalled();
  });

  const inventoryId = 3546;

  const invenotryItemResponse = {
    id: 3546,
    item: {
      id: 2578,
      categories: [],
      subCategories: [],
      netSuiteItemId: 6907,
      isDME: false,
      isConsumable: false,
      name: 'KANGAROO 924 PUMP (DISCONTINUED)',
      description: '4EP-0056-Enteral Feeding Pump - Kangaroo 924',
      itemNumber: '4EP-0056',
      cogsAccountName: '700T COS - Supplies',
      depreciation: 0.0,
      averageCost: 0.0,
      avgDeliveryProcessingTime: 15.0,
      avgPickUpProcessingTime: 15.0,
      categoryId: 0,
      isSerialized: true,
      isAssetTagged: true,
      isLotNumbered: false,
    },
    statusId: 1,
    status: 'Available',
    currentLocationType: 'Vehicle',
    netSuiteInventoryId: 4760,
    lotNumber: null,
    assetTagNumber: '13012374',
    itemId: 2578,
    serialNumber: 'W1357154X',
    count: 1,
    quantityAvailable: 1,
    currentLocationTypeId: 2,
    currentLocationId: 460,
  };

  const invenotryItem = {
    id: 3546,
    item: {
      id: 2578,
      categories: [],
      subCategories: [],
      name: 'KANGAROO 924 PUMP (DISCONTINUED)',
      productNumber: '',
      description: '4EP-0056-Enteral Feeding Pump - Kangaroo 924',
      itemNumber: '4EP-0056',
      cogsAccountName: '700T COS - Supplies',
      depreciation: 0.0,
      averageCost: 0.0,
      avgDeliveryProcessingTime: 15.0,
      avgPickUpProcessingTime: 15.0,
      categoryId: 0,
      isSerialized: true,
      isAssetTagged: true,
      isLotNumbered: false,
    },
    statusId: 1,
    status: 'Available',
    currentLocationType: 'Vehicle',
    netSuiteInventoryId: 4760,
    lotNumber: null,
    assetTagNumber: '13012374',
    itemId: 2578,
    serialNumber: 'W1357154X',
    count: 1,
    quantityAvailable: 1,
    currentLocationTypeId: 2,
    currentLocationId: 460,
  };

  const formValues = {
    count: 1,
    currentLocationId: 460,
    currentLocationTypeId: 2,
    id: 3546,
    itemId: 2578,
    netSuiteInventoryId: 4760,
    quantityAvailable: 1,
    serialNumber: 'W1357154X',
    statusId: 1,
  };

  const inventoryItemDetails = {
    id: 3546,
    item: {
      id: 2578,
      categories: [],
      subCategories: [],
      netSuiteItemId: 6907,
      isDME: false,
      isConsumable: false,
      name: 'KANGAROO 924 PUMP (DISCONTINUED)',
      description: '4EP-0056-Enteral Feeding Pump - Kangaroo 924',
      itemNumber: '4EP-0056',
      cogsAccountName: '700T COS - Supplies',
      depreciation: 0.0,
      averageCost: 0.0,
      avgDeliveryProcessingTime: 15.0,
      avgPickUpProcessingTime: 15.0,
      categoryId: 0,
      isSerialized: true,
      isAssetTagged: true,
      isLotNumbered: false,
    },
    statusId: 1,
    status: null,
    currentLocationType: 'Vehicle',
    netSuiteInventoryId: 4760,
    lotNumber: null,
    assetTagNumber: '13012374',
    itemId: 2578,
    serialNumber: 'W1357154X',
    count: 1,
    quantityAvailable: 1,
    currentLocationTypeId: 2,
    currentLocationId: 460,
  };

  const itemFormControlValue = {
    averageCost: 0.0,
    avgDeliveryProcessingTime: 15.0,
    avgPickUpProcessingTime: 15.0,
    categories: [],
    categoryId: 0,
    cogsAccountName: '700T COS - Supplies',
    depreciation: 0.0,
    description: '4EP-0056-Enteral Feeding Pump - Kangaroo 924',
    id: 2578,
    isAssetTagged: true,
    isDME: false,
    isConsumable: false,
    isSerialized: true,
    isLotNumbered: false,
    subCategories: [],
    netSuiteItemId: 6907,
    name: 'KANGAROO 924 PUMP (DISCONTINUED)',
    itemNumber: '4EP-0056',
  };

  const category = {
    id: 1,
    itemSubCategories: [
      {
        id: 1,
        name: 'CPAP & BIPAP Accessories',
      },
    ],
  };

  const itemRecord = {
    averageCost: 1.87,
    avgDeliveryProcessingTime: 15,
    avgPickUpProcessingTime: 15,
    categories: [category],
    category: 'Respiratory',
    categoryId: 0,
    cogsAccountName: '7020 COS - Supplies : COS - Supplies:7020 · Medical Supplies',
    depreciation: 0,
    description: 'C-PAP MASK SMALL',
    id: 1,
    isAssetTagged: false,
    isConsumable: true,
    isDME: false,
    isLotNumbered: false,
    isSerialized: false,
    itemNumber: '1CB-0003',
    name: 'C-PAP MASK SMALL',
    netSuiteItemId: 24,
    subCategories: [
      {
        id: 1,
        name: 'CPAP & BIPAP Accessories',
      },
    ],
  };

  const itemsResponse: PaginationResponse = {
    pageNumber: 1,
    pageSize: 25,
    records: [itemRecord],
    totalPageCount: 2,
    totalRecordCount: 40,
  };

  const site = {
    address: {},
    capacity: 0,
    currentDriverId: 0,
    currentDriverName: null,
    cvn: '',
    id: 1,
    isActive: false,
    isDisable: false,
    length: 0,
    licensePlate: '',
    locationType: 'Site',
    name: 'Austin (South), TX',
    netSuiteLocationId: 15,
    parentNetSuiteLocationId: 8,
    siteCode: 2240,
    siteId: null,
    siteName: null,
    sitePhoneNumber: [],
    vehicles: null,
    vin: '',
  };

  const siteResponse: PaginationResponse = {
    pageNumber: 1,
    pageSize: 25,
    records: [site],
    totalPageCount: 1,
    totalRecordCount: 1,
  };
});
