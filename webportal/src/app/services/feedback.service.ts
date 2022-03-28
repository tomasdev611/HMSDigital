import {Injectable} from '@angular/core';
import {CoreApiService} from './http/core-api.service';

@Injectable({
  providedIn: 'root',
})
export class FeedbackService {
  constructor(private http: CoreApiService) {}

  submitFeedback(feedback: any) {
    return this.http.post(`feedback`, feedback);
  }
}
