import { Injectable } from '@angular/core';
import { PhotoService } from './photo.service';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {

  constructor(private photoService: PhotoService) { }

  upload(file: File) {
   return this.photoService.uploadPhoto(file);
  }
}
