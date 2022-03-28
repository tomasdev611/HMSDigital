import {Component, OnInit, EventEmitter, Output, OnChanges, Input, ViewChild} from '@angular/core';
import {UserService, ToastService} from 'src/app/services';
import {SieveRequest, PaginationResponse} from 'src/app/models';
import {TableVirtualScrollComponent} from 'src/app/common';
import {finalize} from 'rxjs/operators';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent implements OnInit, OnChanges {
  loading = false;
  @Input() filter;
  @Input() userResponse: PaginationResponse;
  @Input() users: any;
  @Output() getUsers = new EventEmitter<any>();
  @Input() headers = [];
  @Input() searchQuery: string;
  sorted = false;
  statusSubmit = false;
  userRequest = new SieveRequest();
  @ViewChild('userListTable ', {static: false})
  userListTable: TableVirtualScrollComponent;

  constructor(private userService: UserService, private toastService: ToastService) {}

  ngOnChanges(changes) {
    this.loading = false;
    this.userRequest.filters = this.filter;
    if (
      (changes.filter && !changes.filter.firstChange) ||
      (changes.searchQuery && !changes.searchQuery.firstChange)
    ) {
      this.userRequest.page = 1;
      this.emitGetUser();
    }
  }

  ngOnInit(): void {
    this.emitGetUser();
  }

  nextUsers({pageNum}) {
    if (!this.userResponse || pageNum > this.userResponse.totalPageCount) {
      return;
    }
    this.userRequest.page = pageNum;
    this.emitGetUser();
  }

  emitGetUser() {
    this.loading = true;
    this.getUsers.emit({
      userRequest: this.userRequest,
      searchQuery: this.searchQuery,
    });
  }

  toggleUserStatus(event) {
    const selectedUser = event.object;
    event = event.event;
    this.statusSubmit = true;
    (event.checked
      ? this.userService.enableUser(selectedUser.id)
      : this.userService.disableUser(selectedUser.id)
    )
      .pipe(
        finalize(() => {
          this.statusSubmit = false;
        })
      )
      .subscribe(
        (response: any) => {
          this.emitGetUser();
          this.toastService.showSuccess(
            `${selectedUser.firstName ? selectedUser.firstName : selectedUser.email} ${
              event.checked ? 'enabled' : 'disabled'
            } successfully`
          );
        },
        error => {
          console.log('error', error);
          this.emitGetUser();
          throw error;
        }
      );
  }

  sortUsers(event) {
    switch (event.order) {
      case 0:
        this.userRequest.sorts = '';
        break;
      case 1:
        this.userRequest.sorts = event.field;
        break;
      case -1:
        this.userRequest.sorts = '-' + event.field;
        break;
    }
    this.dataTablesReset();
    this.emitGetUser();
  }

  dataTablesReset() {
    if (this.userListTable) {
      this.userListTable.reset();
    }
    this.userRequest.page = 1;
  }
}
