import {Component, Input, OnInit} from '@angular/core';
import {HospiceMemberService, HospiceLocationService} from 'src/app/services';
import {finalize} from 'rxjs/operators';
import {ActivatedRoute} from '@angular/router';
import {IsPermissionAssigned, redirectToSCA} from 'src/app/utils';
import {SieveRequest, FilterConfiguration, PaginationResponse} from 'src/app/models';
import {SieveOperators, FilterTypes} from 'src/app/enums';

@Component({
  selector: 'app-hospice-member',
  templateUrl: './hospice-member.component.html',
  styleUrls: ['./hospice-member.component.scss'],
})
export class HospiceMemberComponent implements OnInit {
  hospiceId: number = Number(this.route.snapshot.paramMap.get('hospiceId'));
  membersResponse: PaginationResponse;
  membersLoading = false;
  membersHeaders = [
    {label: 'Name', field: 'name'},
    {label: 'Email Address', field: 'email', sortable: true},
    {label: 'Phone Number', field: 'phoneNumber', fieldType: 'Phone'},
  ];
  membersRequest = new SieveRequest();
  filterConfiguration: FilterConfiguration[] = [];
  @Input() netSuiteCustomerId;

  constructor(
    private route: ActivatedRoute,
    private hospiceMemberService: HospiceMemberService,
    private hospiceLocationService: HospiceLocationService
  ) {
    this.addMemberUserEditButton();
    this.route.params.subscribe((params: any) => {
      this.hospiceId = params.hospiceId;
      this.getMembers();
      this.getHospiceLocations();
    });
  }

  ngOnInit(): void {}

  getHospiceLocations() {
    this.hospiceLocationService.getHospiceLocations(this.hospiceId).subscribe((res: any) => {
      res = res.records.map(r => ({label: r.name, value: r.id}));
      const filteConfigforlocation = {
        label: 'Location',
        field: 'hospiceLocationId',
        operator: SieveOperators.Equals,
        type: FilterTypes.MultiSelect,
        value: res,
      };
      this.filterConfiguration.push(filteConfigforlocation);
    });
  }

  filterMembers({filterString}) {
    this.membersRequest.filters = filterString;
    this.getMembers();
  }

  getMembers() {
    this.membersLoading = true;
    this.hospiceMemberService
      .getAllHospiceMembers(this.hospiceId, this.membersRequest)
      .pipe(finalize(() => (this.membersLoading = false)))
      .subscribe((res: PaginationResponse) => {
        this.membersResponse = res;
      });
  }

  nextMembers({pageNum}) {
    if (!this.membersResponse || pageNum > this.membersResponse.totalPageCount) {
      return;
    }
    this.membersRequest.page = pageNum;
    this.getMembers();
  }

  addMemberUserEditButton() {
    if (IsPermissionAssigned('Hospice', 'Update')) {
      const memberEditBtn = {
        label: '',
        field: '',
        class: 'xs',
        editBtn: 'Edit Member',
        editBtnIcon: 'pi pi-pencil',
        editBtnLink: './members/edit',
        linkParams: 'id',
      };
      this.membersHeaders.push(memberEditBtn);
    }
  }
  hasPermission(entity, permission) {
    return IsPermissionAssigned(entity, permission);
  }

  goToManageApprovers() {
    const params = new URLSearchParams();
    params.append('netSuiteCustomerId', this.netSuiteCustomerId);
    const redirectUrl = `/hospice/${this.hospiceId}/manage-approvers/?${params.toString()}`;
    redirectToSCA(redirectUrl);
  }
}
