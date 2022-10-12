import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Album } from '../models/album.model';
import { Observable } from 'rxjs';
import { Photo } from '../models/photo.model';
import { Guid } from 'guid-typescript';
import { apiPaths, environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})

export class AlbumService {

  baseUrl = `${environment.baseUrl}${apiPaths.albums}`;

  constructor(private http: HttpClient) { }

  getAlbums(): Observable<Album[]> {
    return this.http.get<Album[]>(this.baseUrl);
  }

  getAlbum(id: Guid): Observable<Album> {
    return this.http.get<Album>(`${this.baseUrl}/${id}`);
  }

  createAlbum(album: Album): Observable<Album> {
    return this.http.post<Album>(this.baseUrl, album);
  }

  editAlbum(id: Guid, album: Album) {
    return this.http.put(`${this.baseUrl}/${id}`, album);
  }

  deleteAlbum(id: Guid) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  getAllPhotosFromAlbum(id: Guid): Observable<Photo[]> {
    return this.http.get<Photo[]>(`${this.baseUrl}/${id}${apiPaths.photos}`);
  }

  addPhotoToAlbums(albumId: Guid, photoId: Guid) {
    return this.http.post(`${this.baseUrl}/${albumId}${apiPaths.photos}`, photoId);
  }

  deletePhotoFromAlbums(albumId: Guid, photoId: Guid) {
    return this.http.delete(`${this.baseUrl}/${albumId}${apiPaths.photos}/${photoId}`);
  }

  uniqueTitleValidate(title: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.baseUrl}/validateTitleExists?title=${title}`);
  }
}
