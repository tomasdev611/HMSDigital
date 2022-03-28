import {Component, OnInit, ChangeDetectorRef, AfterContentChecked} from '@angular/core';
import {finalize} from 'rxjs/operators';
import {PaginationResponse} from 'src/app/models';
import {SystemService, ToastService} from 'src/app/services';

@Component({
  selector: 'app-feature-flags-tab',
  templateUrl: './feature-flags.component.html',
  styleUrls: ['./feature-flags.component.scss'],
})
export class FeatureFlagsComponent implements OnInit, AfterContentChecked {
  isLoading = true;
  disableUpdates = false;
  featureFlags: any[];

  constructor(
    private systemService: SystemService,
    private toastService: ToastService,
    private changeDetector: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.getAllFeatureFlags();
  }

  ngAfterContentChecked() {
    this.changeDetector.detectChanges();
  }

  getAllFeatureFlags() {
    this.isLoading = true;
    this.systemService
      .getAllSystemFeatures()
      .pipe(
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe((response: PaginationResponse) => {
        this.featureFlags = response.records;
      });
  }

  updateFeatureFlag(body) {
    this.disableUpdates = true;
    this.systemService
      .updateSystemFeature(body)
      .pipe(
        finalize(() => {
          this.disableUpdates = false;
        })
      )
      .subscribe((feature: any) => {
        this.featureFlags = this.featureFlags.map((item: any) => {
          if (item.name === feature.name) {
            item = feature;
          }
          return item;
        });
        this.toastService.showSuccess(
          `${feature.name} is ${feature.isEnabled ? 'enabled' : 'disabled'} successfully.`
        );
      });
  }

  toggleFeature(feature) {
    const body = {
      ...feature,
      isDisabled: !feature.isEnabled,
    };
    this.updateFeatureFlag(body);
  }
}
