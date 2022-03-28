import {Component, OnInit, Output, EventEmitter, Input, OnChanges, ViewChild} from '@angular/core';
import {finalize} from 'rxjs/operators';
import {AuditService, ToastService, UserService} from 'src/app/services';
import {buildFilterString, deepCloneObject, exportFile} from 'src/app/utils';
import {
  AuditFilter,
  SieveRequest,
  PaginationResponse,
  FilterConfiguration,
  BaseContinuationTokenResponse,
} from 'src/app/models';
import {UserAuditActionTypes} from 'src/app/constants';
import {TableVirtualScrollComponent} from 'src/app/common';
import {FilterTypes, SieveOperators} from 'src/app/enums';

@Component({
  selector: 'app-audit-logs',
  templateUrl: './audit-logs.component.html',
  styleUrls: ['./audit-logs.component.scss'],
})
export class AuditLogsComponent implements OnInit {
  auditLogResponse = new BaseContinuationTokenResponse();
  continuationToken = null;
  selectedAuditLog: any;
  logsLoading = false;
  auditRequest: any = new SieveRequest();
  ActionTypes = UserAuditActionTypes;
  auditDetailsViewOpen = false;
  auditLogHeaders = [
    {label: 'Action Type', field: 'auditAction'},
    {
      label: 'Total Modified Fields',
      field: 'auditData.length',
    },
    {
      label: 'Modified by',
      field: 'user.name',
    },
    {
      label: 'Modified On',
      field: 'auditDate',
      fieldType: 'Date',
    },
    {
      label: 'Target User',
      field: 'targetUser.name',
    },
  ];

  @ViewChild('auditLogsTable ', {static: false})
  auditLogsTable: TableVirtualScrollComponent;

  auditFilter = new AuditFilter();
  @Output() toggleCsvLoading = new EventEmitter<any>();

  actionTypes = deepCloneObject(UserAuditActionTypes).filter((x: any) => {
    x.auditAction = x.name || '*';
    x.name = x.label;
    return x;
  });

  auditLogsFilterConfiguration: FilterConfiguration[] = [
    {
      label: 'Modified On(range)',
      field: 'auditDate',
      operator: SieveOperators.Equals,
      type: FilterTypes.DateRangePicker,
    },
    {
      label: 'Modified By(Name)',
      field: 'username',
      fields: ['userFirstName', 'userLastName'],
      type: FilterTypes.Autocomplete,
      value: [],
      operator: SieveOperators.CI_Contains,
    },
    {
      label: 'Action Type',
      field: 'auditAction',
      operator: SieveOperators.Equals,
      type: FilterTypes.Autocomplete,
      value: this.actionTypes,
    },
    {
      label: 'Target User',
      field: 'targetUser',
      fields: ['targetUserFirstName', 'targetUserLastName'],
      type: FilterTypes.Autocomplete,
      value: [],
      operator: SieveOperators.CI_Contains,
    },
  ];

  constructor(
    private auditService: AuditService,
    private userService: UserService,
    private toastService: ToastService
  ) {}

  ngOnInit() {
    this.getAuditLogs();
  }

  searchUsers(searchQuery, label) {
    const searchUserRequest = new SieveRequest();
    const params = {...searchUserRequest, searchQuery};

    this.userService.searchUser(params).subscribe((res: PaginationResponse) => {
      if (res && res.records) {
        const list = res.records.map(u => {
          return {
            userId: u.id,
            name: u.name,
          };
        });
        const index = this.auditLogsFilterConfiguration.findIndex(c => c.label === label);
        this.auditLogsFilterConfiguration[index].value = list;
      }
    });
  }

