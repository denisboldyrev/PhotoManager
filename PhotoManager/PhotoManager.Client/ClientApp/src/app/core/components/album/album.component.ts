import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { SubscriptionLike } from 'rxjs';
import { environment, imgPaths } from 'src/environments/environment';
import { Album } from '../../models/album.model';
import { AlbumService } from '../../services/album.service';

@Component({
  selector: 'app-album',
  templateUrl: './album.component.html',
  styleUrls: ['./album.component.css']
})
export class AlbumComponent implements OnInit, OnDestroy {
  @Input() album: Album;
  @Input() url: string;
  private subscription: SubscriptionLike;
  uploadImgUrl  = environment.uploadUrl;
  constructor(private albumService: AlbumService) { }

  ngOnInit() {
    this.subscription = this.albumService.getAllPhotosFromAlbum(this.album.id)
      .subscribe(data => this.album.photos = data);
  }
  setAlbumCover() {
    if (typeof this.album.photos === 'undefined') {
      return `${this.uploadImgUrl}${imgPaths.icons}/5396c2be-55ee-415d-b1a6-f56c222f64dc.jpg`;
    }
    if (this.album.photos.length > 0) {
      const fileName = this.album.photos[0].fileName;
      return `${this.uploadImgUrl}${imgPaths.thumbnail}/${fileName}`;
    }
      return `${this.uploadImgUrl}${imgPaths.icons}/5396c2be-55ee-415d-b1a6-f56c222f64dc.jpg`;
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
      this.subscription = null;
    }
  }
}
