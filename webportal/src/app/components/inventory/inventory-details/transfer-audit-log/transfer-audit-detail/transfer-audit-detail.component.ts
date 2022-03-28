import {Component, OnInit} from '@angular/core';
import {finalize} from 'rxjs/operators';
import {InventoryService} from 'src/app/services';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-transfer-audit-detail',
  templateUrl: './transfer-audit-detail.component.html',
  styleUrls: ['./transfer-audit-detail.component.scss'],
})
export class TransferAuditDetailComponent implements OnInit {
  logId: number;
  itemId: number;
  itemAuditLog: any;
  loading = false;

  headers = [
    {label: 'Field Name', field: 'columnName', sortable: true},
    {label: 'Original Value', field: 'originalValue'},
    {label: '', field: '', bodyIcon: 'pi pi-arrow-right'},
    {label: 'New Value', field: 'newValue'},
  ];
  constructor(private inventoryService: InventoryService, private route: ActivatedRoute) {
    const {paramMap} = this.route.snapshot;
    this.logId = Number(paramMap.get('logId'));
    this.itemId = Number(paramMap.get('itemId'));
  }

  ngOnInit(): void {
    this.loading = true;
    this.inventoryService
      .getAuditLogDetailById(this.itemId, this.logId)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe(response => {
        this.itemAuditLog = response;
      });
  }
}
