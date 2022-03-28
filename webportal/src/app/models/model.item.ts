import {ItemCategory} from './model.item-category';

export interface Item {
  id: number;
  categories: ItemCategory[];
  subCategories: any[];
  name: string;
  productNumber: string;
  categoryId: number;
  description: string;
  itemNumber: string;
  isAssetTagged?: boolean;
  averageCost?: any;
  avgDeliveryProcessingTime?: any;
  avgPickUpProcessingTime?: any;
  depreciation?: any;
  cogsAccountName?: any;
  isLotNumbered?: boolean;
  isSerialized?: boolean;
}
