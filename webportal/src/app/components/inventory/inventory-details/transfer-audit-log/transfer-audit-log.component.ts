import {Component, OnInit} from '@angular/core';
import {finalize} from 'rxjs/operators';
import {AuditService} from 'src/app/services';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-transfer-audit-log',
  templateUrl: './transfer-audit-log.component.html',
  styleUrls: ['./transfer-audit-log.component.scss'],
})
export class TransferAuditLogComponent implements OnInit {
  allLogs = [];
  logsLoading = false;
  itemId: number;
  logsHeaders = [
    {label: 'Action Type', field: 'auditAction', sortable: true},
    {
      label: 'Total Modified Fields',
      field: 'auditData.length',
      sortable: true,
    },
    {
      label: 'Modified by',
      field: 'user.name',
      sortable: true,
      bodyRoute: '/users/edit',
      bodyRouteParams: 'user.userId',
    },
    {
      label: 'Modified On',
      field: 'auditDate',
      sortable: true,
      fieldType: 'Date',
    },
  ];
  continuationToken = null;
  pageSize = 20;

  constructor(private auditService: AuditService, private route: ActivatedRoute) {
    const {paramMap} = this.route.snapshot;
    this.itemId = Number(paramMap.get('itemId'));
  }

  ngOnInit(): void {
    this.getInventoryAuditLogs();
  }

  getInventoryAuditLogs() {
    if (!this.continuationToken && this.allLogs.length > 0) {
      return;
    }

    this.logsLoading = true;
    const auditLogRequest: any = {
      apiLogType: 'Inventory',
      continuationToken: this.continuationToken,
      pageSize: this.pageSize,
      entityId: this.itemId,
    };

    this.auditService
      .getAuditLogs(auditLogRequest)
      .pipe(
        finalize(() => {
          this.logsLoading = false;
        })
      )
      .subscribe((response: any) => {
        if (response.apiLogs) {
          this.allLogs = this.allLogs.concat(response.apiLogs);
          this.continuationToken = response.continuationToken;
        }
      });
  }
}
