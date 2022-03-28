import {HttpClientTestingModule} from '@angular/common/http/testing';
import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthModule} from 'angular-oauth2-oidc';
import {ItemCategoriesService} from 'src/app/services';
import {ItemCategoryDetailsComponent} from './item-category-details.component';
import {BehaviorSubject} from 'rxjs';

describe('ItemCategoryDetailsComponent', () => {
  let component: ItemCategoryDetailsComponent;
  let fixture: ComponentFixture<ItemCategoryDetailsComponent>;
  let itemCategoriesService: any;

  beforeEach(
    waitForAsync(() => {
      const itemCategoriesSerivceStub = jasmine.createSpyObj('ItemCategoriesService', [
        'getItemCategoryById',
      ]);
      itemCategoriesSerivceStub.getItemCategoryById.and.returnValue(
        new BehaviorSubject(itemCategoriesResponse)
      );
      TestBed.configureTestingModule({
        declarations: [ItemCategoryDetailsComponent],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: ItemCategoriesService,
            useValue: itemCategoriesSerivceStub,
          },
        ],
      }).compileComponents();
      itemCategoriesService = TestBed.inject(ItemCategoriesService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(ItemCategoryDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getItemCategoryById` of ItemCategoriesService on getCategoryDetails', () => {
    component.categoryId = 1;
    component.getCategoryDetails();
    expect(itemCategoriesService.getItemCategoryById).toHaveBeenCalled();
    expect(component.categoryDetails).toEqual(itemCategoriesResponse);
  });

  const itemCategoriesResponse = {
    id: 1,
    name: 'Respiratory',
    itemSubCategories: [
      {
        id: 1,
        name: 'CPAP & BIPAP Accessories',
      },
    ],
  };
});
