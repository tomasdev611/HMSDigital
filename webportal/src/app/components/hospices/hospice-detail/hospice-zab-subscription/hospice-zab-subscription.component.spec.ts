import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule} from 'angular-oauth2-oidc';
import {HospiceService} from 'src/app/services';
import {BehaviorSubject} from 'rxjs';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {HospiceZabSubscriptionComponent} from './hospice-zab-subscription.component';

describe('HospiceZabSubscriptionComponent', () => {
  let component: HospiceZabSubscriptionComponent;
  let fixture: ComponentFixture<HospiceZabSubscriptionComponent>;
  let hospiceService: any;

  beforeEach(
    waitForAsync(() => {
      const hospiceServiceStub = jasmine.createSpyObj('HospiceService', [
        'getHospiceZabSubscriptions',
      ]);
      hospiceServiceStub.getHospiceZabSubscriptions.and.returnValue(
        new BehaviorSubject<PaginationResponse>(zabSubscriptionResponse)
      );

      TestBed.configureTestingModule({
        declarations: [HospiceZabSubscriptionComponent],
        providers: [
          {
            provide: HospiceService,
            useValue: hospiceServiceStub,
          },
        ],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
      }).compileComponents();
      hospiceService = TestBed.inject(HospiceService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(HospiceZabSubscriptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getHospiceZabSubscriptions` of HospiceService method on getSubscriptions', () => {
    component.hospiceId = 1;
    component.subscriptionRequest = new SieveRequest();
    component.getSubscriptions();
    expect(hospiceService.getHospiceZabSubscriptions).toHaveBeenCalled();
    expect(component.subscriptionResponse).toEqual(zabSubscriptionResponse);
  });

  const zabRecord = {
    billToCustomer: '1st Choice Hospice, LLC',
    billToEntity: '1st Choice Hospice, LLC',
    billingProfile: 'HS Billing Profile (Location-Specific Invoicing)',
    chargeSchedule: 'Monthly (Calendar)',
    consolidateBilling: false,
    createdDateTime: '2021-04-14T05:07:00Z',
    currency: 'USA',
    enableLineItemShipping: 'Never',
    endDate: '2022-01-31T00:00:00Z',
    hospice: {},
    hospiceId: 5,
    id: 2,
    inheritChargeScheduleFromMasterContract: false,
    isInactive: false,
    lastModifiedDateTime: '2021-04-28T01:11:00Z',
    name: 'Testing 1st Choice Hospice, LLC - 02/01/2021 - 01/31/2022 ',
    netSuiteBillToCustomerId: 32521,
    netSuiteBillToEntityId: 32521,
    netSuiteBillingProfileId: 1,
    netSuiteChargeScheduleId: 1,
    netSuiteCurrencyId: 1,
    netSuiteCustomerId: 32521,
    netSuiteEnableLineItemShippingId: 1,
    netSuiteLastFetchedDateTime: '2021-04-30T19:06:55.8434675Z',
    netSuiteRenewalTemplateId: null,
    netSuiteSubscriptionId: 105,
    renewalTemplate: null,
    startDate: '2021-02-01T00:00:00Z',
    subscriptionItems: [],
  };

  const zabSubscriptionResponse: PaginationResponse = {
    pageNumber: 1,
    pageSize: 25,
    records: [zabRecord],
    totalPageCount: 1,
    totalRecordCount: 1,
  };
});
