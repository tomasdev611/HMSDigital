import {Component, OnInit, Input} from '@angular/core';
import {Address} from 'src/app/models';

@Component({
  selector: 'app-address-details',
  templateUrl: './address-details.component.html',
  styleUrls: ['./address-details.component.scss'],
})
export class AddressDetailsComponent implements OnInit {
  @Input() address: Address;

  constructor() {}

  ngOnInit(): void {}
}
