import {animate, state, style, transition, trigger} from '@angular/animations';
import {
  Component,
  Input,
  ElementRef,
  ViewChild,
  AfterViewInit,
  Output,
  EventEmitter,
} from '@angular/core';

@Component({
  selector: 'app-image-gallery',
  templateUrl: './image-gallery.component.html',
  styleUrls: ['./image-gallery.component.scss'],
  animations: [
    trigger('imageAnimation', [
      state(
        'show-image',
        style({
          opacity: '1',
        })
      ),
      state(
        'hide-image',
        style({
          opacity: '0',
        })
      ),
      transition('show-image <=> hide-image', animate('1000ms ease-in')),
    ]),
  ],
})
export class ImageGalleryComponent implements AfterViewInit {
  @ViewChild('imageLoaded', {static: false}) imageLoaded: ElementRef;
  @Output() clickImageReady: EventEmitter<boolean> = new EventEmitter();

  @Input() url: string;
  @Input() isClickable: boolean;

  imageControl = 'hide-image';
  contentControl = 'show-image';
  isImageLoaded = false;

  constructor() {}

  ngAfterViewInit(): void {
    this.loadImage(this.url);

    this.imageLoaded.nativeElement.onload = () => {
      this.imageControl = 'show-image';
      this.contentControl = 'hide-image';
      this.isImageLoaded = this.isClickable;
    };
  }

  loadImage(urlImage): void {
    this.imageControl = 'hide-image';
    this.contentControl = 'show-image';
    this.imageLoaded.nativeElement.src = urlImage;
  }

  canClickOnImage(): void {
    if (this.isImageLoaded) {
      this.clickImageReady.emit();
    }
  }
}