  getAuditLogs() {
    if (this.logsLoading || this.auditLogResponse.continuationToken === null) {
      return;
    }

    this.logsLoading = true;
    const auditLogRequest = this.prepareAuditLogRequest();
    const res = this.auditService
      .getAuditLogs(auditLogRequest)
      .pipe(
        finalize(() => {
          this.logsLoading = false;
        })
      )
      .subscribe((response: any) => {
        if (response.apiLogs) {
          this.auditLogResponse.records = this.auditLogResponse.records.concat(response.apiLogs);
          this.auditLogResponse.continuationToken = response.continuationToken;
          const userIds = response.apiLogs.map(l => l.entityId);
          this.fetchTargetUsersForAuditLog(userIds);
        }
      });
  }

  fetchTargetUsersForAuditLog(userIds) {
    const getUserRequest = new SieveRequest();
    const filterValues = [
      {
        field: 'id',
        operator: SieveOperators.Equals,
        value: userIds,
      },
    ];
    getUserRequest.filters = buildFilterString(filterValues);
    this.userService.getAllUsers(getUserRequest).subscribe((res: PaginationResponse) => {
      if (res.records) {
        res.records.forEach(r => {
          if (!r.targetUser) {
            const usersInAuditLogResponse = this.auditLogResponse.records.filter(
              u => u.entityId === r.id
            );
            usersInAuditLogResponse.forEach(u => {
              u.targetUser = r;
            });
          }
        });
      }
    });
  }

  prepareAuditLogRequest() {
    const auditLogRequest: any = {
      apiLogType: 'Users',
      continuationToken: this.auditLogResponse.continuationToken,
      pageSize: this.auditRequest.pageSize,
    };

    const {filtersMap} = this.auditRequest;

    if (!filtersMap) {
      return auditLogRequest;
    }
    if (filtersMap.hasOwnProperty('Modified On(range)')) {
      if (filtersMap['Modified On(range)'].value) {
        const [fromDate, toDate] = filtersMap['Modified On(range)'].value;
        if (fromDate && fromDate.value) {
          auditLogRequest.fromDate = fromDate?.value[0];
        }
        if (toDate && toDate.value) {
          auditLogRequest.toDate = toDate?.value[0];
        }
      }
    }

    if (filtersMap.hasOwnProperty('Action Type') && filtersMap['Action Type'].value) {
      auditLogRequest.actionType = filtersMap['Action Type'].value[0];

      if (auditLogRequest.actionType === '*') {
        delete auditLogRequest.actionType;
      }
    }

    if (filtersMap.hasOwnProperty('Modified By(Name)') && filtersMap['Modified By(Name)'].value) {
      auditLogRequest.userId = Number(filtersMap['Modified By(Name)'].value[0]?.userId);
    }

    if (filtersMap.hasOwnProperty('Target User') && filtersMap['Target User'].value) {
      auditLogRequest.entityId = Number(filtersMap['Target User'].value[0]?.userId);
    }
    return auditLogRequest;
  }

  filterChanged(changedFilters) {
    this.auditRequest.filters = changedFilters.filterString;
    this.auditRequest.filtersMap = changedFilters.filters;

    this.auditLogsTable.reset();
    this.auditLogResponse = new BaseContinuationTokenResponse();
    this.continuationToken = null;
    this.getAuditLogs();
  }

  fetchNextAuditLogPage() {
    if (!this.auditLogResponse) {
      return;
    }
    this.getAuditLogs();
  }

  exportUserAuditLogs() {
    const auditCsvRequest = {
      filters: this.auditRequest.filters,
      auditType: 'user',
    };
    this.auditService
      .getAuditLogsAsCsv(auditCsvRequest)
      .pipe(
        finalize(() => {
          this.toggleCsvLoading.emit(false);
        })
      )
      .subscribe((response: string) => {
        exportFile(response, 'UserAuditReport', 'csv');
      });
  }

  showAuditLogDetails({currentRow}) {
    this.auditDetailsViewOpen = true;
    this.selectedAuditLog = currentRow;
  }

  closeAuditLogDetails() {
    this.auditDetailsViewOpen = false;
    this.selectedAuditLog = null;
  }

  getEntityDetailsUrl() {
    return '/users/edit/';
  }
}
