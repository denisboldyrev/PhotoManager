import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Guid } from 'guid-typescript';
import { Observable } from 'rxjs';
import { apiPaths, environment } from 'src/environments/environment';
import { Photo } from '../models/photo.model';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {
  baseUrl = `${environment.baseUrl}${apiPaths.photos}`;
  constructor(private http: HttpClient) { }

  getPhotos() {
    return this.http.get<Photo[]>(this.baseUrl);
  }

  getPhoto(id: Guid): Observable<Photo> {
    return this.http.get<Photo>(`${this.baseUrl}/${id}`);
  }

   uploadPhoto(file: File) {
    const formData: FormData = new FormData();
    formData.append('formFile', file, file.name);
    return this.http.post(this.baseUrl, formData);
  }

  editPhoto(id: Guid, photo: Photo) {
    return this.http.put( `${this.baseUrl}/${id}`, photo);
  }
  deletePhoto(id: Guid) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
  searchPhotos(param: string) {
    return this.http.get(`${this.baseUrl}/search/${param}`);
  }
}

