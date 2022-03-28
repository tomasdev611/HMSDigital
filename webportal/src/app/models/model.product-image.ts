export interface ItemImageRequest {
  itemId: number;
  isUploaded?: boolean;
  name: string;
  description: string;
  contentType: string;
  sizeInBytes: number;
  isPublic?: boolean;
}

export interface ItemImageResponse {
  id: number;
  isUploaded: boolean;
  downloadUrl: string;
  storageTypeId: number;
  storageRoot: string;
  storageFilePath: string;
  name?: string;
  description?: string;
  contentType: string;
  sizeInBytes: number;
  isPblic: boolean;
}

export interface ItemImageBaseResponse {
  id: number;
  url: string;
}
