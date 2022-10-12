import { Component, Input, OnInit } from '@angular/core';
import { Photo } from 'src/app/core/models/photo.model';
import { apiPaths, environment, imgPaths } from 'src/environments/environment';

@Component({
  selector: 'app-photo',
  templateUrl: './photo.component.html',
  styleUrls: ['./photo.component.css']
})
export class PhotoComponent implements OnInit {
  @Input() photo: Photo;
  uploadImgUrl = environment.uploadUrl;
  constructor() { }

  ngOnInit() {
  }
  public createImgPath = (fileName: string) => {
    return `${this.uploadImgUrl}${imgPaths.thumbnail}/${fileName}`;
  }
}
