import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {finalize} from 'rxjs/operators';
import {DispatchService, ToastService} from 'src/app/services';
import {setDateFromInlineInput} from 'src/app/utils';

@Component({
  selector: 'app-patient-dispatch-update',
  templateUrl: './patient-dispatch-update.component.html',
  styleUrls: ['./patient-dispatch-update.component.scss'],
})
export class PatientDispatchUpdateComponent implements OnInit {
  showFixmodal: boolean;
  @Output() cancel = new EventEmitter<any>();
  @Output() clearSelected = new EventEmitter<any>();
  @Input() selectedDispatchRecordIds = [];
  dispatchFixForm: FormGroup;
  dispatchSubmit = false;
  constructor(
    private fb: FormBuilder,
    private dispatchService: DispatchService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {}

  setDispatchFixForm() {
    this.dispatchFixForm = this.fb.group({
      hmsDeliveryDate: new FormControl(null, Validators.required),
      pickupDate: new FormControl(null, Validators.required),
      hmsPickupRequestDate: new FormControl(null, Validators.required),
    });
  }

  showFixModal() {
    this.showFixmodal = true;
    this.setDispatchFixForm();
  }

  closeFixModal(event?) {
    this.showFixmodal = false;
    this.dispatchFixForm.reset();
    this.cancel.emit();
  }
  checkFormValidity() {
    const {hmsDeliveryDate, pickupDate, hmsPickupRequestDate} = this.dispatchFixForm.value;
    if (!(hmsDeliveryDate || pickupDate || hmsPickupRequestDate)) {
      return true;
    }
    return false;
  }
  submitFix() {
    const {hmsDeliveryDate, pickupDate, hmsPickupRequestDate} = this.dispatchFixForm.value;
    const body = this.selectedDispatchRecordIds.map(x => {
      const obj = {
        dispatchRecordId: x,
        hmsDeliveryDate,
        hmsPickupRequestDate,
        pickupDate,
      };
      return obj;
    });
    if (body) {
      this.dispatchSubmit = true;
      this.dispatchService
        .updateDispatch(body)
        .pipe(
          finalize(() => {
            this.dispatchSubmit = false;
          })
        )
        .subscribe((response: any) => {
          this.toastService.showSuccess(`Fulfillment Dates fixed successfully`);
          this.clearSelected.emit(true);
          this.showFixmodal = false;
        });
    }
  }

  updateDate(event, field) {
    const dateToAppend = setDateFromInlineInput(event.target.value);
    if (dateToAppend) {
      this.dispatchFixForm.controls[field].setValue(new Date(dateToAppend));
    }
  }
}
