import {Location} from '@angular/common';
import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';

@Component({
  selector: 'app-body-header',
  templateUrl: './body-header.component.html',
  styleUrls: ['./body-header.component.scss'],
})
export class BodyHeaderComponent implements OnInit {
  constructor(private location: Location) {}
  @Input() displayTitle: string;
  @Input() listCount: number;
  @Input() backText: string;
  @Input() actionBtnUrl: string;
  @Input() actionBtnText: string;
  @Input() showSearchBar: boolean;
  @Output() search = new EventEmitter<any>();

  searchQuery: string;
  suggestions = [];
  ngOnInit(): void {}

  pageSearch(query) {
    this.search.emit(query);
    this.suggestions = [];
  }

  goBack() {
    this.location.back();
  }
}
