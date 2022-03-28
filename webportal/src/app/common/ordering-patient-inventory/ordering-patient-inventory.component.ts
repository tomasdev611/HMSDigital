import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {Table, TableHeaderCheckbox} from 'primeng-lts/table';

@Component({
  selector: 'app-ordering-patient-inventory',
  templateUrl: './ordering-patient-inventory.component.html',
  styleUrls: ['./ordering-patient-inventory.component.scss'],
})
export class OrderingPatientInventoryComponent implements OnInit {
  selectedPatientInventory = [];
  headerCheckBox: TableHeaderCheckbox;
  patientInv = [];

  @Input() patientInventory = [];
  @Input() orderHeader = null;
  @Input() shouldGroupNonSerialisedItems = true;
  @Input() editmode;

  @Output() itemsSelected = new EventEmitter<Array<any>>();

  @ViewChild('headerCheckBox') set setHeaderCheckbox(checkbox: TableHeaderCheckbox) {
    this.headerCheckBox = checkbox;
    if (this.headerCheckBox) {
      this.headerCheckBox.updateCheckedState = this.checkHeadercheckboxState.bind(this);
    }
  }

  @ViewChild('invTable')
  private invTable: Table;

  constructor() {}

  ngOnInit(): void {
    if (this.shouldGroupNonSerialisedItems) {
      this.groupNonSerializedItems();
    }
    this.updateAvailableQuantity();
    this.setSelectedItems();
  }

  getAddress(address) {
    return `${address.addressLine1 ?? ''}${address.addressLine2 ? ' ' + address.addressLine2 : ''}${
      address.addressLine3 ? ' ' + address.addressLine3 : ''
    },
    ${address.city || ''}, ${address.state || ''} ${address.zipCode || 0} - ${
      address.plus4Code || 0
    }`;
  }

  getOrderNumberDomList(inventory) {
    const orderNumbersList = this.getOrderNumber(inventory)?.split(',');
    let list = ``;
    orderNumbersList?.forEach(orderNumber => {
      list += `<li>${orderNumber}</li>`;
    });
    return list;
  }

  getOrderNumber(inventory) {
    if (inventory?.orderNumbers && Array.isArray(inventory?.orderNumbers)) {
      return inventory.orderNumbers.join(',');
    }
    return inventory.orderNumber;
  }

  checkHeadercheckboxState(): boolean {
    const records: any[] = this.invTable.filteredValue || this.invTable.value;
    const selection: any[] = this.invTable.selection?.filter(a => a.availableQuantity > 0);
    records.forEach(r => {
      if (!selection.find(s => s.id === r.id) && r.fulfilledQuantity) {
        selection.push(r);
      }
    });
    if (!selection) {
      this.updateHeaderCheckBoxState(null);
      return false;
    }
    this.invTable.selection = [...selection];
    this.invTable.updateSelectionKeys();
    if (records.length === selection.length) {
      this.updateHeaderCheckBoxState('all');
      return true;
    } else if (selection.length > 0) {
      this.updateHeaderCheckBoxState('partial');
      return true;
    } else {
      this.updateHeaderCheckBoxState(null);
      return false;
    }
  }

  updateHeaderCheckBoxState(checkedState) {
    if (this.headerCheckBox?.boxViewChild?.nativeElement?.className) {
      let className = 'p-checkbox-box';
      if (checkedState === 'partial' || checkedState === 'all') {
        className += ' p-highlight';
      }
      this.headerCheckBox.boxViewChild.nativeElement.className = className;
    }
    if (this.headerCheckBox?.boxViewChild?.nativeElement?.children[0]?.className) {
      let className = 'p-checkbox-icon';
      if (checkedState === 'all') {
        className += ' pi pi-check';
      } else if (checkedState === 'partial') {
        className += ' pi pi-minus';
      }
      this.headerCheckBox.boxViewChild.nativeElement.children[0].className = className;
    }
  }

