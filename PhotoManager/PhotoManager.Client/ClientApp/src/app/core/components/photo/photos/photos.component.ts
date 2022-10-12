import { Component, Input} from '@angular/core';
import { Photo } from 'src/app/core/models/photo.model';

@Component({
  selector: 'app-photos',
  templateUrl: './photos.component.html',
  styleUrls: ['./photos.component.css'],
})

export class PhotosComponent {
  @Input() photos: Photo[];
  constructor() {}
}
