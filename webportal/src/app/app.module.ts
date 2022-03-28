import {BrowserModule, TransferState} from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {NgModule, ErrorHandler} from '@angular/core';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import {OAuthModule} from 'angular-oauth2-oidc';
import {TableModule} from 'primeng/table';
import {PaginatorModule} from 'primeng/paginator';
import {DropdownModule} from 'primeng/dropdown';
import {ButtonModule} from 'primeng/button';
import {InputSwitchModule} from 'primeng/inputswitch';
import {MenuModule} from 'primeng/menu';
import {PanelModule} from 'primeng/panel';
import {MessageModule} from 'primeng/message';
import {InputTextModule} from 'primeng/inputtext';
import {MultiSelectModule} from 'primeng/multiselect';
import {ToastModule} from 'primeng/toast';
import {CardModule} from 'primeng/card';
import {InputMaskModule} from 'primeng/inputmask';
import {CheckboxModule} from 'primeng/checkbox';
import {TabMenuModule} from 'primeng/tabmenu';
import {TabViewModule} from 'primeng/tabview';
import {DialogModule} from 'primeng/dialog';
import {ConfirmDialogModule} from 'primeng/confirmdialog';
import {TooltipModule} from 'primeng/tooltip';
import {CalendarModule} from 'primeng/calendar';
import {ProgressSpinnerModule} from 'primeng/progressspinner';
import {InputNumberModule} from 'primeng/inputnumber';
import {AutoCompleteModule} from 'primeng/autocomplete';
import {DataViewModule} from 'primeng/dataview';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {MessageService, ConfirmationService} from 'primeng/api';
import {StepsModule} from 'primeng/steps';
import {FileUploadModule} from 'primeng/fileupload';
import {FieldsetModule} from 'primeng/fieldset';
import {SidebarModule} from 'primeng/sidebar';
import {GalleriaModule} from 'primeng/galleria';
import {SelectButtonModule} from 'primeng/selectbutton';
import {DragDropModule} from 'primeng/dragdrop';
import {DragAndDropModule} from 'angular-draggable-droppable';
import {TreeTableModule} from 'primeng/treetable';
import {ChipsModule} from 'primeng/chips';
import {InplaceModule} from 'primeng/inplace';
import {RadioButtonModule} from 'primeng/radiobutton';
import {PanelMenuModule} from 'primeng/panelmenu';
import {ListboxModule} from 'primeng/listbox';
import {ChartModule} from 'primeng/chart';
import {AccordionModule} from 'primeng/accordion';
import {NgxBarcodeModule} from 'ngx-barcode';
import {NgxOrgChartModule} from 'ngx-org-chart';
import {NgxPanZoomModule} from 'ngx-panzoom';
import {NgxSliderModule} from '@angular-slider/ngx-slider';
import {ImageCropperModule} from 'ngx-image-cropper';

import * as components from './components';
import * as common from './common';
import * as constants from './constants';
import * as guards from './guards';
import * as services from './services';
import * as pipes from './pipes';

import {environment} from 'src/environments/environment';
import {DatePipe} from '@angular/common';
import {CalendarModule as AngularCalendarModule, DateAdapter} from 'angular-calendar';
import {adapterFactory} from 'angular-calendar/date-adapters/date-fns';

