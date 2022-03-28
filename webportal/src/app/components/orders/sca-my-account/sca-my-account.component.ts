import {Component, OnInit} from '@angular/core';
import {DomSanitizer, SafeResourceUrl} from '@angular/platform-browser';
import {ActivatedRoute, Router} from '@angular/router';
import {environment} from 'src/environments/environment';

@Component({
  selector: 'app-sca-my-account',
  templateUrl: './sca-my-account.component.html',
  styleUrls: ['./sca-my-account.component.scss'],
})
export class ScaMyAccountComponent implements OnInit {
  approveOrderIframeUrl: SafeResourceUrl;
  netSuiteOrderId =
    this.route.snapshot && this.route.snapshot.paramMap.get('netSuiteOrderId')
      ? this.route.snapshot.paramMap.get('netSuiteOrderId')
      : null;
  netSuiteCustomerId =
    this.route.snapshot && this.route.snapshot.queryParamMap.get('netSuiteCustomerId')
      ? this.route.snapshot.queryParamMap.get('netSuiteCustomerId')
      : null;
  pageInfo = [
    {hash: 'approval-policies', label: 'Manage Approvers'},
    {hash: 'orders-dashboard', label: 'Approve Orders'},
    {hash: 'orders-cancel', label: 'Cancel Orders'},
  ];
  urlHashIndex = 0;
  constructor(
    private sanitizer: DomSanitizer,
    private route: ActivatedRoute,
    private router: Router
  ) {
    if (this.router.url.startsWith('/hospice')) {
      this.urlHashIndex = 0;
    } else if (this.router.url.startsWith('/dashboard/approve-orders')) {
      this.urlHashIndex = 1;
    } else if (this.router.url.startsWith('/dashboard/cancel-orders')) {
      this.urlHashIndex = 2;
    } else {
      this.urlHashIndex = 0;
    }
  }

  ngOnInit(): void {
    let iframeUrl = `${environment.scaOrderingUrl}/sca/my_account.ssp#${
      this.pageInfo[this.urlHashIndex].hash
    }`;
    if (this.netSuiteOrderId) {
      iframeUrl += `?netSuiteOrderId=${this.netSuiteOrderId}`;
    }
    if (this.netSuiteCustomerId) {
      iframeUrl += `?netSuiteCustomerId=${this.netSuiteCustomerId}`;
    }
    this.approveOrderIframeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(iframeUrl);
  }
}
