import {Component, OnInit} from '@angular/core';
import {ReportService} from '../../services/report.service';
import {Router} from '@angular/router';
import {isFeatureEnabled, IsPermissionAssigned} from '../../utils';

@Component({
  selector: 'app-report-portal',
  templateUrl: './report-portal.component.html',
  styleUrls: ['./report-portal.component.scss'],
})
export class ReportPortalComponent implements OnInit {
  reportCenterFeatureFlag = isFeatureEnabled('ReportPortal');

  constructor(private reportService: ReportService, private router: Router) {}

  ngOnInit(): void {
    if (!this.reportCenterFeatureFlag) {
      this.router.navigate(['/dashboard']);
    }
  }

  checkPermission(module, action) {
    return IsPermissionAssigned(module, action);
  }
}
