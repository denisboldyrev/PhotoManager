import { Component, Input, OnInit } from '@angular/core';
import { Album } from 'src/app/core/models/album.model';

@Component({
  selector: 'app-albums',
  templateUrl: './albums.component.html',
  styleUrls: ['./albums.component.css']
})
export class AlbumsComponent {
@Input() albums: Album[];
  constructor() { }
}
