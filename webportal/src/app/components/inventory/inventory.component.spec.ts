import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {InventoryComponent} from './inventory.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule} from 'angular-oauth2-oidc';
import {InventoryService} from 'src/app/services';
import {MessageService, ConfirmationService} from 'primeng/api';
import {TransferState} from '@angular/platform-browser';
import {BehaviorSubject} from 'rxjs';
import {PaginationResponse, SieveRequest} from 'src/app/models';

describe('InventoryComponent', () => {
  let component: InventoryComponent;
  let fixture: ComponentFixture<InventoryComponent>;
  let inventoryService: any;

  beforeEach(
    waitForAsync(() => {
      const inventoryServiceStub = jasmine.createSpyObj('InventoryService', [
        'getInvetoryList',
        'searchInventory',
      ]);
      inventoryServiceStub.getInvetoryList.and.returnValue(
        new BehaviorSubject<PaginationResponse>(inventoryResponse)
      );
      inventoryServiceStub.searchInventory.and.returnValue(
        new BehaviorSubject<PaginationResponse>(inventoryResponse)
      );

      TestBed.configureTestingModule({
        declarations: [InventoryComponent],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: InventoryService,
            useValue: inventoryServiceStub,
          },
          MessageService,
          TransferState,
        ],
      }).compileComponents();
      inventoryService = TestBed.inject(InventoryService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(InventoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'getInventoryList').and.callThrough();
    spyOn(component, 'getLocationInfo').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getInventoryList` on ngOnInit', () => {
    component.ngOnInit();
    expect(component.getInventoryList).toHaveBeenCalled();
    expect(inventoryService.getInvetoryList).toHaveBeenCalled();
  });

  it('should call `getInvetoryList` and `searchInventory` and  on getInventoryList and match the result', () => {
    // when searchQuery is empty
    component.searchQuery = '';
    component.getInventoryList();
    expect(inventoryService.getInvetoryList).toHaveBeenCalled();
    expect(component.inventoryResponse).toEqual(inventoryResponse);
    expect(component.getLocationInfo).toHaveBeenCalled();

    // when searchQuery is not empty
    component.searchQuery = 'kangaroo';
    component.inventoryRequest = new SieveRequest();
    component.getInventoryList();
    expect(inventoryService.searchInventory).toHaveBeenCalled();
    expect(component.inventoryResponse).toEqual(inventoryResponse);
    expect(component.getLocationInfo).toHaveBeenCalled();
  });

  const inventoryItem = {
    assetTagNumber: '13012374',
    count: 1,
    currentLocationId: 10,
    currentLocationType: 'Site',
    currentLocationTypeId: 1,
    id: 3546,
    item: {},
    itemId: 2578,
    lotNumber: null,
    netSuiteInventoryId: 4760,
    quantityAvailable: 1,
    serialNumber: 'W1357154X',
    status: 'Available',
    statusId: 1,
  };

  const inventoryResponse: PaginationResponse = {
    records: [inventoryItem],
    pageSize: 20,
    pageNumber: 1,
    totalRecordCount: 1,
    totalPageCount: 1,
  };
});
