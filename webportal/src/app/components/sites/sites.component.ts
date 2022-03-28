import {Component, OnInit, ViewChild} from '@angular/core';
import {SitesService, ToastService} from 'src/app/services';
import {finalize, concatMap} from 'rxjs/operators';
import {ConfirmationService} from 'primeng/api';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {NavbarSearchService} from 'src/app/services/navbar-search.service';
import {TableVirtualScrollComponent} from 'src/app/common';
import {convertArrayToTree} from 'src/app/utils';
import {Router} from '@angular/router';
import {PanZoomConfig} from 'ngx-panzoom';
import {Subscription} from 'rxjs';
@Component({
  selector: 'app-sites',
  templateUrl: './sites.component.html',
  styleUrls: ['./sites.component.scss'],
})
export class SitesComponent implements OnInit {
  @ViewChild('sitesTable ', {static: false})
  sitesTable: TableVirtualScrollComponent;

  sitesResponse: PaginationResponse;
  sitesRequest: SieveRequest = new SieveRequest();
  headers = [
    {label: 'Name', field: 'name', sortable: true},
    {label: 'Site Code', field: 'siteCode', sortable: true},
    {label: 'Site Type', field: 'locationType'},
    {label: 'Parent Site', field: 'parent.name'},
    {
      label: '',
      field: '',
      class: 'sm',
      editBtn: 'View Site',
      editBtnIcon: 'pi pi-info-circle',
      editBtnLink: 'info',
      linkParams: 'id',
    },
  ];
  searchQuery = '';
  loading = false;
  subscriptions: Subscription[] = [];
  heirarchialSites: any;
  showHeirarchialDiagram = false;
  diagramDirection = 'vertical';
  filterValue = '';
  panZoomConfig: PanZoomConfig = new PanZoomConfig();
  @ViewChild('siteHeirarchy', {static: false}) siteHeirarchy;

  constructor(
    private siteService: SitesService,
    private confirmationService: ConfirmationService,
    private toastService: ToastService,
    private navbarSearchService: NavbarSearchService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.sitesRequest.pageSize = 200;
    this.getSites();
    this.subscriptions.push(
      this.navbarSearchService.search.subscribe(text => this.searchSites(text))
    );
    this.panZoomConfig.zoomLevels = 12;
    this.panZoomConfig.initialZoomLevel = 0.4;
  }

  getSites(filterValue?) {
    this.loading = true;
    this.siteService
      .searchSites(
        {
          ...this.sitesRequest,
          searchQuery: filterValue ? filterValue : this.searchQuery,
        },
        filterValue ? ['locationType'] : this.searchQuery ? ['name', 'locationType'] : null
      )
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((response: any) => {
        this.sitesResponse = response;
        this.heirarchialSites = convertArrayToTree(
          response.records.map(x => {
            x.title = x.locationType;
            return x;
          }),
          'netSuiteLocationId',
          'parentNetSuiteLocationId',
          'childs'
        );
        this.sitesResponse.records.map(x => {
          if (x.parentNetSuiteLocationId) {
            x.parent = response.records.find(
              s => s.netSuiteLocationId === x.parentNetSuiteLocationId
            );
          }
          return x;
        });
      });
  }

  deleteSite(site) {
    this.confirmationService.confirm({
      message: `Do you want to delete Site "${site.name}"?`,
      header: 'Delete Confirmation',
      icon: 'pi pi-info-circle',
      accept: () => {
        this.siteService.deleteSite(site.id).subscribe((response: Response) => {
          this.sitesResponse.records = this.sitesResponse.records.filter(r => r.id !== site.id);
          this.toastService.showSuccess(`Site Deleted successfully`);
        });
      },
    });
  }

  getNext({pageNum}) {
    if (!this.sitesResponse || pageNum > this.sitesResponse.totalPageCount) {
      return;
    }
    this.sitesRequest.page = pageNum;
    this.getSites(this.filterValue);
  }

  searchSites(searchQuery) {
    this.dataTablesReset();
    this.sitesRequest.page = 1;
    this.searchQuery = searchQuery;
    this.getSites(this.filterValue);
  }

  sortSites(event) {
    this.dataTablesReset();
    switch (event.order) {
      case 0:
        this.sitesRequest.sorts = '';
        break;
      case 1:
        this.sitesRequest.sorts = event.field;
        break;
      case -1:
        this.sitesRequest.sorts = '-' + event.field;
        break;
    }
    this.getSites(this.filterValue);
  }

  toggleTableAndDiagram() {
    this.showHeirarchialDiagram = !this.showHeirarchialDiagram;
  }

  onSiteClick(event) {
    this.router.navigate([`/sites/info/${event.id}`]);
  }

  filterSites(filterString) {
    this.dataTablesReset();
    this.sitesRequest.filters = filterString;
    this.filterValue = filterString?.split('==')[1];
    this.sitesRequest.page = 1;
    this.getSites(this.filterValue);
  }

  diagramDirectionChange() {
    this.panZoomConfig.initialZoomLevel = 0.4;
    this.siteHeirarchy.resetView();
    this.siteHeirarchy.zoomToFit();
  }

  dataTablesReset() {
    if (this.sitesTable) {
      this.sitesTable.reset();
    }
    this.sitesRequest.page = 1;
  }
}
