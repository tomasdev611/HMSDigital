import {Component, OnInit, Input, Output, OnChanges} from '@angular/core';
import {EventEmitter} from '@angular/core';
import {ToastService} from 'src/app/services';

@Component({
  selector: 'app-import-wizard',
  templateUrl: './import-wizard.component.html',
  styleUrls: ['./import-wizard.component.scss'],
})
export class ImportWizardComponent implements OnInit, OnChanges {
  // Input props
  @Output() fileUpload = new EventEmitter<any>(true);
  @Output() importData = new EventEmitter<any>();
  @Output() downloadSample = new EventEmitter<any>();
  @Output() finish = new EventEmitter<any>();
  @Input() mappedHeaders: [];
  @Input() isImportSuccess: boolean;
  @Input() validRecords: [];
  @Input() inValidRecords: [];
  @Input() loading: boolean;
  @Input() outroMessage: string;
  @Input() errors: [];

  isValid = false;
  items;
  totalPageCount: number;
  currentPage: number;
  selectedFiles = [];
  cardClasses = 'iw-card-container';
  fileClass = 'iw-file-container';

  constructor(private toasterService: ToastService) {}

  ngOnChanges() {
    this.isValid = !this.inValidRecords?.length ? true : false;
  }

  ngOnInit(): void {
    this.items = [
      {label: 'Select and upload file'},
      {label: 'Validate Records'},
      {label: 'Import'},
    ];
    this.totalPageCount = this.items.length;
    this.currentPage = 0;
  }

  nextPage() {
    switch (this.currentPage) {
      case 0:
        if (this.selectedFiles && this.selectedFiles.length) {
          const file = this.selectedFiles[0];
          this.fileUpload.emit(file);
        }
        break;
      case 1:
        if (this.selectedFiles && this.selectedFiles.length) {
          const file = this.selectedFiles[0];
          this.importData.emit(file);
        }
        break;
    }
    if (!this.isLastPage()) {
      this.currentPage += 1;
    }
  }

  previousPage() {
    if (this.currentPage !== 0) {
      this.currentPage -= 1;
    }
  }

  isFirstPage() {
    return this.currentPage === 0;
  }

  isLastPage() {
    return this.currentPage === this.totalPageCount - 1;
  }

  uploadFile({files}) {
    if (files && files.length) {
      this.selectedFiles = files;
      this.nextPage();
    }
  }

  CloseWizard() {
    this.finish.emit();
  }

  isNextDisabled() {
    switch (this.currentPage) {
      case 0:
        return !(this.selectedFiles && this.selectedFiles.length);
        break;
      case 1:
        return this.validRecords?.length ? !this.isValid : true;
        break;
    }
    return false;
  }

  clearFile() {
    this.selectedFiles = [];
  }

  selectFile(event) {
    const selectedFile = event.files[0];
    if (!selectedFile.name.endsWith('.csv') || !selectedFile.type.startsWith('application/')) {
      this.toasterService.showError('File Type not compatible');
      return false;
    }
    return true;
  }

  sampleFile() {
    this.downloadSample.emit();
  }
}
