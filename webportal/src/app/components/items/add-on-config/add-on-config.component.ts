import {Component, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {finalize, map} from 'rxjs/operators';
import {ConfirmDialog} from 'src/app/models';
import {ItemsService, ToastService} from 'src/app/services';

@Component({
  selector: 'app-add-on-config',
  templateUrl: './add-on-config.component.html',
  styleUrls: ['./add-on-config.component.scss'],
})
export class AddOnConfigComponent implements OnInit {
  itemId: number;
  itemDetails: any;
  showAddEditDialog = false;
  loading = false;
  selectedAddOnGroup: any;

  addOnHeaders = [
    {
      label: 'Group Name',
      field: 'name',
    },
    {
      label: 'Allow Multiple Selection',
      field: 'allowMultipleSelectionString',
    },
    {
      label: 'Items Count',
      field: 'addOnGroupProducts.length',
    },
    {
      class: 'xs edit-group-btn',
      actionBtn: 'Edit Group',
      actionBtnIcon: 'pi pi-pencil',
    },
  ];

  @ViewChild('confirmDialog', {static: false}) confirmDialog;

  constructor(
    private route: ActivatedRoute,
    private itemService: ItemsService,
    private toastr: ToastService
  ) {
    const {paramMap} = this.route.snapshot;
    this.itemId = Number(paramMap.get('itemId'));
    this.getItemDetails();
  }

  ngOnInit(): void {}

  getItemDetails() {
    this.itemService.getItemDetailsById(this.itemId).subscribe(item => {
      this.itemDetails = item;
      this.itemDetails.addOnGroups.forEach(
        group => (group.allowMultipleSelectionString = group.allowMultipleSelection ? 'Yes' : 'No')
      );
    });
  }

  openConfigureGroupDialog() {
    this.showAddEditDialog = true;
  }

  closeAddonConfigDialog() {
    this.showAddEditDialog = false;
    this.selectedAddOnGroup = null;
  }

  addOrUpdateAddonGroup({groupId, addOnGroup}) {
    if (groupId) {
      const existingGroupIndex = this.itemDetails.addOnGroups.findIndex(ag => ag.id === groupId);
      if (existingGroupIndex >= 0) {
        this.itemDetails.addOnGroups[existingGroupIndex] = {...addOnGroup};
      }
    } else {
      this.itemDetails.addOnGroups.push(addOnGroup);
    }

    this.updateAddOns().subscribe((response: any) => {
      this.toastr.showSuccess(`New Add-on ${addOnGroup.name} added successfully`);
    });
  }

  updateAddOns() {
    const newAddOnGroups = this.itemDetails.addOnGroups.map(ag => {
      const group: any = {
        name: ag.name,
        allowMultipleSelection: ag.allowMultipleSelection,
        productIds: ag.addOnGroupProducts.map(agp => agp.itemId),
      };
      if (ag.id) {
        group.id = ag.id;
      }
      return group;
    });

    this.loading = true;
    return this.itemService.updateAddOnGroups(this.itemDetails.id, newAddOnGroups).pipe(
      finalize(() => (this.loading = false)),
      map((response: any) => {
        this.itemDetails = response;
        return response;
      })
    );
  }

  deleteAddOnGroup(group) {
    const confirmData = new ConfirmDialog();
    confirmData.message = `Are you sure you want to delete add on Group (${group.name})`;
    confirmData.header = `Delete Add on`;
    confirmData.data = {group};
    this.confirmDialog.showDialog(confirmData);
  }

  confirmAccepted(data) {
    const {group} = data;
    const groupIndex = this.itemDetails.addOnGroups.indexOf(group);
    if (groupIndex >= 0) {
      this.itemDetails.addOnGroups.splice(groupIndex, 1);
    }
    this.updateAddOns().subscribe((response: any) => {
      this.toastr.showSuccess(`Add on Group ${group.name} deleted Successfully`);
    });
  }

  editAddOnGroup(group) {
    this.selectedAddOnGroup = group;
    this.openConfigureGroupDialog();
  }

  getItemCategories() {
    return this.itemDetails?.categories.map(c => c.name).join(',');
  }

  getItemSubCategories() {
    return this.itemDetails?.subCategories.map(sc => sc.name).join(',');
  }
}