  groupNonSerializedItems() {
    this.patientInventory = this.patientInventory?.reduce(this.groupNonSerializedItemsReducer, []);
  }

  groupNonSerializedItemsReducer(groupedInvs, inventory) {
    const nonSerializedItem = groupedInvs.find(
      gi => gi.itemId === inventory.itemId && !gi.serialNumber && !gi.assetTagNumber
    );
    if (nonSerializedItem) {
      nonSerializedItem.quantity += inventory.quantity;
      nonSerializedItem.isPartOfExistingPickup =
        inventory.isPartOfExistingPickup || nonSerializedItem.isPartOfExistingPickup;
      nonSerializedItem.existingPickupCount += inventory.existingPickupCount;
      nonSerializedItem.orderNumbers.push(inventory.orderNumber);
    } else {
      inventory.orderNumbers = [inventory.orderNumber];
      groupedInvs.push(inventory);
    }
    return groupedInvs;
  }

  updateAvailableQuantity() {
    if (!this.patientInventory || (this.editmode && !this.orderHeader)) {
      return;
    }

    this.patientInventory.forEach(inv => {
      inv.availableQuantity = inv.isPartOfExistingPickup
        ? inv.quantity - inv.existingPickupCount
        : inv.quantity;

      // add current order item count
      if (this.orderHeader) {
        const lineItem = this.findLineItemForInv(inv, 'pickup');
        if (lineItem) {
          inv.availableQuantity += lineItem.itemCount;
          inv.orderQuantity = lineItem.itemCount;

          // remove fulfilled item count
          const fulfilledLineItem = this.orderHeader.orderFulfillmentLineItems.find(foli => {
            return (
              foli.orderLineItemId === lineItem.id && foli.orderType.toLowerCase() === 'pickup'
            );
          });
          if (fulfilledLineItem) {
            inv.availableQuantity -= fulfilledLineItem.quantity;
            inv.fulfilledQuantity = this.orderHeader?.orderFulfillmentLineItems
              .filter(fli => fli.orderLineItemId === lineItem.id)
              .reduce((count, fli) => count + fli.quantity, 0);
          }
        }
      }

      if (!inv.orderQuantity) {
        inv.orderQuantity = inv.availableQuantity;
      }
    });
  }

  findLineItemForInv(patientInv, action) {
    return this.orderHeader?.orderLineItems?.find(
      (oli: any) =>
        oli.item.id === patientInv.itemId &&
        this.matchSerializedItem(oli, patientInv) &&
        action.toUpperCase() === oli.action.toUpperCase()
    );
  }

  matchSerializedItem(orderLineItem, invItem) {
    if (!(orderLineItem.item.isAssetTagged && orderLineItem.item.isSerialized)) {
      return true;
    }
    return (
      orderLineItem.assetTagNumber === invItem.assetTagNumber ||
      orderLineItem.serialNumber === invItem.serialNumber
    );
  }

  setSelectedItems() {
    if (!this.patientInventory || !this.editmode || !this.orderHeader) {
      return;
    }

    const selection = this.patientInventory.filter(inv => this.findLineItemForInv(inv, 'pickup'));
    this.selectedPatientInventory = [...selection];

    this.onItemsSelected();
  }

  onItemsSelected() {
    this.itemsSelected.emit(this.selectedPatientInventory);
  }

  getRowTooltip(inv, action) {
    const orderLineItem = this.findLineItemForInv(inv, action);

    if (orderLineItem) {
      const fulfilledItem = this.orderHeader?.orderFulfillmentLineItems.find(
        i => i.orderLineItemId === orderLineItem.id
      );

      if (fulfilledItem) {
        if (orderLineItem.itemCount === fulfilledItem.quantity) {
          return 'Item is completely fulfilled';
        } else {
          return 'Item is partially fulfilled';
        }
      }
    }

    if (inv.availableQuantity === 0) {
      return 'No items for this inventory are present';
    }
  }
}
