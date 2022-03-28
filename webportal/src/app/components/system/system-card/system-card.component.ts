import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {IsPermissionAssigned} from 'src/app/utils';

@Component({
  selector: 'app-system-card',
  templateUrl: './system-card.component.html',
  styleUrls: ['./system-card.component.scss'],
})
export class SystemCardComponent implements OnInit {
  @Input() loading = false;
  @Input() listCard = false;
  @Input() cardCount;
  @Input() header;
  @Input() showFix = true;
  @Input() groupTitle = '';
  @Input() toolList = [];
  @Input() patientInventoryWithIssues = false;
  @Output() getCount = new EventEmitter<any>();
  @Output() fix = new EventEmitter<any>();
  @Output() triggeredAction = new EventEmitter<any>();

  patientInventory = {
    orderNumber: '',
    assetTagNumber: '',
  };

  checkboxValue = true;

  constructor() {}

  ngOnInit(): void {}

  fixAction() {
    this.fix.emit();
  }

  getAction(issueType?: string) {
    if (this.patientInventoryWithIssues) {
      this.getCount.emit({
        patientInventory: {
          ...this.patientInventory,
          issue: issueType,
        },
      });
    } else {
      this.getCount.emit();
    }
  }

  canUpdateSystem() {
    return IsPermissionAssigned('System', 'Update');
  }
  triggerAction(event) {
    if (event?.checkbox) {
      event.data = {...event.data, isAssetTagged: this.checkboxValue};
    }
    this.triggeredAction.emit({
      action: event?.action,
      listTitle: this.groupTitle,
      tableHeaderLabel: event?.tableHeaderLabel,
      data: event?.data,
      headerBtnAction: event?.headerBtnAction,
      actionBtn: event?.actionBtn,
    });
  }
}
