import {HttpClientTestingModule} from '@angular/common/http/testing';
import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthModule} from 'angular-oauth2-oidc';
import {ItemCategoriesService} from 'src/app/services';
import {ItemCategoriesComponent} from './item-categories.component';
import {BehaviorSubject} from 'rxjs';
import {PaginationResponse, SieveRequest} from 'src/app/models';

describe('ItemCategoriesComponent', () => {
  let component: ItemCategoriesComponent;
  let fixture: ComponentFixture<ItemCategoriesComponent>;
  let categoriesService: any;

  beforeEach(
    waitForAsync(() => {
      const categoriesSerivceStub = jasmine.createSpyObj('ItemCategoriesService', [
        'getItemCategories',
        'searchItemCategories',
      ]);
      categoriesSerivceStub.getItemCategories.and.returnValue(
        new BehaviorSubject<PaginationResponse>(itemCategoriesResponse)
      );
      categoriesSerivceStub.searchItemCategories.and.returnValue(
        new BehaviorSubject<PaginationResponse>(itemCategoriesResponse)
      );

      TestBed.configureTestingModule({
        declarations: [ItemCategoriesComponent],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: ItemCategoriesService,
            useValue: categoriesSerivceStub,
          },
        ],
      }).compileComponents();
      categoriesService = TestBed.inject(ItemCategoriesService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(ItemCategoriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getItemCategories` of ItemCategoriesService on getCategoriesList', () => {
    component.searchQuery = '';
    component.categoriesReqeust = new SieveRequest();
    component.getCategoriesList();
    expect(categoriesService.getItemCategories).toHaveBeenCalled();
    expect(component.categoriesResponse).toEqual(itemCategoriesResponse);
  });

  it('should call `searchItemCategories` of ItemCategoriesService on getCategoriesList', () => {
    component.searchQuery = 'query';
    component.categoriesReqeust = new SieveRequest();
    component.getCategoriesList();
    expect(categoriesService.searchItemCategories).toHaveBeenCalled();
    expect(component.categoriesResponse).toEqual(itemCategoriesResponse);
  });

  const category = {
    id: 1,
    name: 'Respiratory',
    itemSubCategories: [
      {
        id: 1,
        name: 'CPAP & BIPAP Accessories',
      },
    ],
  };
  const itemCategoriesResponse: PaginationResponse = {
    pageNumber: 1,
    pageSize: 25,
    records: [category],
    totalPageCount: 2,
    totalRecordCount: 30,
  };
});
