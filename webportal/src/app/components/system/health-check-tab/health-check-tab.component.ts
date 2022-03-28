import {Component, OnInit} from '@angular/core';
import {finalize} from 'rxjs/operators';
import {SystemService} from 'src/app/services';
import {getSeconds} from 'src/app/utils';

@Component({
  selector: 'app-health-check-tab',
  templateUrl: './health-check-tab.component.html',
  styleUrls: ['./health-check-tab.component.scss'],
})
export class HealthCheckTabComponent implements OnInit {
  statusResponse;
  statusResponseKeys = [];
  isLoading = true;
  constructor(private systemService: SystemService) {}

  ngOnInit(): void {
    this.isLoading = true;
    this.systemService
      .getHealthCheck()
      .pipe(
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe(response => {
        this.statusResponse = response;
        this.statusResponseKeys = Object.keys(this.statusResponse.entries);
        this.formatAllTime();
      });
  }

  formatAllTime() {
    this.statusResponse.totalDuration = getSeconds(this.statusResponse.totalDuration);
    this.statusResponseKeys.forEach(key => {
      this.statusResponse.entries[key].duration = getSeconds(
        this.statusResponse.entries[key].duration
      );
    });
  }
}
