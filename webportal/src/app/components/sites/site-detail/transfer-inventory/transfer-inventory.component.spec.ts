import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {TransferInventoryComponent} from './transfer-inventory.component';
import {OAuthModule} from 'angular-oauth2-oidc';
import {ReactiveFormsModule, FormsModule} from '@angular/forms';
import {MessageService} from 'primeng/api';
import {AutoCompleteModule} from 'primeng/autocomplete';
import {DropdownModule} from 'primeng/dropdown';
import {TransferState} from '@angular/platform-browser';
import {ItemsService, SitesService} from 'src/app/services';
import {BehaviorSubject, of} from 'rxjs';

describe('TransferInventoryComponent', () => {
  let component: TransferInventoryComponent;
  let fixture: ComponentFixture<TransferInventoryComponent>;
  let productService: any;
  let siteService: any;

  beforeEach(
    waitForAsync(() => {
      const productServiceStub = jasmine.createSpyObj('ItemsService', [
        'getItemDetailsById',
        'transferProduct',
      ]);
      productServiceStub.getItemDetailsById.and.returnValue(of(productResponse));
      productServiceStub.transferProduct.and.returnValue(of({}));
      const siteServiceStub = jasmine.createSpyObj('SitesService', ['getSiteById']);
      siteServiceStub.getSiteById.and.returnValue(new BehaviorSubject(siteResponse));

      TestBed.configureTestingModule({
        declarations: [TransferInventoryComponent],
        imports: [
          RouterTestingModule.withRoutes([{path: 'sites/info/:siteId', redirectTo: ''}]),
          HttpClientTestingModule,
          ReactiveFormsModule,
          AutoCompleteModule,
          FormsModule,
          DropdownModule,
          OAuthModule.forRoot(),
        ],
        providers: [
          {
            provide: ItemsService,
            useValue: productServiceStub,
          },
          {
            provide: SitesService,
            useValue: siteServiceStub,
          },
          MessageService,
          TransferState,
        ],
      }).compileComponents();
      productService = TestBed.inject(ItemsService);
      siteService = TestBed.inject(SitesService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(TransferInventoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('inventoryTransferForm form invalid when empty', () => {
    expect(component.inventoryTransferForm.valid).toBeFalsy();
  });

  it('inventoryTransferForm form valid when all required fileds are set', () => {
    component.inventoryTransferForm.controls.sourceLocationTypeId.setValue(1);
    component.inventoryTransferForm.controls.sourceLocation.setValue({
      name: 'Austin (South), TX',
      id: 1,
    });
    component.inventoryTransferForm.controls.destinationLocationTypeId.setValue(2);
    component.inventoryTransferForm.controls.destinationLocation.setValue({
      name: 'Austin (South), TX',
      id: 2,
    });

    expect(component.inventoryTransferForm.valid).toBeTruthy();
  });

  it('should call `getItemDetailsById` of ProductService on getItemDetails and match the result', () => {
    component.itemIds = [1];
    component.getItemDetails();
    expect(productService.getItemDetailsById).toHaveBeenCalled();
    expect(component.items).toEqual([productResponse]);
  });

  it('should call `getSiteById` of SiteService on getSourceLocation and match the result', () => {
    component.sourceLocationId = 3;
    component.getSourceLocation();
    expect(siteService.getSiteById).toHaveBeenCalled();
    expect(component.sourceLocation).toEqual({
      name: siteResponse.name,
      id: siteResponse.id,
    });
    expect(component.inventoryTransferForm.controls.sourceLocation.value).toEqual({
      name: siteResponse.name,
      id: siteResponse.id,
    });
  });

  it('should call `transferProduct` of ProductService on requestTransfer and match the result', () => {
    const location = {
      name: siteResponse.name,
      id: siteResponse.id,
    };
    component.inventoryTransferForm.patchValue({
      sourceLocationTypeId: 1,
      sourceLocation: location,
      destinationLocationTypeId: 1,
      destinationLocation: location,
    });
    component.items = [siteResponse];
    component.requestTransfer();
    expect(productService.transferProduct).toHaveBeenCalled();
  });

  const productResponse = {
    averageCost: 0.32,
    avgDeliveryProcessingTime: 15,
    avgPickUpProcessingTime: 15,
    categories: [],
    categoryId: 0,
    cogsAccountName: '7020 COS - Supplies : COS - Supplies:7020 · Medical Supplies',
    depreciation: 0,
    description: 'Oxygen Humidifier Bottle - 500 ML',
    id: 4076,
    isAssetTagged: false,
    isConsumable: true,
    isDME: false,
    isLotNumbered: false,
    isSerialized: false,
    itemNumber: '1CS-0002',
    name: 'Oxygen Humidifier Bottle - 500 ML',
    netSuiteItemId: 33,
    subCategories: [],
  };

  const siteResponse = {
    address: {},
    capacity: 0,
    currentDriverId: 0,
    currentDriverName: null,
    cvn: '',
    id: 3,
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
    vin: '',
  };
});
