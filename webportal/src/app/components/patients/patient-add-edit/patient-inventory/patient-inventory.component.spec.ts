import {HttpClientTestingModule} from '@angular/common/http/testing';
import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {MessageService, ConfirmationService} from 'primeng/api';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {InventoryService} from 'src/app/services';
import {BehaviorSubject} from 'rxjs';
import {PatientInventoryComponent} from './patient-inventory.component';

describe('PatientInventoryComponent', () => {
  let component: PatientInventoryComponent;
  let fixture: ComponentFixture<PatientInventoryComponent>;
  let inventoryService: any;

  beforeEach(
    waitForAsync(() => {
      const inventoryServiceStub = jasmine.createSpyObj('InventoryService', [
        'getPatientInventoryByUuid',
      ]);
      inventoryServiceStub.getPatientInventoryByUuid.and.returnValue(
        new BehaviorSubject<PaginationResponse>(patientInventoryResponse)
      );
      TestBed.configureTestingModule({
        declarations: [PatientInventoryComponent],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: InventoryService,
            useValue: inventoryServiceStub,
          },
          OAuthService,
          MessageService,
          ConfirmationService,
        ],
      }).compileComponents();
      inventoryService = TestBed.inject(InventoryService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(PatientInventoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'getPatientInventory').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getPatientInventoryByUuid` method of InventoryService on getPatientInventory and match the result', () => {
    component.getPatientInventory();
    expect(inventoryService.getPatientInventoryByUuid).toHaveBeenCalled();
    expect(component.patientInventoryResponse).toEqual(patientInventoryResponse);
  });

  it('should call `getPatientInventory` method on getNextInventoryPage ', () => {
    component.patientInventoryResponse = {
      pageNumber: 1,
      totalPageCount: 3,
      totalRecordCount: 1,
      records: [],
      pageSize: 20,
    };
    component.getNextInventoryPage({pageNum: 1});
    expect(component.getPatientInventory).toHaveBeenCalled();
  });
});

const inventoryRecords = [
  {
    assetTagNumber: null,
    cogsAccountName: 'cogsAccountName',
    inventoryId: 9393,
    itemDescription: 'PAP Mask - Small',
    itemId: 4061,
    itemName: 'PAP Mask - Small',
    itemNumber: '1CB-0003',
    lotNumber: null,
    netSuiteInventoryId: 0,
    netSuiteItemId: 24,
    netSuiteOrderId: 435631,
    netSuiteOrderLineItemId: 1039891,
    orderHeaderId: 547,
    patientUuid: 'f2412fd3-963c-421b-bf7b-b9591e1dc343',
    quantity: 5,
    serialNumber: null,
  },
];

const patientInventoryResponse = {
  records: inventoryRecords,
  pageSize: 1,
  totalRecordCount: 1,
  pageNumber: 1,
  totalPageCount: 1,
};
