import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {HospiceService, ToastService} from 'src/app/services';
import {HospiceMemberFields} from 'src/app/constants';
import {Location} from '@angular/common';

@Component({
  selector: 'app-hospice-member-mappings',
  templateUrl: './hospice-member-mappings.component.html',
  styleUrls: ['./hospice-member-mappings.component.scss'],
})
export class HospiceMemberMappingsComponent implements OnInit {
  hospiceId: number;
  inputMappings = {};
  outputMappings = {};
  fields = [];

  constructor(
    private hospiceService: HospiceService,
    private route: ActivatedRoute,
    private toaster: ToastService,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.fields = HospiceMemberFields;
    this.route.params.subscribe((params: any) => {
      this.hospiceId = params.hospiceId;
    });
    this.hospiceService
      .getHospiceMemberInputMapping(this.hospiceId, {mappedItemType: 'hospicemember'})
      .subscribe((response: any) => {
        this.inputMappings = response.column_name_mapping;
      });
    this.hospiceService
      .getHospiceMemberOutputMapping(this.hospiceId, {mappedItemType: 'hospicemember'})
      .subscribe((res: any) => {
        this.outputMappings = res.column_name_mapping;
      });
  }

  saveMappings({mappings, type}) {
    if (type === 'input') {
      this.hospiceService
        .updateHospiceMemberInputMapping(this.hospiceId, {column_name_mapping: mappings})
        .subscribe(
          (res: any) => {
            this.inputMappings = res.column_name_mapping;
            this.toaster.showSuccess('Input Mappings Updated Successfully');
          },
          (err: any) => {
            this.toaster.showError('Error Ocurred While updating Input Mappings');
          }
        );
    } else if (type === 'output') {
      this.hospiceService
        .updateHospiceMemberOutputMapping(this.hospiceId, {column_name_mapping: mappings})
        .subscribe(
          (res: any) => {
            this.outputMappings = res.column_name_mapping;
            this.toaster.showSuccess('Output Mappings Updated Successfully');
          },
          (err: any) => {
            this.toaster.showError('Error Ocurred While updating Output Mappings');
          }
        );
    }
  }

  cancel() {
    this.location.back();
  }
}
