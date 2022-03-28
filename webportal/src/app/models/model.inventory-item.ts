import {Item} from './model.item';

export interface InventoryItem {
  id: number;
  count: number;
  serialNumber: string;
  itemId: number;
  item: Item;
  statusId: number;
  status: string;
  currentLocationType: string;
  currentLocationTypeId: number;
  currentLocationId: number;
  netSuiteInventoryId: number;
  assetTagNumber: string;
  lotNumber: string;
  quantityAvailable?: number;
}
