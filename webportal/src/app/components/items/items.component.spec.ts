import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {ItemComponent} from './items.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule} from 'angular-oauth2-oidc';
import {ItemsService} from 'src/app/services';
import {MessageService, ConfirmationService} from 'primeng/api';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {BehaviorSubject} from 'rxjs';

describe('ItemComponent', () => {
  let component: ItemComponent;
  let fixture: ComponentFixture<ItemComponent>;
  let itemService: any;

  beforeEach(
    waitForAsync(() => {
      const itemSerivceStub = jasmine.createSpyObj('ItemsService', ['getItemsList', 'searchItems']);
      itemSerivceStub.getItemsList.and.returnValue(
        new BehaviorSubject<PaginationResponse>(itemsResponse)
      );
      itemSerivceStub.searchItems.and.returnValue(
        new BehaviorSubject<PaginationResponse>(itemsResponse)
      );

      TestBed.configureTestingModule({
        declarations: [ItemComponent],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: ItemsService,
            useValue: itemSerivceStub,
          },
          MessageService,
        ],
      }).compileComponents();
      itemService = TestBed.inject(ItemsService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(ItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getItemsList` method of itemService on getItemsList when searchQuery is set', () => {
    component.searchQuery = '';
    component.itemsRequest = new SieveRequest();
    component.getItemsList();
    expect(itemService.getItemsList).toHaveBeenCalled();
    expect(component.itemsResponse).toEqual(itemsResponse);
  });

  it('should call `getItemsList` method of itemService on getItemsList when searchQuery is set', () => {
    component.searchQuery = '22 mm';
    component.itemsRequest = new SieveRequest();
    component.getItemsList();
    expect(itemService.searchItems).toHaveBeenCalled();
    expect(component.itemsResponse).toEqual(itemsResponse);
  });

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
});
