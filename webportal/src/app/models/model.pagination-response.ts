export interface PaginationResponse {
  records: any[];
  pageSize: number;
  pageNumber: number;
  totalPageCount: number;
  totalRecordCount: number;
}

export class BasePaginationReponse implements PaginationResponse {
  records = [];
  pageSize = 0;
  pageNumber = 0;
  totalPageCount = 0;
  totalRecordCount = 0;
}
