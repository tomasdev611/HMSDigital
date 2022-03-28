export interface ContinuationTokenResponse {
  records: any[];
  continuationToken: any;
}

export class BaseContinuationTokenResponse {
  records = [];
  continuationToken;
}