@NgModule({
  declarations: [
    AppComponent,
    common.NavbarComponent,
    common.LayoutMainComponent,
    common.LayoutPublicComponent,
    common.UserListComponent,
    common.UserRoleListComponent,
    common.TableComponent,
    common.TableVirtualScrollComponent,
    common.BodyHeaderComponent,
    common.ModalComponent,
    common.SidenavComponent,
    common.AddressComponent,
    common.ImportWizardComponent,
    common.ImportWizardValidatorComponent,
    common.ConfirmDialogComponent,
    common.AddressDetailsComponent,
    common.EditableTableComponent,
    common.MappingsComponent,
    common.InputMappingsComponent,
    common.OutputMappingsComponent,
    common.SortIconComponent,
    common.FiltersComponent,
    common.FilterFieldComponent,
    common.LocationInventoryComponent,
    common.TreeTableComponent,
    common.SearchBarComponent,
    common.AddressDetailsComponent,
    common.EditableTableComponent,
    common.CreateOrderModalComponent,
    common.AddressListComponent,
    common.ImageGalleryComponent,
    common.AddressVerificationModalComponent,
    common.PatientInfoComponent,
    common.OrderingPatientInventoryComponent,
    common.OrderNotesComponent,
    components.LoginComponent,
    components.UsersComponent,
    components.UserDetailComponent,
    components.RolesPermissionComponent,
    components.AddUserComponent,
    components.AuditLogsComponent,
    components.PatientsComponent,
    components.PatientAddEditComponent,
    components.HospicesComponent,
    components.HospiceDetailComponent,
    components.HospiceMemberComponent,
    components.HospiceLocationComponent,
    components.ImportHospiceMembersComponent,
    components.HospiceFacilityComponent,
    components.AddEditFacilityComponent,
    components.HospiceSettingsComponent,
    components.HospiceZabSubscriptionComponent,
    components.ZabSubscriptionItemsComponent,
    components.SitesComponent,
    components.SiteDetailComponent,
    components.HospiceMemberMappingsComponent,
    components.OrdersComponent,
    components.OrdersComponent,
    components.ItemComponent,
    components.InventoryComponent,
    components.OrdersComponent,
    components.DriversComponent,
    components.DriverAddEditComponent,
    components.AddEditInventoryComponent,
    components.VehiclesComponent,
    components.SiteMembersComponent,
    components.AddEditSiteMemberComponent,
    components.AddEditHospiceMemberComponent,
    components.TransferAuditLogComponent,
    components.TransferAuditDetailComponent,
    components.DispatchComponent,
    components.DispatchListComponent,
    components.DispatchAssignComponent,
    components.TransferInventoryComponent,
    components.DispatchSchedulerComponent,
    components.DispatchListItemComponent,
    components.DispatchMapComponent,
    components.PatientSearchComponent,
    components.SystemComponent,
    components.ItemCategoriesComponent,
    components.ItemCategoryDetailsComponent,
    components.SystemCardComponent,
    components.PatientInventoryComponent,
    components.SiteServiceAreasComponent,
    components.ForbiddenComponent,
    components.ImportHospiceFacilityComponent,
    components.DispatchOrderDetailComponent,
    components.PatientsFiltersComponent,
    components.AddFacilityComponent,
    components.DispatchFulfillOrderComponent,
    components.DispatchFulfillOrderItemComponent,
    components.ScaMyAccountComponent,
    components.InventoryFiltersComponent,
    components.OrderFlyoutComponent,
    components.HealthCheckTabComponent,
    components.SitesFiltersComponent,
    components.OrdersFilterComponent,
    components.SystemFiltersComponent,
    components.ProfileComponent,
    components.FeatureFlagsComponent,
    components.PatientDispatchComponent,
    components.PatientDispatchUpdateComponent,
    components.FinanceComponent,
    components.PatientHospiceComponent,
    components.MovePatientHospiceComponent,
    components.PatientMergeComponent,
    components.ZabSubscriptionContractsComponent,
    components.CancelOrderDialogComponent,
    components.AddEditPickupComponent,
    components.OrderHmsComponent,
    components.ReportPortalComponent,
    components.ChartComponent,
    components.AddEditMoveComponent,
    components.DeliveryDetailsComponent,
    components.AddEditDeliveryComponent,
    components.OrderReviewComponent,
    components.CurrentInventoryOrderComponent,
    components.EquipmentSettingsConfigComponent,
    components.AddOnsEquipmentSettingsComponent,
    components.AddEditExchangeComponent,
    components.AddOnConfigComponent,
    components.AddEditAddonsComponent,
    components.OperationsComponent,
    components.ValueWidgetComponent,
    components.HospiceContractsComponent,
    components.ContractItemsComponent,
    components.ClientServicesComponent,
    pipes.TruncatePipe,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    TableModule,
    DropdownModule,
    PaginatorModule,
    ButtonModule,
    InputSwitchModule,
    MenuModule,
    PanelModule,
    MessageModule,
    InputTextModule,
    MultiSelectModule,
    ToastModule,
    CardModule,
    TabMenuModule,
    InputMaskModule,
    CheckboxModule,
    TabViewModule,
    DialogModule,
    ConfirmDialogModule,
    TooltipModule,
    CalendarModule,
    ProgressSpinnerModule,
    InputNumberModule,
    AutoCompleteModule,
    DataViewModule,
    SidebarModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    StepsModule,
    FileUploadModule,
    FieldsetModule,
    GalleriaModule,
    DragDropModule,
    SelectButtonModule,
    DragAndDropModule,
    TreeTableModule,
    ChipsModule,
    InplaceModule,
    RadioButtonModule,
    PanelMenuModule,
    ListboxModule,
    AccordionModule,
    ChartModule,
    OAuthModule.forRoot({
      resourceServer: {
        allowedUrls: [environment.apiServerURL, environment.patientApiServerUrl],
        sendAccessToken: true,
      },
    }),
    AngularCalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory,
    }),
    NgxBarcodeModule,
    NgxOrgChartModule,
    NgxPanZoomModule,
    NgxSliderModule,
    ImageCropperModule,
  ],
  providers: [
    {provide: ErrorHandler, useClass: services.GlobalErrorHandler},
    {
      provide: HTTP_INTERCEPTORS,
      useClass: services.ServerErrorInterceptor,
      multi: true,
    },
    MessageService,
    ConfirmationService,
    constants.MessageConstants,
    guards.AuthGuard,
    guards.RoleGuard,
    DatePipe,
    pipes.PhonePipe,
    TransferState,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
