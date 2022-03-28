import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule} from 'angular-oauth2-oidc';
import {HospiceService} from 'src/app/services';
import {BehaviorSubject} from 'rxjs';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {ZabSubscriptionItemsComponent} from './zab-subscription-items.component';

describe('ZabSubscriptionItemsComponent', () => {
  let component: ZabSubscriptionItemsComponent;
  let fixture: ComponentFixture<ZabSubscriptionItemsComponent>;
  let hospiceService: any;

  beforeEach(
    waitForAsync(() => {
      const hospiceServiceStub = jasmine.createSpyObj('HospiceService', [
        'getHospiceZabSubscriptionItems',
      ]);
      hospiceServiceStub.getHospiceZabSubscriptionItems.and.returnValue(
        new BehaviorSubject<PaginationResponse>(itemResponse)
      );

      TestBed.configureTestingModule({
        declarations: [ZabSubscriptionItemsComponent],
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
    fixture = TestBed.createComponent(ZabSubscriptionItemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getHospiceZabSubscriptionItems` of HospiceService method on getSubscriptionItems', () => {
    component.subscriptionRequest = new SieveRequest();
    component.getSubscriptionItems();
    expect(hospiceService.getHospiceZabSubscriptionItems).toHaveBeenCalled();
    expect(component.subscriptionItemResponse).toEqual(itemResponse);
  });

  const item = {
    adjustmentExcludePriorToEffectiveDate: false,
    applyAdjustmentsRetroactively: false,
    applyPrepaidToAllSubscriptionItems: false,
    billInArrears: true,
    chargeIncludedUnits: true,
    chargeScheduleUsed: 'Monthly (Calendar)',
    createdDateTime: '2021-04-28T03:12:00Z',
    creditRebillPriorNewCountCharges: false,
    currency: 'USA',
    endDate: '2022-01-31T00:00:00Z',
    excludeChargesFromBillingWhen: 'Zero Quantity',
    excludeFromOrderLevelMinimumCommitment: false,
    hospice: null,
    hospiceId: 5,
    id: 1,
    inheritChargeScheduleFrom: 'Subscription',
    invertNegativeQuantity: false,
    isInactive: false,
    item: null,
    itemDescription: null,
    itemId: null,
    lastModifiedDatetime: '2021-04-28T03:12:00Z',
    name: 'Patient Level included units (Tanks) O2-004, O2-005 Credit',
    netSuiteChargeScheduleUsedId: 1,
    netSuiteCurrencyId: 1,
    netSuiteCustomerId: 32521,
    netSuiteExcludeChargesFromBillingWhenId: 2,
    netSuiteInheritChargeScheduleFromId: 1,
    netSuiteItemId: 5350,
    netSuiteLastFetchedDateTime: '2021-04-30T19:16:22.4356532Z',
    netSuiteProRationTypeId: 3,
    netSuiteRatePlanId: 57,
    netSuiteRateTypeId: 2,
    netSuiteRenewalItemId: null,
    netSuiteSubscriptionId: 105,
    netSuiteSubscriptionItemId: 6245,
    overageRate: null,
    proRationType: 'Do Not Prorate',
    pushFutureCountCharges: false,
    ratePlan: 'Patient Level included units (Tanks) O2-004, O2-005 Credit Rate Plan',
    rateType: 'Usage (Variable)',
    ratingPriority: 2,
    ratingScheduleBillInArrears: false,
    renewalItem: null,
    startDate: '2021-02-01T00:00:00Z',
    subscriptionId: 2,
    useAlternateTermMultiplier: false,
  };

  const itemResponse: PaginationResponse = {
    pageNumber: 1,
    pageSize: 25,
    records: [item],
    totalPageCount: 1,
    totalRecordCount: 1,
  };
});
