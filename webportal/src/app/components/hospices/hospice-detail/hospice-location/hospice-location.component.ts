import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {HospiceLocationService} from 'src/app/services';
import {ActivatedRoute} from '@angular/router';
import {finalize} from 'rxjs/operators';

@Component({
  selector: 'app-hospice-location',
  templateUrl: './hospice-location.component.html',
  styleUrls: ['./hospice-location.component.scss'],
})
export class HospiceLocationComponent implements OnInit {
  allLocations: any;
  allLocationCount = 0;
  pageNumber = 0;
  pageSize = 10;
  totalPageCount = 0;
  hospiceLocationRequest: any;
  locationsHeaders = [{label: 'Name', field: 'name', sortable: true}];
  hospiceId: number;
  locationsLoading = false;

  @Output() showDetailsView = new EventEmitter<any>();

  constructor(
    private hospiceLocationService: HospiceLocationService,
    private route: ActivatedRoute
  ) {
    this.route.params.subscribe((params: any) => {
      this.hospiceId = params.hospiceId;
      this.getHospiceLocations();
    });
  }

  ngOnInit(): void {}
  getHospiceLocations() {
    this.locationsLoading = true;
    this.hospiceLocationService
      .getHospiceLocations(this.hospiceId)
      .pipe(
        finalize(() => {
          this.locationsLoading = false;
        })
      )
      .subscribe((response: any) => {
        this.allLocations = response.records;
        this.allLocationCount = response.totalRecordCount;
        this.pageNumber = response.pageNumber;
        this.pageSize = response.pageSize;
        this.totalPageCount = response.totalPageCount;
      });
  }

  locationSelected({currentRow}) {
    this.showDetailsView.emit({location: currentRow});
  }
}
