import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {forkJoin} from 'rxjs';
import {finalize} from 'rxjs/operators';
import {SieveOperators} from 'src/app/enums';
import {ItemsService, ToastService} from 'src/app/services';
import {buildFilterString} from 'src/app/utils';

@Component({
  selector: 'app-equipment-settings-config',
  templateUrl: './equipment-settings-config.component.html',
  styleUrls: ['./equipment-settings-config.component.scss'],
})
export class EquipmentSettingsConfigComponent implements OnInit {
  equipmentSettingTypes = [];
  selectedEquipmentSettingType: any;
  loading = false;

  @Input() isConfigDialogOpen = false;
  @Input() equipmentSettingsConfig: any[];
  @Input() item: any;
  @Output() closeEqConfigDialog = new EventEmitter<any>();
  @Output() reloadDetailsView = new EventEmitter<any>();

  constructor(private itemService: ItemsService, private toast: ToastService) {}

  ngOnInit(): void {}

  searchEqSettingTypes({query}) {
    if (query) {
      this.equipmentSettingTypes = [{equipmentSettingTypeName: query, isQuery: true}];
    }
    const filter = [
      {
        field: 'name',
        operator: SieveOperators.CI_Contains,
        value: [query],
      },
    ];
    const request = {
      filters: buildFilterString(filter),
    };
    this.itemService.getEquipmentSettingTypes(request).subscribe((response: any) => {
      this.equipmentSettingTypes = response.map(eqType => {
        return {
          equipmentSettingTypeId: eqType.id,
          equipmentSettingTypeName: eqType.name,
        };
      });
      if (query) {
        this.equipmentSettingTypes.splice(0, 0, {
          equipmentSettingTypeName: query,
          isQuery: true,
        });
      }
    });
  }

  eqConfigSelected(selectedItem) {
    const existingEqConfig = this.equipmentSettingsConfig.find(
      esc =>
        esc.equipmentSettingTypeId === selectedItem.equipmentSettingTypeId &&
        esc.equipmentSettingTypeName === selectedItem.equipmentSettingTypeName
    );
    if (!existingEqConfig) {
      if (selectedItem.isQuery) {
        delete selectedItem.isQuery;
      }
      this.equipmentSettingsConfig.push(selectedItem);
    }
    selectedItem = null;
    this.selectedEquipmentSettingType = null;
  }

  removeItem(item) {
    const index = this.equipmentSettingsConfig.findIndex(esc => {
      return (
        esc.equipmentSettingTypeId === item.equipmentSettingTypeId &&
        esc.equipmentSettingTypeName === item.equipmentSettingTypeName
      );
    });
    this.equipmentSettingsConfig.splice(index, 1);
  }

  closeEquipmentSettingsConfigurationDialog() {
    this.closeEqConfigDialog.emit();
  }

  saveEquipmentSettingsConfiguartion() {
    this.loading = true;
    this.itemService
      .updateEquipmentSettingConfig(this.item.id, this.equipmentSettingsConfig)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe((response: any) => {
        this.fetchUpdatedItem(response);
        this.toast.showSuccess(`Equipment Setting updated for item ${response.name}`);
        this.closeEqConfigDialog.emit();
      });
  }

  fetchUpdatedItem(itemDetails) {
    this.itemService.getItemDetailsById(itemDetails.id).subscribe(itemResponse => {
      this.reloadDetailsView.emit({
        currentRow: {item: itemResponse},
      });
    });
  }
}
