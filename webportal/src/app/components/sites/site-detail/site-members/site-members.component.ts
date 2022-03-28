import {Component, OnInit} from '@angular/core';
import {IsPermissionAssigned} from 'src/app/utils';
import {ActivatedRoute} from '@angular/router';
import {SiteMembersService} from 'src/app/services';
import {finalize} from 'rxjs/operators';
import {PaginationResponse, SieveRequest} from 'src/app/models';

@Component({
  selector: 'app-site-members',
  templateUrl: './site-members.component.html',
  styleUrls: ['./site-members.component.scss'],
})
export class SiteMembersComponent implements OnInit {
  siteId: number = Number(this.route.snapshot.paramMap.get('siteId'));
  membersLoading = false;
  membersHeaders = [
    {label: 'Name', field: 'name'},
    {label: 'Designation', field: 'designation'},
    {label: 'Email Address', field: 'email', sortable: true},
  ];
  editMemberButton = {
    label: '',
    field: '',
    class: 'xs',
    editBtn: 'Edit Member',
    editBtnIcon: 'pi pi-pencil',
    editBtnLink: './members/edit',
    linkParams: 'id',
  };
  membersRequest = new SieveRequest();
  membersResponse: PaginationResponse;
  constructor(private route: ActivatedRoute, private siteMemberService: SiteMembersService) {}

  ngOnInit(): void {
    this.getMembers();
    this.addEditUserButton();
    this.addEditMemberButton();
  }

  getMembers() {
    this.membersLoading = true;
    this.siteMemberService
      .getAllSiteMembers(this.siteId, this.membersRequest)
      .pipe(
        finalize(() => {
          this.membersLoading = false;
        })
      )
      .subscribe((response: PaginationResponse) => {
        this.membersResponse = response;
      });
  }

  nextMembers({pageNum}) {
    const {totalPageCount} = this.membersResponse;
    if (pageNum > totalPageCount) {
      return;
    }

    this.membersRequest.page = pageNum;
    this.getMembers();
  }

  addEditUserButton() {
    if (IsPermissionAssigned('User', 'Update')) {
      (this.membersHeaders as any).push({
        label: '',
        field: '',
        class: 'xs',
        editBtn: 'Edit User',
        editBtnIcon: 'pi pi-user-edit',
        editBtnLink: '/users/edit',
        linkParams: 'userId',
      });
    }
  }

  addEditMemberButton() {
    if (IsPermissionAssigned('User', 'Update') && IsPermissionAssigned('Site', 'Update')) {
      this.membersHeaders.push(this.editMemberButton);
    }
  }

  checkPermission(entity: string, action: string) {
    return IsPermissionAssigned(entity, action);
  }
}
