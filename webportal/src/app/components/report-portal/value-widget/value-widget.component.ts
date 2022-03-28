import {Component, OnInit, Input} from '@angular/core';

@Component({
  selector: 'app-value-widget',
  templateUrl: './value-widget.component.html',
  styleUrls: ['./value-widget.component.scss'],
})
export class ValueWidgetComponent implements OnInit {
  @Input() title: string;
  @Input() value: any;
  @Input() unit: any;
  @Input() loading = true;

  constructor() {}

  ngOnInit(): void {}
}
