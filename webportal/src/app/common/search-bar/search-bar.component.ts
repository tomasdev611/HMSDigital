import {Component, OnInit, Output, EventEmitter, ViewChild, ElementRef, Input} from '@angular/core';
import {FormBuilder, FormControl, FormGroup} from '@angular/forms';
import {NavigationEnd, NavigationStart, Router} from '@angular/router';
import {map, debounceTime, distinctUntilChanged} from 'rxjs/operators';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.scss'],
})
export class SearchBarComponent implements OnInit {
  @Output() search = new EventEmitter<any>();
  @ViewChild('searchBarField', {static: true}) searchBarField: ElementRef;
  @Input() searchPreservedRoutes = [];
  searchForm: FormGroup;
  lastParentUrl = '';
  constructor(private fb: FormBuilder, private router: Router) {
    this.searchForm = this.fb.group({
      query: new FormControl(''),
    });
    this.router.events.subscribe(event => {
      if (event instanceof NavigationStart || event instanceof NavigationEnd) {
        if (this.searchPreservedRoutes.some(route => event.url.includes(route))) {
          if (event instanceof NavigationEnd) {
            const url = event.url.split('/')[1];
            if (this.lastParentUrl !== url) {
              this.lastParentUrl = url;
              this.clearInput();
            }
          }
        } else {
          this.clearInput();
        }
      }
    });
    this.searchForm
      .get('query')
      .valueChanges.pipe(
        map((query: any) => query),
        debounceTime(1000),
        distinctUntilChanged()
      )
      .subscribe((query: string) => {
        this.searchForm.controls.query.setValue(query);
        this.search.emit(this.searchForm.value.query);
        sessionStorage.setItem('navSearch', query);
      });
  }

  clearInput() {
    this.searchForm.controls.query.setValue('');
    this.search.emit(this.searchForm.value.query);
    sessionStorage.setItem('navSearch', '');
  }

  ngOnInit(): void {}
}
